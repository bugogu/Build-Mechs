using UnityEngine;
using DG.Tweening;

public class Diamond : MonoBehaviour, IInteractable
{
    [SerializeField] private RectTransform diamondText;
    [SerializeField] private AudioClip clickSFX;

    private void OnEnable()
    {
        diamondText.parent = null;
        diamondText.gameObject.SetActive(false);
        diamondText.GetComponent<TMPro.TMP_Text>().text = "+" + Mathf.RoundToInt((15 * GameManager.Prestige) + GameManager.Level + 10);
    }

    public void Interact()
    {
        GetComponent<BoxCollider>().enabled = false;

        MyFunc.PlaySound(clickSFX, gameObject);

        diamondText.gameObject.SetActive(true);

        diamondText.GetComponent<RectTransform>().DOAnchorPosY(8, 1).OnComplete(() => diamondText.gameObject.SetActive(false));

        GameManager.Diamond += Mathf.RoundToInt((15 * GameManager.Prestige) + GameManager.Level + 10);

        gameObject.SetActive(false);
    }
}
