import os
import random
import numpy as np
import torch
import torch.nn as nn
import torch.optim as optim
from collections import deque
import socket
import json
import argparse
import matplotlib.pyplot as plt
# DQNのニューラルネットワーク
class DQN(nn.Module):
    def __init__(self, state_size, action_size):
        super(DQN, self).__init__()
        self.fc1 = nn.Linear(state_size, 512)
        self.fc2 = nn.Linear(512, 256)
        self.fc3 = nn.Linear(256, 128)
        self.fc4 = nn.Linear(128, action_size)

    def forward(self, x):
        x = torch.relu(self.fc1(x))
        x = torch.relu(self.fc2(x))
        x = torch.relu(self.fc3(x))
        return self.fc4(x)


class DQNAgent:
    def __init__(self, state_size, action_size):
        self.state_size = state_size
        self.action_size = action_size
        self.memory = deque(maxlen=2000)
        self.gamma = 0.99
        self.epsilon = 1.0
        self.epsilon_min = 0.01
        self.epsilon_decay = 0.995
        self.learning_rate = 0.001

        self.model = DQN(state_size, action_size)
        self.optimizer = optim.Adam(self.model.parameters(), lr=self.learning_rate)
        self.criterion = nn.MSELoss()

    def select_action(self, state):
        if np.random.rand() <= self.epsilon:
            return random.randrange(self.action_size)
        state = torch.FloatTensor(state).unsqueeze(0)
        act_values = self.model(state)
        return torch.argmax(act_values[0]).item()

    def observe(self, state, action, reward, next_state, done):
        self.memory.append((state, action, reward, next_state, done))
        self.replay(32)

    def replay(self, batch_size):
        if len(self.memory) < batch_size:
            return
        minibatch = random.sample(self.memory, batch_size)
        for state, action, reward, next_state, done in minibatch:
            state = torch.FloatTensor(state)
            next_state = torch.FloatTensor(next_state)
            target = reward
            if not done:
                target = reward + self.gamma * torch.max(self.model(next_state)).item()
            target_f = self.model(state)
            target_f = target_f.clone().detach()
            target_f[action] = target
            output = self.model(state)[action]
            loss = self.criterion(output, torch.tensor(target))
            self.optimizer.zero_grad()
            loss.backward()
            self.optimizer.step()
        if self.epsilon > self.epsilon_min:
            self.epsilon *= self.epsilon_decay

# 行動リスト
ACTIONS = ["forward", "backward", "right", "left", "wait", "set_bomb"]

# UDP通信設定
UDP_IP = "127.0.0.1"      # Unity側のIPアドレス
#UDP_PORT_RECEIVE = 5005   # Unity→Python（状態・報酬受信）
#UDP_PORT_SEND = 5006      # Python→Unity（行動送信）

def receive_json(sock):
    try:
        data, addr = sock.recvfrom(65536)
        return json.loads(data.decode()), addr
    except socket.timeout:
        return None, None

def send_json(sock, addr, obj):
    msg = json.dumps(obj).encode()
    sock.sendto(msg, addr)

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
MODEL_PATH = os.path.join(SCRIPT_DIR, "dqn_model2.pth")
REWARD_LOG_PATH = os.path.join(SCRIPT_DIR, "reward_log2.json")
REWARD_PLOT_PATH = os.path.join(SCRIPT_DIR, "reward_plot2.png")

def load_rewards():
    if os.path.exists(REWARD_LOG_PATH):
        with open(REWARD_LOG_PATH, "r") as f:
            return json.load(f)
    return []

def save_rewards(rewards):
    with open(REWARD_LOG_PATH, "w") as f:
        json.dump(rewards, f)

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('--receive_port', type=int, default=5005)
    parser.add_argument('--send_port', type=int, default=5006)
    args = parser.parse_args()

    UDP_PORT_RECEIVE = args.receive_port
    UDP_PORT_SEND = args.send_port
    print(f"受信ポート: {UDP_PORT_RECEIVE}, 送信ポート: {UDP_PORT_SEND}")
    state_size = 2815  # 状態の次元数（Unity側と合わせる）
    action_size = len(ACTIONS)
    agent = DQNAgent(state_size, action_size)

    # モデルがあればロード
    if os.path.exists(MODEL_PATH):
        agent.model.load_state_dict(torch.load(MODEL_PATH))
        print("学習済みモデルをロードしました")

    sock_receive = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sock_receive.bind((UDP_IP, UDP_PORT_RECEIVE))
    sock_receive.settimeout(5.0)  # 1秒ごとにタイムアウト
    sock_send = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    #Unity側のflag
    print("READY", flush=True)


    episode_reward = 0    # 今回の合計報酬

    try:
        while True:
            # 1. Unityから状態を受信
            state_data, unity_addr = receive_json(sock_receive)
            if state_data is None:
                break  # タイムアウト時は終了
            
            # print("受信したstate_data:", state_data)  # ここで内容を表示

            ground = np.array(state_data["ground"], dtype=np.float32)
            wall = np.array(state_data["wall"], dtype=np.float32)
            break_wall = np.array(state_data["breakWall"], dtype=np.float32)
            warp_rl = np.array(state_data["warpRL"], dtype=np.float32)
            warp_ud = np.array(state_data["warpUD"], dtype=np.float32)
            area = np.array(state_data["area"], dtype=np.float32)
            spawn = np.array(state_data["spawn"], dtype=np.float32)
            player = np.array(state_data["player"], dtype=np.float32)
            self_info = np.array(state_data["self"], dtype=np.float32)
            bomb = np.array(state_data["bomb"], dtype=np.float32)
            exp = np.array(state_data["exp"], dtype=np.float32)
            time = float(state_data["time"])
            done = state_data.get("done", False)

            state = np.concatenate([
                ground.flatten(),
                wall.flatten(),
                break_wall.flatten(),
                warp_rl.flatten(),
                warp_ud.flatten(),
                area.flatten(),
                spawn.flatten(),
                player.flatten(),
                self_info.flatten(),
                bomb.flatten(),
                exp.flatten(),
                np.array([time], dtype=np.float32)
            ])

            # 2. 行動を決定
            action_idx = agent.select_action(state)
            action = ACTIONS[action_idx]

            # 3. 行動をUnityへ送信
            send_json(sock_send, (unity_addr[0], UDP_PORT_SEND), {"action": action_idx})

            # 4. Unityから報酬・次状態を受信
            reward_data, _ = receive_json(sock_receive)
            if reward_data is None:
                break  # タイムアウト時は終了
            reward = reward_data["reward"]  # float型
            next_state_data = reward_data["next_state"]  # StateMsg形式（dict）
            done = reward_data.get("done", False)
            # 報酬加算
            episode_reward += reward

            # next_state_dataから状態ベクトルを生成
            next_state = np.concatenate([
                np.array(next_state_data["ground"], dtype=np.float32).flatten(),
                np.array(next_state_data["wall"], dtype=np.float32).flatten(),
                np.array(next_state_data["breakWall"], dtype=np.float32).flatten(),
                np.array(next_state_data["warpRL"], dtype=np.float32).flatten(),
                np.array(next_state_data["warpUD"], dtype=np.float32).flatten(),
                np.array(next_state_data["area"], dtype=np.float32).flatten(),
                np.array(next_state_data["spawn"], dtype=np.float32).flatten(),
                np.array(next_state_data["player"], dtype=np.float32).flatten(),
                np.array(next_state_data["self"], dtype=np.float32).flatten(),
                np.array(next_state_data["bomb"], dtype=np.float32).flatten(),
                exp.flatten(),
                np.array([time], dtype=np.float32)
            ])

            # 5. 学習
            agent.observe(state, action_idx, reward, next_state, done)
    finally:
        print("エピソード終了")
        sock_receive.close()
        sock_send.close()
        # モデル保存
        torch.save(agent.model.state_dict(), MODEL_PATH)
        print("学習済みモデルを保存しました")
        # 報酬保存
        episode_rewards = load_rewards()  # 報酬履歴
        episode_rewards.append(episode_reward)
        save_rewards(episode_rewards)  # ファイルに保存
        episode_reward = 0
        # グラフ表示
        plt.rcParams['font.family'] = 'MS Gothic'
        plt.plot(episode_rewards)
        plt.xlabel("エピソード（接続回数）")
        plt.ylabel("報酬合計")
        plt.title("Unity接続ごとの報酬合計")
        plt.savefig(REWARD_PLOT_PATH)
        print("グラフ画像を保存しました")