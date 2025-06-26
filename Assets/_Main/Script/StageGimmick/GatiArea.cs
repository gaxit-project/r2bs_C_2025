using System.Xml.Serialization;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class GatiArea: MonoBehaviour
{
    public int[] areaTileMaxCnt;   // エリアのタイルの量を保存
    public int[] areaSecuredCnt;   // エリアの取得タイルの数
    public int[] areaHalfCnt;      // エリアのタイルの半分を保存
    private const float GATIAREA_PERCE = 0.7f;     // エリア取得の割合
    private const float GATIAREA_HALFPERCE = 0.5f; // エリアの半分の割合


    public int[] teamOneAreaCnt;  // 1つ目のチームのエリア取得数
    public int[] teamTwoAreaCnt;  // 2つ目のチームのエリア取得数

    private Color _bombColor; // 爆弾の色
    private Team _currentAreaTeamNam; // エリアを取得しているチームを保存

    public bool[] isAreaObtained; // エリアが取得されているかのフラグ


    public Transform[] GatiAreaGenerate; // エリアタイルの親オブジェクトを取得

    public static GatiArea Instance;
    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        for(int i = 0; i < GatiAreaGenerate.Length; i++)
        {
            teamOneAreaCnt[i] = 0;
            teamTwoAreaCnt[i] = 0;
            areaHalfCnt[i] = 0;
            isAreaObtained[i] = false;
            // エリアのタイル数をカウントする
            areaTileMaxCnt[i] = GatiAreaGenerate[i].childCount;
            areaSecuredCnt[i] = (int)(areaTileMaxCnt[i] * GATIAREA_PERCE);
            areaHalfCnt[i] = (int)(areaTileMaxCnt[i] * GATIAREA_HALFPERCE);
        }
    }


    /// <summary>
    /// 塗られていないエリアを塗る
    /// </summary>
    /// <param name="teamName"></param>
    public void AddGatiArea(Team teamName, Color bombColor, int type)
    {
        switch (teamName)
        {
            case Team.TeamOne:
                teamOneAreaCnt[type]++;
                break;
            case Team.TeamTwo:
                teamTwoAreaCnt[type]++;
                break;
        }
        SecuredGatiAreaJudge(teamName, bombColor, type);
    }



    /// <summary>
    /// エリアを上書きする
    /// </summary>
    /// <param name="teamName"></param>
    public void RemoveGatiArea(Team teamName, Color bombColor, int type)
    {
        switch (teamName)
        {
            case Team.TeamOne:
                teamOneAreaCnt[type]++;
                teamTwoAreaCnt[type]--;
                break;
            case Team.TeamTwo:
                teamTwoAreaCnt[type]++;
                teamOneAreaCnt[type]--;
                break;
        }
        SecuredGatiAreaJudge(teamName, bombColor, type);
    }




    /// <summary>
    /// エリアの塗が一定の値を超えたか確認+色の付与
    /// </summary>
    /// <param name="teamName"></param>
    private void SecuredGatiAreaJudge(Team teamName, Color bombColor, int type)
    {
        int areaTileCnt = 0;
        // チームごとのタイルの取得数の取得
        switch (teamName)
        {
            case Team.TeamOne:
                areaTileCnt = teamOneAreaCnt[type];
                _bombColor = bombColor;
                break;
            case Team.TeamTwo:
                areaTileCnt = teamTwoAreaCnt[type];
                _bombColor = bombColor;
                break;
        }


        // 取得数の比較
        if (areaTileCnt >= areaSecuredCnt[type] && !isAreaObtained[type])
        {
            BloomAllArea(teamName, _bombColor, type);
        }
        // もしエリア取得済の場合
        else if (isAreaObtained[type])
        {
            switch(_currentAreaTeamNam)
            {
                case Team.TeamOne:
                    areaTileCnt = teamTwoAreaCnt[type];
                    break;
                case Team.TeamTwo:
                    areaTileCnt = teamOneAreaCnt[type];
                    break;
            }
           
            // 現在のエリア取得チームでないチームがエリアの半分を塗ったらエリア占有解除
            if(areaTileCnt >= areaHalfCnt[type])
            {
                isAreaObtained[type] = false;
            }
        }
    }



    /// <summary>
    /// エリアをすべて塗る
    /// </summary>
    private void BloomAllArea(Team teamName, Color bombColor, int type)
    {
        isAreaObtained[type] = true;
        for (int i = 0; i < areaTileMaxCnt[type]; i++)
        {
            Transform child = GatiAreaGenerate[type].GetChild(i);
            Renderer renderer = child.GetComponent<Renderer>();
            GameObject obj = child.gameObject;
            if (renderer != null)
            {
                // 色変更
                BloomTile.Instance.TileChange(teamName, renderer, obj.transform.position);
                BloomEffect.Instance.CreateFlowerEffect(obj.transform.position, teamName);
                switch (teamName)
                {
                    case Team.TeamOne:
                        obj.layer = LayerMask.NameToLayer("TeamOneTile");
                        break;
                    case Team.TeamTwo:
                        obj.layer = LayerMask.NameToLayer("TeamTwoTile");
                        break;
                }
            }
        }
        switch (teamName)
        {
            case Team.TeamOne:
                teamOneAreaCnt[type] = areaTileMaxCnt[type];
                teamTwoAreaCnt[type] = 0;
                break;
            case Team.TeamTwo:
                teamTwoAreaCnt[type] = areaTileMaxCnt[type];
                teamOneAreaCnt[type] = 0;
                break;
        }
        CurrentSecuredGatiArea(teamName);
    }


    /// <summary>
    /// エリアを取得しているチームを保存
    /// </summary>
    /// <param name="teamName"></param>
    private void CurrentSecuredGatiArea(Team teamName)
    {
        _currentAreaTeamNam = teamName;
    }
}
