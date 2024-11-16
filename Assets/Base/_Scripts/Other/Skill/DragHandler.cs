using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public enum SkillType { Meteor, Shield }
    public SkillType skillType;
    [SerializeField] private RectTransform specialSkillFrame;
    [SerializeField] private AudioClip clickSFX;
    private RectTransform rectTransform;
    private Vector2 initialPosition;

    private void Start()
    {
        rectTransform = transform as RectTransform;
        initialPosition = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData) => rectTransform.anchoredPosition += eventData.delta;

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 imageCenter = rectTransform.position;

        Vector2 otherElementCenter = specialSkillFrame.position;

        float distance = Vector2.Distance(imageCenter, otherElementCenter);

        if (distance > 50)
        {
            rectTransform.SmoothPosition(initialPosition, .5f);
        }

        else
        {
            if (SkillController.Instance.placeable)
            {
                rectTransform.position = initialPosition;

                if (skillType == SkillType.Meteor)
                {
                    SkillController.Instance.MeteorCount -= 1;
                    PlayerManager.Instance.SkillMeteor();
                    SkillController.Instance.ActivateTimerPanel();

                }

                if (skillType == SkillType.Shield)
                {
                    SkillController.Instance.ShieldCount -= 1;
                    PlayerManager.Instance.SkillSiheld();
                    SkillController.Instance.ActivateTimerPanel();
                }

            }
            else
                rectTransform.SmoothPosition(initialPosition, .5f);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MyFunc.PlaySound(clickSFX, gameObject);
    }

}
