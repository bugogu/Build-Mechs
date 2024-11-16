using UnityEngine;

public class PopUpDeactive : MonoBehaviour
{
    private RectTransform _popupTranform;
    private Vector2 _initialPos;
    private void Awake()
    {
        _popupTranform = GetComponent<RectTransform>();
        _initialPos = _popupTranform.anchoredPosition;
    }
    private void OnDisable()
    {
        _popupTranform.anchoredPosition = _initialPos;
    }
}
