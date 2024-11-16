using UnityEngine;
using UnityEngine.EventSystems;

public class MergeableItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public MergeItem itemType;
    [SerializeField] private AudioClip clickSound;
    private RectTransform rectTransform;
    private Vector2 initialPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData) => rectTransform.anchoredPosition += eventData.delta;

    public void OnEndDrag(PointerEventData eventData)
    {
        MergeAction();
        UIManager.Instance.trash.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MyFunc.PlaySound(clickSound, gameObject);
        UIManager.Instance.trash.gameObject.SetActive(true);
    }

    private void MergeAction()
    {
        RectTransform closestRectTransform;
        float closestDistance = 1000;

        if (Vector2.Distance(transform.position, UIManager.Instance.trash.position) < 50)
            Destroy(gameObject);

        Debug.Log(Vector2.Distance(transform.position, UIManager.Instance.trash.position));

        foreach (RectTransform targetRectTransform in DropManager.Instance.mergePlaces)
        {
            // Frame ile item arasındaki mesafeyi hesapladı.
            Vector3 targetPosition = targetRectTransform.position;
            float distance = Vector2.Distance(rectTransform.position, targetPosition);

            // Ölçülen mesafe son ölçülenden küçük çıktı.
            if (distance < closestDistance)
            {
                // 6 frame den item nesnesine en yakın olan arasındaki mesafe 50 den 
                // büyük veya eşit ise item başlangıç konumuna döndü değilse yakın olan frame değişkene atandı.
                closestDistance = distance;

                if (closestDistance >= 50)
                    rectTransform.SmoothPosition(initialPosition, .5f);
                else
                {
                    closestRectTransform = targetRectTransform;

                    if (closestRectTransform.childCount > 0)
                        if (closestRectTransform.GetChild(0).TryGetComponent(out MergeableItem mergeable))
                            if (mergeable.itemType != itemType)
                                GenerateWeapon(mergeable);
                            else
                                rectTransform.SmoothPosition(initialPosition, .5f);
                        else
                            rectTransform.SmoothPosition(initialPosition, .5f);
                    else
                        rectTransform.SmoothPosition(initialPosition, .5f);
                }
            }
        }
    }

    private void GenerateWeapon(MergeableItem otherItemScript)
    {
        MergeManager.Instance.wowFX.Play();
        MergeItem otherItem = otherItemScript.itemType;
        GameObject generatedItem = Instantiate(MergeManager.Instance.CompoundItem(itemType, otherItem), otherItemScript.transform.parent);
        generatedItem.transform.localPosition = Vector3.zero;

        if (generatedItem.TryGetComponent(out WeaponItem item))
            MergeManager.Instance.mergedWeapons.Add(generatedItem);


        Destroy(otherItemScript.gameObject);
        Destroy(gameObject);

    }
}