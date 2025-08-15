using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using UnityEngine;
using static MapManager;

public class BloomRushAgent : PlayerBase
{
    public string pythonIP = "127.0.0.1";
    public int sendPort = 5005;
    public int receivePort = 5006;

    UdpClient udpSend;
    UdpClient udpReceive;
    IPEndPoint pythonEndPoint;

    private float[][] Cground;
    private float[][] Cwall;
    private float[][] CbreakWall;
    private float[][] CwarpRL;
    private float[][] CwarpUD;
    private float[][] Carea;
    private float[][] Cspawn;


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

        GetMapState();
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
        stateMsg.ground = Cground;
        stateMsg.wall = Cwall;
        stateMsg.breakWall = CbreakWall;
        stateMsg.warpRL = CwarpRL;
        stateMsg.warpUD = CwarpUD;
        stateMsg.area = Carea;
        stateMsg.spawn = Cspawn;
        stateMsg.player = GetPlayerState();
        stateMsg.self = GetSelfState();
        stateMsg.bomb = GetBombState();
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
                    Cground[g][0] = block.gridPosition.x;
                    Cground[g][1] = block.gridPosition.y;
                    Renderer renderer = block.instance.GetComponent<Renderer>();
                    if (this.gameObject.tag == "TeamOne")
                    {
                        if ((renderer.gameObject.layer == LayerMask.NameToLayer("TeamOneTile"))){ Cground[g++][2] = 1; }
                        else if ((renderer.gameObject.layer == LayerMask.NameToLayer("TeamTwoTile"))) { Cground[g++][2] = 2; }
                        else { Cground[g ++][2] = 0; }

                    }
                    else
                    {
                        if ((renderer.gameObject.layer == LayerMask.NameToLayer("TeamTwoTile"))) { Cground[g++][2] = 1; }
                        else if ((renderer.gameObject.layer == LayerMask.NameToLayer("TeamOneTile"))) { Cground[g++][2] = 2; }
                        else { Cground[g++][2] = 0; }
                    }
                    break;
                case "WallObject":
                    Cwall[w][0] = block.gridPosition.x;
                    Cwall[w++][1] = block.gridPosition.y;
                    break;
                case "BreakWallObject":
                    CbreakWall[b][0] = block.gridPosition.x;
                    CbreakWall[b++][1] = block.gridPosition.y;
                    break;
                case "WarpRLObject":
                    CwarpRL[rl][0] = block.gridPosition.x;
                    CwarpRL[rl++][1] = block.gridPosition.y;
                    break;
                case "WarpUDObject":
                    CwarpUD[ud][0] = block.gridPosition.x;
                    CwarpUD[ud++][1] = block.gridPosition.y;
                    break;
                case "GatiAreaObject":
                    Carea[a][0] = block.gridPosition.x;
                    Carea[a][1] = block.gridPosition.y;
                    Carea[a++][2] = block.type;
                    break;
                case "StartObject":
                    Cspawn[s][0] = block.gridPosition.x;
                    Cspawn[s++][1] = block.gridPosition.y;
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
            player[i++] = MapManager.Instance.WorldToGridPosition(t.transform.position).x;
            player[i++] = MapManager.Instance.WorldToGridPosition(t.transform.position).y;
        }

        foreach (GameObject e in enemy)
        {
            player[i++] = MapManager.Instance.WorldToGridPosition(e.transform.position).x;
            player[i++] = MapManager.Instance.WorldToGridPosition(e.transform.position).y;
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
        float[][] bombState= new float[bomb.Length][];
        foreach (GameObject b in bomb)
        {
            bombState[i][0] = MapManager.Instance.WorldToGridPosition(b.transform.position).x;
            bombState[i][1] = MapManager.Instance.WorldToGridPosition(b.transform.position).y;
            if (this.gameObject.tag == "TeamOne")
            {
                if (b.GetComponent<BombProcess>()._teamName == Team.TeamOne) { bombState[i++][2] = 0; }
                else { bombState[i++][2] = 1; }
            }
            else
            {
                if (b.GetComponent<BombProcess>()._teamName == Team.TeamTwo) { bombState[i++][2] = 0; }
                else { bombState[i++][2] = 1; }
            }
        }
        return bombState;
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
        public float[][] ground;
        public float[][] wall;
        public float[][] breakWall;
        public float[][] warpRL;
        public float[][] warpUD;
        public float[][] area;
        public float[][] spawn;
        public float[] player;
        public float[] self;
        public float[][] bomb;
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