using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillController : MonoSing<SkillController>
{

    [SerializeField]
    public int MeteorCount
    {
        get => PlayerPrefs.GetInt("MeteorCount", 3);
        set
        {
            PlayerPrefs.SetInt("MeteorCount", value);
            meteorCountText.text = PlayerPrefs.GetInt("MeteorCount").ToString();

            if (MeteorCount > 0)
                meteorFrame.SetActive(true);
            else
                meteorFrame.SetActive(false);
        }
    }

    public int ShieldCount
    {
        get => PlayerPrefs.GetInt("ShieldCount", 3);
        set
        {
            PlayerPrefs.SetInt("ShieldCount", value);
            shieldCountText.text = PlayerPrefs.GetInt("ShieldCount").ToString();

            if (ShieldCount > 0)
                shieldFrame.SetActive(true);
            else
                shieldFrame.SetActive(false);
        }
    }

    [HideInInspector] public bool placeable;
    [SerializeField] private GameObject meteorFrame;
    [SerializeField] private GameObject shieldFrame;
    [SerializeField] private GameObject skillTimerPanel;
    [SerializeField] private TMPro.TMP_Text meteorCountText;
    [SerializeField] private TMPro.TMP_Text shieldCountText;
    [SerializeField] private TMPro.TMP_Text skillTimerText;
    [SerializeField] private Image timerFillImage;

    private void Awake() => Initial();

    private void Initial()
    {
        placeable = true;
        meteorCountText.text = MeteorCount.ToString();
        shieldCountText.text = ShieldCount.ToString();

        if (MeteorCount > 0)
            meteorFrame.SetActive(true);
        else
            meteorFrame.SetActive(false);

        if (ShieldCount > 0)
            shieldFrame.SetActive(true);
        else
            shieldFrame.SetActive(false);

    }

    public void ActivateTimerPanel()
    {
        MyFunc.DoVibrate();
        placeable = false;
        skillTimerPanel.SetActive(true);

        skillTimerText.IncrementalText(10, 0, UIManager.timeScale == 1 ? 10 : 5).SetEase(Ease.Linear);
        timerFillImage.FillImageAnimation(1, 0, UIManager.timeScale == 1 ? 10 : 5).SetEase(Ease.Linear).OnComplete(() => SkillTimerEnd());
    }

    private void SkillTimerEnd()
    {
        skillTimerPanel.SetActive(false);
        placeable = true;
    }
}
