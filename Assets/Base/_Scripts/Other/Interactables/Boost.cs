using UnityEngine;
using DG.Tweening;

public class Boost : MonoBehaviour, IInteractable
{
    [SerializeField] private RectTransform damageBoostText;
    [SerializeField] private AudioClip clickSFX;

    private void OnEnable()
    {
        damageBoostText.parent = null;
        damageBoostText.gameObject.SetActive(false);
    }

    public void Interact()
    {
        GetComponent<BoxCollider>().enabled = false;

        MyFunc.PlaySound(clickSFX, gameObject);

        damageBoostText.gameObject.SetActive(true);

        damageBoostText.GetComponent<RectTransform>().DOAnchorPosY(8, 1).OnComplete(() => damageBoostText.gameObject.SetActive(false));

        GameManager.damageBoost += .10f;

        gameObject.SetActive(false);
    }
}
