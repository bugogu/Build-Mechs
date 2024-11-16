using DG.Tweening;
using UnityEngine;

public class diamondReward : MonoBehaviour
{
    [SerializeField] private GameObject pileOfDiamond;
    [SerializeField] private Transform diamondImage;
    [SerializeField] private Vector2[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int diamondAmount;
    void Start()
    {

        if (diamondAmount == 0)
            diamondAmount = 10; // you need to change this value based on the number of coins in the inspector

        initialPos = new Vector2[diamondAmount];
        initialRotation = new Quaternion[diamondAmount];

        for (int i = 0; i < pileOfDiamond.transform.childCount; i++)
        {
            initialPos[i] = pileOfDiamond.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            initialRotation[i] = pileOfDiamond.transform.GetChild(i).GetComponent<RectTransform>().rotation;
        }
    }

    private void Reset()
    {
        for (int i = 0; i < pileOfDiamond.transform.childCount; i++)
        {
            pileOfDiamond.transform.GetChild(i).GetComponent<RectTransform>().localPosition = initialPos[i];
            pileOfDiamond.transform.GetChild(i).GetComponent<RectTransform>().localRotation = initialRotation[i];
        }
    }

    public void CountDiamonds()
    {
        Reset();

        pileOfDiamond.SetActive(true);
        var delay = 0f;

        for (int i = 0; i < pileOfDiamond.transform.childCount; i++)
        {
            pileOfDiamond.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            pileOfDiamond.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(-250, 900), 0.8f)
                .SetDelay(delay + 0.5f).SetEase(Ease.InBack);


            pileOfDiamond.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f)
                .SetEase(Ease.Flash);


            pileOfDiamond.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            delay += 0.1f;

            diamondImage.DOScale(1.1f, 0.1f).SetLoops(10, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(1.2f);
        }
    }

    private void OnEnable() => EventManager.AddListener(GameEvent.OnDiamondCollected, CountDiamonds);

    private void OnDisable() => EventManager.RemoveListener(GameEvent.OnDiamondCollected, CountDiamonds);
}
