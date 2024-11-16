using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour, IInteractable
{

    [SerializeField] private RectTransform coinText;
    [SerializeField] private AudioClip clickSFX;

    private void OnEnable()
    {
        coinText.parent = null;
        coinText.gameObject.SetActive(false);
        coinText.GetComponent<TMPro.TMP_Text>().text = "+" + Mathf.RoundToInt((15 * GameManager.Prestige) + GameManager.Level + 100);
    }

    public void Interact()
    {
        MyFunc.PlaySound(UIManager.Instance.coinSFX, gameObject);

        GetComponent<BoxCollider>().enabled = false;

        coinText.gameObject.SetActive(true);

        coinText.GetComponent<RectTransform>().DOAnchorPosY(8, 1).OnComplete(() => coinText.gameObject.SetActive(false));

        GameManager.Coin += Mathf.RoundToInt((15 * GameManager.Prestige) + GameManager.Level + 100);

        gameObject.SetActive(false);
    }
}
