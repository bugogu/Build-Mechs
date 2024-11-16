using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MergeItem { Flame, Powder, Electricity }

[System.Serializable]
public enum WeaponType { Blaster, Rocket }

public class MergeManager : MonoSing<MergeManager>
{
    public GameObject[] blasterVariants;
    public GameObject[] rocketVariants;
    public List<GameObject> mergedWeapons = new List<GameObject>();
    public ParticleSystem wowFX;

    [SerializeField] private GameObject[] mergedItems;

    public GameObject CompoundItem(MergeItem itemOne, MergeItem itemTwo)
    {
        if ((itemOne == MergeItem.Flame && itemTwo == MergeItem.Powder) || (itemOne == MergeItem.Powder && itemTwo == MergeItem.Flame))
            return mergedItems[0];

        else if ((itemOne == MergeItem.Flame && itemTwo == MergeItem.Electricity) || (itemOne == MergeItem.Electricity && itemTwo == MergeItem.Flame))
            return mergedItems[1];

        else if ((itemOne == MergeItem.Powder && itemTwo == MergeItem.Electricity) || (itemOne == MergeItem.Electricity && itemTwo == MergeItem.Powder))
            return mergedItems[2];
        else
            return null;
    }
}


