using UnityEngine;

public class ScrollerBackground : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.RawImage scrollingImage;
    [SerializeField] private float x, y;

    private void Update() => scrollingImage.uvRect = new Rect(scrollingImage.uvRect.position + new Vector2(x, y) * Time.deltaTime, scrollingImage.uvRect.size);
}
