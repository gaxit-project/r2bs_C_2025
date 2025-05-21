using UnityEngine;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    [Header("UI�X���C�_�[")]
    public Slider bgmSlider;
    public Slider seSlider;

    private const string BgmPrefKey = "BgmVolume";
    private const string SePrefKey = "SeVolume";

    void Start()
    {
        // �ۑ����ꂽ���ʐݒ��ǂݍ��݁i�Ȃ����1.0f�j
        float savedBgm = PlayerPrefs.GetFloat(BgmPrefKey, 1.0f);
        float savedSe = PlayerPrefs.GetFloat(SePrefKey, 1.0f);

        // SoundManager�ɔ��f
        SoundManager.BgmVolume = savedBgm;
        SoundManager.SEVolume = savedSe;

        // �X���C�_�[�ɔ��f
        bgmSlider.value = savedBgm;
        seSlider.value = savedSe;

        // �C�x���g�o�^
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        seSlider.onValueChanged.AddListener(SetSeVolume);

        // �Đ�����BGM�ɂ����f
        var source = SoundManager.GetBgmSource();
        if (source != null)
        {
            source.volume = savedBgm;
        }
    }

    public void SetBgmVolume(float volume)
    {
        SoundManager.BgmVolume = volume;
        PlayerPrefs.SetFloat(BgmPrefKey, volume);

        var source = SoundManager.GetBgmSource();
        if (source != null)
        {
            source.volume = volume;
        }
    }

    public void SetSeVolume(float volume)
    {
        SoundManager.SEVolume = volume;
        PlayerPrefs.SetFloat(SePrefKey, volume);
    }
}
