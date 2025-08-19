using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using UnityEngine;
using static MapManager;
using System.Linq;
using System.Diagnostics;
using System.IO;



public class BloomRushAgent : PlayerBase
{
    public string pythonIP = "127.0.0.1";

    public int sendPort = 5005;
    public int receivePort = 5006;

    UdpClient udpSend;
    UdpClient udpReceive;
    IPEndPoint pythonEndPoint;

    private bool soFirst = false;
    private bool oneBool = false;
    Process process = new Process();


    private const int GROUND = 500;
    private const int WALL = 200;
    private const int BREAKWALL = 100;
    private const int WARPRL = 20;
    private const int WARPUD = 20;
    private const int AREA = 100;
    private const int SPAWN = 50;
    private const int BOMB = 40;
    private const int EXP = 50;

    private float[][] Cground = new float[3][];
    private float[][] Cwall = new float[2][];
    private float[][] CbreakWall = new float[2][];
    private float[][] CwarpRL = new float[2][];
    private float[][] CwarpUD = new float[2][];
    private float[][] Carea = new float[3][];
    private float[][] Cspawn = new float[2][];


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
        sendPort += (playerIndex * 2);
        receivePort += (playerIndex * 2);
        StartPythonProcess();
        UnityEngine.Debug.Log("port" + sendPort + "," + receivePort);

    }
    private void StartPythonProcess()
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = Path.Combine(Application.dataPath, "../Python/Python311/python.exe"),
            Arguments = $"\"{Path.Combine(Application.dataPath, "../Python/BloomRushAi"+playerIndex.ToString()+".py")}\" --receive_port {sendPort} --send_port {receivePort}",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        process.StartInfo = psi;
        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null && e.Data.Contains("READY"))  // Python側で "READY" を出力させる
            {
                soFirst = true; // フラグだけ立てる
            }
        };
        process.ErrorDataReceived += (sender, e) => { if (e.Data != null) UnityEngine.Debug.LogError(e.Data); };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
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
        Cground[0] = new float[GROUND];
        Cground[1] = new float[GROUND];
        Cground[2] = new float[GROUND];
        Cwall[0] = new float[WALL];
        Cwall[1] = new float[WALL];
        CbreakWall[0] = new float[BREAKWALL];
        CbreakWall[1] = new float[BREAKWALL];
        CwarpRL[0] = new float[WARPRL];
        CwarpRL[1] = new float[WARPRL];
        CwarpUD[0] = new float[WARPUD];
        CwarpUD[1] = new float[WARPUD];
        Carea[0] = new float[AREA];
        Carea[1] = new float[AREA];
        Carea[2] = new float[AREA];
        Cspawn[0] = new float[SPAWN];
        Cspawn[1] = new float[SPAWN];
    }

    protected override void Update()
    {
        base.Update();
        if (soFirst)
        {
            udpSend = new UdpClient(sendPort);
            udpReceive = new UdpClient(receivePort);
            udpReceive.Client.ReceiveTimeout = 5000;
            pythonEndPoint = new IPEndPoint(IPAddress.Parse(pythonIP), sendPort);
            StartCoroutine(AgentLoop());
            soFirst = false;
        }
        if (IsEpisodeDone() && !oneBool)
        {
            oneBool = true;
            udpSend.Close();
            udpReceive.Close();
            UnityEngine.Debug.Log(playerIndex + "udp.close");
        }
    }

    private void NPCMove(Vector2 dir)
    {
        moveInput = dir;
        if ((GameTimer.instance.IsGameStart()))
        {
            anifirst = true;
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
        }
    }


    IEnumerator AgentLoop()
    {
        while (true)
        {
            try
            {
                // 1. 状態送信
                StateMsg stateMsg = GetStateMsg();
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
                rewardMsg.next_state = GetStateMsg();
                rewardMsg.done = IsEpisodeDone();
                string rewardJson = JsonUtility.ToJson(rewardMsg);
                byte[] rewardBytes = Encoding.UTF8.GetBytes(rewardJson);
                udpSend.Send(rewardBytes, rewardBytes.Length, pythonEndPoint);
            }catch (SocketException e)
            {
                UnityEngine.Debug.Log(e + ":pthonとの通信が途絶えました。");
                udpSend.Close();
                udpReceive.Close();
                FBSceneManager.Instance.LoadTeamSelectScene();
                break;
            }

            yield return null;
        }

    }

    /*// 状態ベクトル例
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
    }*/

    private StateMsg GetStateMsg()
    {
        GetMapState();
        StateMsg stateMsg = new StateMsg();
        stateMsg.ground = Flatten(Cground);
        stateMsg.wall = Flatten(Cwall);
        stateMsg.breakWall = Flatten(CbreakWall);
        stateMsg.warpRL = Flatten(CwarpRL);
        stateMsg.warpUD = Flatten(CwarpUD);
        stateMsg.area = Flatten(Carea);
        stateMsg.spawn = Flatten(Cspawn);
        stateMsg.player = GetPlayerState();
        stateMsg.self = GetSelfState();
        stateMsg.bomb = Flatten(GetBombState());
        stateMsg.exp = Flatten(GetExpState());
        stateMsg.time = GetTimeState();
        stateMsg.done = IsEpisodeDone();
        return stateMsg;
    }

    private void GetMapState()
    {
        int g = 0;
        int w = 0;
        int b = 0;
        int rl = 0;
        int ud = 0;
        int a = 0;
        int s = 0;
        MapBlockData[,] _mapState = MapManager.Instance.GetMapData();
        foreach(MapBlockData block in _mapState)
        {
            switch(block.name)
            {
                case "GroundObject":
                    Cground[0][g] = (float)block.gridPosition.x;
                    Cground[1][g] = (float)block.gridPosition.y;
                    Renderer renderer = block.instance.GetComponent<Renderer>();
                    if (this.gameObject.tag == "TeamOne")
                    {
                        if ((renderer.gameObject.layer == LayerMask.NameToLayer("TeamOneTile"))){ Cground[2][g++] = 1; }
                        else if ((renderer.gameObject.layer == LayerMask.NameToLayer("TeamTwoTile"))) { Cground[2][g++] = 2; }
                        else { Cground[2][g++] = 0; }

                    }
                    else
                    {
                        if ((renderer.gameObject.layer == LayerMask.NameToLayer("TeamTwoTile"))) { Cground[2][g++] = 1; }
                        else if ((renderer.gameObject.layer == LayerMask.NameToLayer("TeamOneTile"))) { Cground[2][g++] = 2; }
                        else { Cground[2][g++] = 0; }
                    }
                    break;
                case "WallObject":
                    Cwall[0][w] = (float)block.gridPosition.x;
                    Cwall[1][w++] = (float)block.gridPosition.y;
                    break;
                case "BreakWallObject":
                    CbreakWall[0][b] = (float)block.gridPosition.x;
                    CbreakWall[1][b++] = (float)block.gridPosition.y;
                    break;
                case "WarpRLObject":
                    CwarpRL[0][rl] = (float)block.gridPosition.x;
                    CwarpRL[1][rl++] = (float)block.gridPosition.y;
                    break;
                case "WarpUDObject":
                    CwarpUD[0][ud] = (float)block.gridPosition.x;
                    CwarpUD[1][ud++] = (float)block.gridPosition.y;
                    break;
                case "GatiAreaObject":
                    Carea[0][a] = (float)block.gridPosition.x;
                    Carea[1][a] = (float)block.gridPosition.y;
                    Carea[2][a++] = (float)block.type;
                    break;
                case "StartObject":
                    Cspawn[0][s] = (float)block.gridPosition.x;
                    Cspawn[1][s++] = (float)block.gridPosition.y;
                    break;
            }
        }
    }

    private float[] GetPlayerState()
    {
        int i = 0;
        GameObject[] team = GameObject.FindGameObjectsWithTag(this.gameObject.tag);
        GameObject[] enemy;
        if (this.gameObject.tag == "TeamOne")
        {
            enemy = GameObject.FindGameObjectsWithTag("TeamTwo");
        }
        else
        {
            enemy = GameObject.FindGameObjectsWithTag("TeamOne");
        }
        float[] player = new float[(team.Length+enemy.Length)*2];
        foreach (GameObject t in team)
        {
            player[i++] = (float)MapManager.Instance.WorldToGridPosition(t.transform.position).x;
            player[i++] = (float)MapManager.Instance.WorldToGridPosition(t.transform.position).y;
        }

        foreach (GameObject e in enemy)
        {
            player[i++] = (float)MapManager.Instance.WorldToGridPosition(e.transform.position).x;
            player[i++] = (float)MapManager.Instance.WorldToGridPosition(e.transform.position).y;
        }

        return player;
    }

    private float[] GetSelfState()
    {
        float[] self = {playerUI.Rlevel, playerUI.RnExp, playerUI.Rexp, playerUI.Rcnt, playerUI.BombRange, playerUI.Speed};
        return self;
    }

    private float[][] GetBombState()
    {
        int i = 0;
        GameObject[] bomb = GameObject.FindGameObjectsWithTag("FlowerBomb");
        float[][] bombState= new float[3][];
        bombState[0] = new float[BOMB];
        bombState[1] = new float[BOMB];
        bombState[2] = new float[BOMB];
        foreach (GameObject b in bomb)
        {
            bombState[0][i] = (float)MapManager.Instance.WorldToGridPosition(b.transform.position).x;
            bombState[1][i] = (float)MapManager.Instance.WorldToGridPosition(b.transform.position).y;
            if (this.gameObject.tag == "TeamOne")
            {
                if (b.GetComponent<BombProcess>()._teamName == Team.TeamOne) { bombState[2][i++] = 0; }
                else { bombState[2][i++] = 1; }
            }
            else
            {
                if (b.GetComponent<BombProcess>()._teamName == Team.TeamTwo) { bombState[2][i++] = 0; }
                else { bombState[2][i++] = 1; }
            }
        }
        return bombState;
    }

    private float[][] GetExpState()
    {
        int i = 0;
        GameObject[] Exp = GameObject.FindGameObjectsWithTag("Exp");
        float[][] expState = new float[2][];
        expState[0] = new float[EXP];
        expState[1] = new float[EXP];
        foreach (GameObject e in Exp)
        {
            expState[0][i] = (float)MapManager.Instance.WorldToGridPosition(e.transform.position).x;
            expState[1][i] = (float)MapManager.Instance.WorldToGridPosition(e.transform.position).y;
        }
        return expState;
    }

    private float GetTimeState()
    {
        return GameTimer.instance.GetTime();
    }


    bool IsEpisodeDone()
    {
        return GameTimer.instance.IsMapTimer0();
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
                NPCMove(new Vector2(0, 0));
                break;
            case 5:
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

    private float[] Flatten(float[][] array)
    {
        if (array == null) return new float[0];
        return array.SelectMany(x => x ?? new float[0]).ToArray();
    }

    [System.Serializable]
    public class StateMsg
    {
        public float[] ground;
        public float[] wall;
        public float[] breakWall;
        public float[] warpRL;
        public float[] warpUD;
        public float[] area;
        public float[] spawn;
        public float[] player;
        public float[] self;
        public float[] bomb;
        public float[] exp;
        public float time;
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
        public StateMsg next_state;
        public bool done;
    }
}