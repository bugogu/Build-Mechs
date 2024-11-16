using UnityEngine;

public class DailyPrize : MonoBehaviour
{
    public int prizeValue;
    public TMPro.TMP_Text prizeText;
    public UnityEngine.UI.Image prizeImage;
    public Sprite prizeImageSprite;
    public string prizeTitle;
    public TMPro.TMP_Text prizeTitleText;
    public Color elementBackground;
    public UnityEngine.UI.Image elementBackgroundImage;

    public GameObject lockImage;
    public GameObject tickImage;

    private void Awake()
    {
        prizeText.text = prizeValue.ToString();
        prizeImage.sprite = prizeImageSprite;
        prizeTitleText.text = prizeTitle;
        elementBackgroundImage.color = elementBackground;
    }

    public void ClaimDailyPrize()
    {
        if (prizeValue == 9999)
        {
            PlayerPrefs.SetString("Skin", "Claimed");
            GameManager.Instance.mechBaseMaterial.color = GameManager.Instance.mechClaimedSkinColor;
            GameManager.Instance.mechDissolveBaseMaterial.SetColor("_Albedo", GameManager.Instance.mechClaimedSkinColor);
        }

        else if (prizeValue >= 1000 && prizeValue < 5001)
            UIManager.Instance.UpdateCoin(prizeValue);

        else if (prizeValue < 1000)
            UIManager.Instance.UpdateDiamond(prizeValue);
    }

    public void ClaimedEffect()
    {
        lockImage.SetActive(true);
        tickImage.SetActive(true);
    }

    public void ReverseClaimedEffect()
    {
        lockImage.SetActive(false);
        tickImage.SetActive(false);
    }
}
