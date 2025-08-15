using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using UnityEngine;

public class BloomRushAgent : PlayerBase
{
    public string pythonIP = "127.0.0.1";
    public int sendPort = 5005;
    public int receivePort = 5006;

    UdpClient udpSend;
    UdpClient udpReceive;
    IPEndPoint pythonEndPoint;

    private void Awake()
    {
        oneReward = 0;
        // ボムのPrefabと生成先オブジェクトの取得
        StandardBomb = Resources.Load<GameObject>("Prefab/StandardBomb");
        GameObject bombParentObj = GameObject.Find("BombGenerate");
        BombParent = bombParentObj.transform;

        //チーム分け
        playerIndex = GameObject.FindGameObjectsWithTag("TeamOne").Length + GameObject.FindGameObjectsWithTag("TeamTwo").Length; ;
        NPCTeamTag();
        TeamSplit();

        InitSpecialStatus();

    }
    private void NPCTeamTag()
    {
        if (GameObject.FindGameObjectsWithTag("TeamOne").Length < 2)
        {
            this.gameObject.tag = "TeamOne";
        }
        else
        {
            this.gameObject.tag = "TeamTwo";
        }
    }

    protected override void Start()
    {
        base.Start();

        udpSend = new UdpClient();
        udpReceive = new UdpClient(receivePort);
        pythonEndPoint = new IPEndPoint(IPAddress.Parse(pythonIP), sendPort);
        StartCoroutine(AgentLoop());
    }

    private void NPCMove(Vector2 dir)
    {
        moveInput = dir;
        if ((GameTimer.instance.IsGameStart()))
        {
            if (moveInput == Vector2.zero)
            {
                animator.SetBool("isWalking", false);
            }
            else
            {
                animator.SetBool("isWalking", true);
            }
        }
    }
    public void NPCBomb()
    {
        if (currentState == PlayerState.Alive && GameTimer.instance.IsGameStart())
        {
            BombPlacement(CatchPlayerPos());
            addReward(0.1f);
        }
    }


    IEnumerator AgentLoop()
    {
        while (true)
        {
            // 1. 状態送信
            StateMsg stateMsg = new StateMsg();
            stateMsg.wall = GetState2DArray("Wall");
            stateMsg.breakWall = GetState2DArray("BreakWall");
            stateMsg.done = IsEpisodeDone();
            string stateJson = JsonUtility.ToJson(stateMsg);
            byte[] stateBytes = Encoding.UTF8.GetBytes(stateJson);
            udpSend.Send(stateBytes, stateBytes.Length, pythonEndPoint);

            // 2. 行動受信
            IPEndPoint remoteEP = null;
            byte[] actionBytes = udpReceive.Receive(ref remoteEP);
            string actionJson = Encoding.UTF8.GetString(actionBytes);
            ActionMsg actionMsg = JsonUtility.FromJson<ActionMsg>(actionJson);
            int actionIdx = actionMsg.action;

            // 3. 行動を適用
            ApplyAction(actionIdx);

            // 4. 報酬送信
            RewardMsg rewardMsg = new RewardMsg();
            rewardMsg.reward = GetReward();
            rewardMsg.next_state = GetStateArray();
            rewardMsg.done = IsEpisodeDone();
            string rewardJson = JsonUtility.ToJson(rewardMsg);
            byte[] rewardBytes = Encoding.UTF8.GetBytes(rewardJson);
            udpSend.Send(rewardBytes, rewardBytes.Length, pythonEndPoint);

            yield return null;
        }

    }

    // 状態ベクトル例
    float[] GetStateArray()
    {
        return new float[10]; // 実装に合わせて
    }

    float[][] GetState2DArray(string name)
    {
        int i = 0;
        GameObject[] obj = GameObject.FindGameObjectsWithTag(name);
        float[][] stateObj = new float[obj.Length][];
        foreach (GameObject Obj in obj)
        {
            stateObj[i][0] = Obj.transform.position.x;
            stateObj[i++][1] = Obj.transform.position.z;
        }
        
        return stateObj; // 実装に合わせて
    }

    bool IsEpisodeDone()
    {
        return false;
    }

    void ApplyAction(int actionIdx)
    {
        switch (actionIdx)
        {
            case 0:
                NPCMove(new Vector2(0,1));
                break;
            case 1:
                NPCMove(new Vector2(0, -1));
                break;
            case 2:
                NPCMove(new Vector2(1, 0));
                break;
            case 3:
                NPCMove(new Vector2(-1, 0));
                break;
            case 4:
                NPCBomb();
                break;
        }
    }

    float GetReward()
    {
        float reward = oneReward;
        oneReward = 0f;
        return reward;
    }

    [System.Serializable]
    public class StateMsg
    {
        public float[][] wall;
        public float[][] breakWall;
        public float[][] area;
        public float[] player;
        public float[] enemy;
        public bool done;
    }

    [System.Serializable]
    public class ActionMsg
    {
        public int action;
    }

    [System.Serializable]
    public class RewardMsg
    {
        public float reward;
        public float[] next_state;
        public bool done;
    }
}