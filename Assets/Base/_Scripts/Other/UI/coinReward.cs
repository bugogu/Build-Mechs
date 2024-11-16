using DG.Tweening;
using UnityEngine;

public class coinReward : MonoBehaviour
{
    [SerializeField] private GameObject pileOfCoins;
    [SerializeField] private Transform coinImage;
    [SerializeField] private Vector2[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int coinsAmount;
    void Start()
    {

        if (coinsAmount == 0)
            coinsAmount = 10;

        initialPos = new Vector2[coinsAmount];
        initialRotation = new Quaternion[coinsAmount];

        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            initialPos[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            initialRotation[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().rotation;
        }
    }

    private void Reset()
    {
        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().localPosition = initialPos[i];
            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().localRotation = initialRotation[i];
        }
    }

    public void CountCoins()
    {
        Reset();

        pileOfCoins.SetActive(true);
        var delay = 0f;

        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            pileOfCoins.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(258, 895), 0.8f)
                .SetDelay(delay + 0.5f).SetEase(Ease.InBack);


            pileOfCoins.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f)
                .SetEase(Ease.Flash);


            pileOfCoins.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            delay += 0.1f;

            coinImage.DOScale(1.1f, 0.1f).SetLoops(10, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(1.2f);
        }
    }

    private void OnEnable() => EventManager.AddListener(GameEvent.OnCoinCollected, CountCoins);

    private void OnDisable() => EventManager.RemoveListener(GameEvent.OnCoinCollected, CountCoins);
}
