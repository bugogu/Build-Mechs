using UnityEngine;
using DG.Tweening;

public class DropManager : MonoSing<DropManager>
{
    public GameObject[] mergeableItems;
    public RectTransform[] mergePlaces;
    public RectTransform[] frames;
    public int partCount;

    [SerializeField] private Transform[] dropSendingPositions;
    [SerializeField] private GameObject dropPrefab;

    private int emptyPlaceIndex;

    public void GenerateItem(int frameIndex)
    {
        var generatedItem = Instantiate(mergeableItems[Random.Range(0, mergeableItems.Length)], mergePlaces[frameIndex]);

        generatedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        generatedItem.transform.DOScale(Vector3.zero, .5f).From().SetEase(Ease.InOutCubic);
    }

    public int EmptyFrames()
    {
        emptyPlaceIndex = 0;
        for (int i = 0; i < mergePlaces.Length; i++)
            if (mergePlaces[i].transform.childCount < 2)
                emptyPlaceIndex++;

        return emptyPlaceIndex;
    }

    public void SendDrop()
    {
        var sendedDrop = Instantiate(dropPrefab, dropSendingPositions[Random.Range(0, dropSendingPositions.Length)]);
    }

}
