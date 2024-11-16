using UnityEngine;
using DG.Tweening;

public class Info : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerManager playerScript;
    [SerializeField] private RectTransform rocketPanel;
    [SerializeField] private RectTransform meteorPanel;
    [SerializeField] private TMPro.TMP_Text rocketInfo;
    [SerializeField] private TMPro.TMP_Text meteorInfoText;

    private bool _isInteractable = true;

    public void Interact()
    {
        if (!_isInteractable) return;
        _isInteractable = false;

        rocketInfo.text = ((((GameManager.Prestige * 15) + GameManager.Level) / 4 + 1) * GameManager.DamageMultiplier).ToString("0.00");
        meteorInfoText.text = (playerScript.DamagePerSecond * 10 / 4).ToString("0.00") + "/s";

        rocketPanel.gameObject.SetActive(true);
        rocketPanel.DOScale(Vector3.zero, 1f).From().SetEase(Ease.OutBack);

        meteorPanel.gameObject.SetActive(true);
        meteorPanel.DOScale(Vector3.zero, 1f).From().SetEase(Ease.OutBack);

        Invoke("CloseInfoPanel", 4);
    }

    private void CloseInfoPanel()
    {
        rocketPanel.DOScale(Vector3.zero, 1f).SetEase(Ease.OutBack).OnComplete(() => rocketPanel.gameObject.SetActive(false));
        meteorPanel.DOScale(Vector3.zero, 1f).SetEase(Ease.OutBack).OnComplete(() => meteorPanel.gameObject.SetActive(false))
        .OnComplete(() => _isInteractable = true);

    }

}