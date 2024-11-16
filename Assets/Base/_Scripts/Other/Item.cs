using UnityEngine;

[System.Serializable]
public struct Item
{
    public enum ItemType
    {
        Shield, Meteor, Part
    }
    public ItemType itemType;
    public string itemName;
    public int priceValue;
    public GameObject itemImage;
}
