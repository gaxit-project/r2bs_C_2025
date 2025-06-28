using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BloomResult : MonoBehaviour
{
    public TextMeshProUGUI TeamOnePer;
    public TextMeshProUGUI TeamTwoPer;
    public TextMeshProUGUI TeamOneCnt;
    public TextMeshProUGUI TeamTwoCnt;
    public TextMeshProUGUI Winner;
    public Slider perSlider1;
    public Slider perSlider2;
    public Slider cntSlider1;
    public Slider cntSlider2;

    public GameObject handle1;
    public GameObject handle2;
    public GameObject handle3;
    public GameObject handle4;

    public GameObject judge;

    public GameObject ResultPanel;

    private int _blueTile;
    private int _redTile;
    private int sumTile;

    private int _blueArea;
    private int _redArea;
    private int sumArea;

    /// <summary>
    /// Resultデータ（ScriptableObject）
    /// </summary>
    private ResultData _ResultData;
    void Start()
    {
        _ResultData = Resources.Load<ResultData>("ResultData");

#if !UNITY_EDITOR
    ResultDataIO.Load(_ResultData);
#endif

        _blueTile = _ResultData.blueTile+1;
        _redTile = _ResultData.redTile+1;
        sumTile = _blueTile + _redTile;
        perSlider1.maxValue = sumTile-2;
        perSlider2.maxValue = sumTile - 2;

        _blueArea = _ResultData.blueArea + 1;
        _redArea = _ResultData.redArea + 1;
        sumArea = _blueArea + _redArea;
        cntSlider1.maxValue = sumArea-2;
        cntSlider2.maxValue = sumArea-2;

        perSlider1.value = 0;
        perSlider2.value = 0;

        cntSlider1.value = 0;
        cntSlider2.value = 0;

        judge.SetActive(false);
        ResultPanel.SetActive(false);

        StartCoroutine(ResultTimeline());
    }

    private IEnumerator ResultTimeline()
    {
        judge.SetActive(true);
        yield return new WaitForSeconds(2f);
        judge.SetActive(false);
        Coroutine cr1 = StartCoroutine(BlueTileS());
        Coroutine cr2 = StartCoroutine(RedTileS());

        yield return cr1; yield return cr2;

        if (_blueTile == _redTile)
        {
            handle1.SetActive(false);
            handle2.SetActive(false);
        }
        else if (_blueTile > _redTile)
        {
            handle2.SetActive(false);
        }
        else
        {
            handle1.SetActive(false);
        }

        Coroutine cr3 = StartCoroutine(BlueAreaS());
        Coroutine cr4 = StartCoroutine(RedAreaS());

        

        yield return cr3; yield return cr4;

        if (_blueArea == _redArea)
        {
            handle3.SetActive(false);
            handle4.SetActive(false);
        }
        else if (_blueArea > _redArea)
        {
            handle4.SetActive(false);
        }
        else
        {
            handle3.SetActive(false);
        }


        if (_blueTile == _redTile)
        {
            Winner.text = "Draw";
            Winner.color = Color.white;
        }
        else if (_blueTile > _redTile)
        {
            Winner.text = "TeamBlueWin";
            Winner.color = Color.blue;
        }
        else
        {
            Winner.text = "TeamRedWin";
            Winner.color = Color.red;
        }

        yield return new WaitForSeconds(3f);

        if (_blueArea == _redArea)
        {
            Winner.text = "Draw";
            Winner.color = Color.white;
        }
        else if (_blueArea > _redArea)
        {
            Winner.text = "TeamBlueWin";
            Winner.color = Color.blue;
        }
        else
        {
            Winner.text = "TeamRedWin";
            Winner.color = Color.red;
        }

        yield return new WaitForSeconds(1f);

        ResultPanel.SetActive(true);


    }

    public void OnSkip()
    {
        StopAllCoroutines();
        perSlider1.value = _blueTile - 1;
        TeamOnePer.text = (((int)(((double)_blueTile / sumTile) * 100)) - ((int)(((double) 1 / sumTile)*100))).ToString()+"%";
        perSlider2.value = _redTile - 1;
        TeamTwoPer.text = (((int)(((double)_redTile / sumTile) * 100)) - ((int)(((double)1 / sumTile)*100))).ToString()+"%";
        cntSlider1.value = _blueArea - 1;
        TeamOneCnt.text = (_blueArea-1).ToString();
        cntSlider2.value = _redArea - 1;
        TeamTwoCnt.text = (_redArea-1).ToString();

        if (_blueArea == _redArea)
        {
            handle3.SetActive(false);
            handle4.SetActive(false);
            Winner.text = "Draw";
            Winner.color = Color.white;
        }
        else if (_blueArea > _redArea)
        {
            handle4.SetActive(false);
            Winner.text = "TeamBlueWin";
            Winner.color = Color.blue;
        }
        else
        {
            handle3.SetActive(false);
            Winner.text = "TeamRedWin";
            Winner.color = Color.red;
        }
        ResultPanel.SetActive(true);
    }

    private IEnumerator BlueTileS()
    {
        for (int i = 1; i <= _blueTile; i++)
        {
            perSlider1.value = i - 1;
            TeamOnePer.text = (((int)(((double)i / sumTile) * 100))-((int)(((double)1 / sumTile) * 100))).ToString() + "%";
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator RedTileS()
    {
        for (int i = 1; i <= _redTile; i++)
        {
            perSlider2.value = i - 1;
            TeamTwoPer.text = (((int)(((double)i / sumTile) * 100)) - ((int)(((double)1 / sumTile) * 100))).ToString() + "%";
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator BlueAreaS()
    {
        for (int i = 1; i <= _blueArea; i++)
        {
            cntSlider1.value = i - 1;
            TeamOneCnt.text = (i - 1).ToString();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator RedAreaS()
    {
        for (int i = 1; i <= _redArea; i++)
        {
            cntSlider2.value = i - 1;
            TeamTwoCnt.text = (i - 1).ToString();
            yield return new WaitForSeconds(1f);
        }
    }
}

        