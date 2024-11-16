using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ItemColumn : MonoBehaviour
{
    [SerializeField] private RectTransform circleShadow;
    [SerializeField] private TMPro.TMP_Text itemHeader;
    public TMPro.TMP_Text priceText;
    [SerializeField] private UnityEngine.UI.Button buyButton;
    [SerializeField] private ItemData itemDataList;
    [SerializeField] private RectTransform notEnoughDiamondText;
    [SerializeField] private GameObject otherColumn1, otherColumn2;

    private Item _selectedItemData;
    [HideInInspector] public int itemPrice;

    private IEnumerator Start()
    {
        _selectedItemData = itemDataList.itemsData[Random.Range(0, itemDataList.itemsData.Length)];
        SetData();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItemButton);

        yield return new WaitForSeconds(.3f);
        Time.timeScale = .0001f;
    }

    private void SetData()
    {
        itemPrice = _selectedItemData.priceValue;

        itemHeader.text = _selectedItemData.itemName;
        priceText.text = itemPrice.ToString();
        Instantiate(_selectedItemData.itemImage, circleShadow as Transform);
    }

    private void BuyItemButton()
    {
        if (_selectedItemData.itemType == Item.ItemType.Shield)
            BuyShield();

        if (_selectedItemData.itemType == Item.ItemType.Meteor)
            BuyMeteor();

        if (_selectedItemData.itemType == Item.ItemType.Part)
            BuyPart();
    }

    private void BuyShield()
    {
        if (GameManager.Diamond >= itemPrice)
        {
            if (UIManager.Instance.selledItemCount < 2)
                UIManager.Instance.selledItemCount++;
            else
            {
                UIManager.Instance.itemChoosePanel.SetActive(false);
                Time.timeScale = 1f;
                UIManager.Instance.OpenExtraDamagePanel();
            }


            SkillController.Instance.ShieldCount++;
            GameManager.Diamond -= itemPrice;
            gameObject.SetActive(false);

            otherColumn1.GetComponent<ItemColumn>().itemPrice *= 2;
            otherColumn1.GetComponent<ItemColumn>().priceText.text = otherColumn1.GetComponent<ItemColumn>().itemPrice.ToString();

            otherColumn2.GetComponent<ItemColumn>().itemPrice *= 2;
            otherColumn2.GetComponent<ItemColumn>().priceText.text = otherColumn2.GetComponent<ItemColumn>().itemPrice.ToString();

        }

        else
        {
            notEnoughDiamondText.gameObject.SetActive(true);
            notEnoughDiamondText.DOShakePosition(.000035f, 10, 30, 150).OnComplete(() => notEnoughDiamondText.gameObject.SetActive(false));
        }

    }

    private void BuyMeteor()
    {
        if (GameManager.Diamond >= itemPrice)
        {
            if (UIManager.Instance.selledItemCount < 2)
                UIManager.Instance.selledItemCount++;
            else
            {
                UIManager.Instance.itemChoosePanel.SetActive(false);
                Time.timeScale = 1f;
            }

            SkillController.Instance.MeteorCount++;
            GameManager.Diamond -= itemPrice;
            gameObject.SetActive(false);

            otherColumn1.GetComponent<ItemColumn>().itemPrice *= 2;
            otherColumn1.GetComponent<ItemColumn>().priceText.text = otherColumn1.GetComponent<ItemColumn>().itemPrice.ToString();

            otherColumn2.GetComponent<ItemColumn>().itemPrice *= 2;
            otherColumn2.GetComponent<ItemColumn>().priceText.text = otherColumn2.GetComponent<ItemColumn>().itemPrice.ToString();
        }

        else
        {
            notEnoughDiamondText.gameObject.SetActive(true);
            notEnoughDiamondText.DOShakePosition(.000035f, 10, 30, 150).OnComplete(() => notEnoughDiamondText.gameObject.SetActive(false));
        }

    }

    private void BuyPart()
    {
        if (GameManager.Diamond >= itemPrice)
        {
            if (UIManager.Instance.selledItemCount < 2)
                UIManager.Instance.selledItemCount++;
            else
            {
                UIManager.Instance.itemChoosePanel.SetActive(false);
                Time.timeScale = 1f;
            }

            GameManager.Diamond -= itemPrice;
            DropManager.Instance.GenerateItem(UIManager._partSelledCount);
            UIManager._partSelledCount++;
            gameObject.SetActive(false);

            otherColumn1.GetComponent<ItemColumn>().itemPrice *= 2;
            otherColumn1.GetComponent<ItemColumn>().priceText.text = otherColumn1.GetComponent<ItemColumn>().itemPrice.ToString();

            otherColumn2.GetComponent<ItemColumn>().itemPrice *= 2;
            otherColumn2.GetComponent<ItemColumn>().priceText.text = otherColumn2.GetComponent<ItemColumn>().itemPrice.ToString();
        }

        else
        {
            notEnoughDiamondText.gameObject.SetActive(true);
            notEnoughDiamondText.DOShakePosition(.000035f, 10, 30, 150).OnComplete(() => notEnoughDiamondText.gameObject.SetActive(false));
        }

    }

}
