using UnityEngine;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using System;

[System.Serializable]
public class ConsumableItem
{
    public string name;
    public string id;
    public string desc;
    public float price;
    public int value;
}

[System.Serializable]
public class NonConsumableItem
{
    public string name;
    public string id;
    public string desc;
    public float price;
}

public class ShopElement : MonoBehaviour, IStoreListener
{
    IStoreController _iStoreController;

    public ConsumableItem cItem1;
    public ConsumableItem cItem2;
    public ConsumableItem cItem3;
    public ConsumableItem cItem4;
    public ConsumableItem cItem5;
    public ConsumableItem cItem6;
    public NonConsumableItem nItem;

    [SerializeField] private GameObject removeAdsButton;

    [HideInInspector]
    public string environment = "production";

    [System.Obsolete]
    private void Start()
    {
        if (PlayerPrefs.GetInt("AdsActive", 1) == 0)
            removeAdsButton.SetActive(true);

        SetupBuilder();
    }

    async void Awake()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception)
        {
            Debug.Log(exception);
        }
    }

    [System.Obsolete]
    void SetupBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());


        builder.AddProduct(cItem1.id, ProductType.Consumable);
        builder.AddProduct(cItem2.id, ProductType.Consumable);
        builder.AddProduct(cItem3.id, ProductType.Consumable);
        builder.AddProduct(cItem4.id, ProductType.Consumable);
        builder.AddProduct(cItem5.id, ProductType.Consumable);
        builder.AddProduct(cItem6.id, ProductType.Consumable);
        builder.AddProduct(nItem.id, ProductType.NonConsumable);

        // Warning This Section
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _iStoreController = controller;
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;

        print("Purchase Completed " + product.definition.id);

        if (product.definition.id == cItem1.id)
            UIManager.Instance.UpdateCoin(cItem1.value);
        else if (product.definition.id == cItem2.id)
            UIManager.Instance.UpdateCoin(cItem2.value);
        else if (product.definition.id == cItem3.id)
            UIManager.Instance.UpdateCoin(cItem3.value);
        else if (product.definition.id == cItem4.id)
            UIManager.Instance.UpdateDiamond(cItem4.value);
        else if (product.definition.id == cItem5.id)
            UIManager.Instance.UpdateDiamond(cItem5.value);
        else if (product.definition.id == cItem6.id)
            UIManager.Instance.UpdateDiamond(cItem6.value);
        else if (product.definition.id == nItem.id)
        {
            PlayerPrefs.SetInt("AdsActive", 0);
            removeAdsButton.SetActive(true);
        }

        return PurchaseProcessingResult.Complete;
    }

    public void ConsumableCoin(string itemId)
    {
        _iStoreController.InitiatePurchase(itemId);
    }

    public void ConsumableDiamond(string itemId)
    {
        _iStoreController.InitiatePurchase(itemId);
    }

    public void NonConsumableRemoveAds()
    {
        _iStoreController.InitiatePurchase(nItem.id);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new System.NotImplementedException();
    }
}
