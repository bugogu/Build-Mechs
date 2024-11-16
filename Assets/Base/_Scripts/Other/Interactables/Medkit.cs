using UnityEngine;
using DG.Tweening;

public class Medkit : MonoBehaviour, IInteractable
{
    [SerializeField] private RectTransform healText;
    [SerializeField] private AudioClip clickSFX;

    private void OnEnable()
    {
        healText.parent = null;
        healText.gameObject.SetActive(false);
    }

    public void Interact()
    {
        GetComponent<BoxCollider>().enabled = false;
        MyFunc.PlaySound(clickSFX, gameObject);

        healText.gameObject.SetActive(true);

        healText.GetComponent<RectTransform>().DOAnchorPosY(8, 1).OnComplete(() => healText.gameObject.SetActive(false));

        PlayerManager.Instance.TakeHeal(GameManager.Health / 4);

        gameObject.SetActive(false);
    }
}
