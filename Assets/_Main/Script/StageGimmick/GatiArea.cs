using System.Xml.Serialization;
using UnityEngine;

public class GatiArea: MonoBehaviour
{
    private int _areaTileMaxCnt = 0;   // エリアのタイルの量を保存
    private int _areaSecuredCnt = 0;   // エリアの取得タイルの数
    private int _areaHalfCnt = 0;      // エリアのタイルの半分を保存
    private const float GATIAREA_PERCE = 0.7f;     // エリア取得の割合
    private const float GATIAREA_HALFPERCE = 0.5f; // エリアの半分の割合


    private int _teamOneAreaCnt = 0;  // 1つ目のチームのエリア取得数
    private int _teamTwoAreaCnt = 0;  // 2つ目のチームのエリア取得数

    private Color _bombColor; // 爆弾の色
    private Team? _currentAreaTeamNam; // エリアを取得しているチームを保存

    private bool _isAreaObtained = false; // エリアが取得されているかのフラグ


    public Transform GatiAreaGenerate; // エリアタイルの親オブジェクトを取得

    public static GatiArea Instance;
    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        _teamOneAreaCnt = 0;
        _teamTwoAreaCnt = 0;
        _areaHalfCnt = 0;
        _isAreaObtained = false;
        // エリアのタイル数をカウントする
        _areaTileMaxCnt = GatiAreaGenerate.childCount;
        _areaSecuredCnt = (int)(_areaTileMaxCnt * GATIAREA_PERCE);
        _areaHalfCnt = (int)(_areaTileMaxCnt * GATIAREA_HALFPERCE);
    }


    /// <summary>
    /// 塗られていないエリアを塗る
    /// </summary>
    /// <param name="teamName"></param>
    public void AddGatiArea(Team teamName, Color bombColor)
    {
        switch (teamName)
        {
            case Team.TeamOne:
                _teamOneAreaCnt++;
                break;
            case Team.TeamTwo:
                _teamTwoAreaCnt++;
                break;
        }
        SecuredGatiAreaJudge(teamName, bombColor);
    }



    /// <summary>
    /// エリアを上書きする
    /// </summary>
    /// <param name="teamName"></param>
    public void RemoveGatiArea(Team teamName, Color bombColor)
    {
        switch (teamName)
        {
            case Team.TeamOne:
                _teamOneAreaCnt--;
                _teamTwoAreaCnt++;
                break;
            case Team.TeamTwo:
                _teamTwoAreaCnt--;
                _teamOneAreaCnt++;
                break;
        }
        SecuredGatiAreaJudge(teamName, bombColor);
    }




    /// <summary>
    /// エリアの塗が一定の値を超えたか確認+色の付与
    /// </summary>
    /// <param name="teamName"></param>
    private void SecuredGatiAreaJudge(Team teamName, Color bombColor)
    {
        int areaTileCnt = 0;
        // チームごとのタイルの取得数の取得
        switch (teamName)
        {
            case Team.TeamOne:
                areaTileCnt = _teamOneAreaCnt;
                _bombColor = bombColor;
                break;
            case Team.TeamTwo:
                areaTileCnt = _teamTwoAreaCnt;
                _bombColor = bombColor;
                break;
        }


        // 取得数の比較
        if (areaTileCnt >= _areaSecuredCnt && !_isAreaObtained)
        {
            BloomAllArea(teamName, _bombColor);
        }
        // もしエリア取得済の場合
        else if(_isAreaObtained)
        {
            switch(_currentAreaTeamNam)
            {
                case Team.TeamOne:
                    areaTileCnt = _teamTwoAreaCnt;
                    break;
                case Team.TeamTwo:
                    areaTileCnt = _teamOneAreaCnt;
                    break;
            }
            // 現在のエリア取得チームでないチームがエリアの半分を塗ったらエリア占有解除
            if(areaTileCnt >= _areaHalfCnt)
            {
                _isAreaObtained = false;
                _currentAreaTeamNam = null;
            }
        }
    }



    /// <summary>
    /// エリアをすべて塗る
    /// </summary>
    private void BloomAllArea(Team teamName, Color bombColor)
    {
        _isAreaObtained = true;
        for (int i = 0; i < _areaTileMaxCnt; i++)
        {
            Transform child = GatiAreaGenerate.GetChild(i);
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = bombColor;
            }
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
