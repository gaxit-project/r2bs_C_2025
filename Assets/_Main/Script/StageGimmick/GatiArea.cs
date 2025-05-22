using System.Xml.Serialization;
using UnityEngine;

public class GatiArea: MonoBehaviour
{
    private int _areaTileMaxCnt = 0;   // �G���A�̃^�C���̗ʂ�ۑ�
    private int _areaSecuredCnt = 0;   // �G���A�̎擾�^�C���̐�
    private int _areaHalfCnt = 0;      // �G���A�̃^�C���̔�����ۑ�
    private const float GATIAREA_PERCE = 0.7f;     // �G���A�擾�̊���
    private const float GATIAREA_HALFPERCE = 0.5f; // �G���A�̔����̊���


    private int _teamOneAreaCnt = 0;  // 1�ڂ̃`�[���̃G���A�擾��
    private int _teamTwoAreaCnt = 0;  // 2�ڂ̃`�[���̃G���A�擾��

    private Color _bombColor; // ���e�̐F
    private Team? _currentAreaTeamNam; // �G���A���擾���Ă���`�[����ۑ�

    private bool _isAreaObtained = false; // �G���A���擾����Ă��邩�̃t���O


    public Transform GatiAreaGenerate; // �G���A�^�C���̐e�I�u�W�F�N�g���擾

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
        // �G���A�̃^�C�������J�E���g����
        _areaTileMaxCnt = GatiAreaGenerate.childCount;
        _areaSecuredCnt = (int)(_areaTileMaxCnt * GATIAREA_PERCE);
        _areaHalfCnt = (int)(_areaTileMaxCnt * GATIAREA_HALFPERCE);
    }


    /// <summary>
    /// �h���Ă��Ȃ��G���A��h��
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
    /// �G���A���㏑������
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
    /// �G���A�̓h�����̒l�𒴂������m�F+�F�̕t�^
    /// </summary>
    /// <param name="teamName"></param>
    private void SecuredGatiAreaJudge(Team teamName, Color bombColor)
    {
        int areaTileCnt = 0;
        // �`�[�����Ƃ̃^�C���̎擾���̎擾
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


        // �擾���̔�r
        if (areaTileCnt >= _areaSecuredCnt && !_isAreaObtained)
        {
            BloomAllArea(teamName, _bombColor);
        }
        // �����G���A�擾�ς̏ꍇ
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
            // ���݂̃G���A�擾�`�[���łȂ��`�[�����G���A�̔�����h������G���A��L����
            if(areaTileCnt >= _areaHalfCnt)
            {
                _isAreaObtained = false;
                _currentAreaTeamNam = null;
            }
        }
    }



    /// <summary>
    /// �G���A�����ׂēh��
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
    /// �G���A���擾���Ă���`�[����ۑ�
    /// </summary>
    /// <param name="teamName"></param>
    private void CurrentSecuredGatiArea(Team teamName)
    {
        _currentAreaTeamNam = teamName;
    }
}
