using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private AudioClip clickSound;
    private RectTransform rectTransform;
    private Vector2 initialPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData) => rectTransform.anchoredPosition += eventData.delta;

    public void OnEndDrag(PointerEventData eventData) => UpgradeAction();

    private void UpgradeAction()
    {
        RectTransform closestRectTransform;
        float closestDistance = 1000;

        if (MergeManager.Instance.mergedWeapons.Count > 0)
            foreach (GameObject targetRectTransform in MergeManager.Instance.mergedWeapons)
            {
                Vector3 targetPosition = targetRectTransform.transform.position;
                float distance = Vector2.Distance(rectTransform.position, targetPosition);

                if (distance < closestDistance)
                {
                    closestDistance = distance;

                    if (closestDistance > 50)
                        rectTransform.SmoothPosition(initialPosition, .5f);
                    else
                    {
                        closestRectTransform = targetRectTransform.GetComponent<RectTransform>();

                        if (closestRectTransform.GetComponent<WeaponItem>().level > 1)
                            rectTransform.SmoothPosition(initialPosition, .5f);
                        else
                            closestRectTransform.GetComponent<WeaponItem>().UpgradeAction(gameObject);
                    }
                }
            }

        else
            rectTransform.SmoothPosition(initialPosition, .5f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MyFunc.PlaySound(clickSound, gameObject);
    }
}
