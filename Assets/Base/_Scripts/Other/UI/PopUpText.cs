using UnityEngine;
using DG.Tweening;

public class PopUpText : MonoBehaviour
{
    private static GameObject popupTextObject;
    private static Transform canvasTransform;

    private void Start() => canvasTransform = this.transform;

    public static void GeneratePopup(float popupTextValue, string type)
    {
        if (type == "Coin")
            popupTextObject = GameManager.Instance.popupTextCoin;
        if (type == "Diamond")
            popupTextObject = GameManager.Instance.popupTextDiamond;

        popupTextValue = Mathf.Round(popupTextValue);
        popupTextObject.GetComponent<TMPro.TMP_Text>().text = "+" + popupTextValue.ToString();
        popupTextObject.SetActive(true);
        popupTextObject.GetComponent<RectTransform>().DOAnchorPosY(913, 1).OnComplete(() => popupTextObject.SetActive(false));
    }
}