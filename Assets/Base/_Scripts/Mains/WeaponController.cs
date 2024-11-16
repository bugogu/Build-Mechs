using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform[] weaponHolders;
    public GameObject[] weaponPlaces;

    [SerializeField] private ParticleSystem smoke;

    private int _childIndex = 3;

    private void WeaponInformationControl()
    {
        Invoke("ControlStarter", .001f);
        // for (int i = 0; i < weaponPlaces.Length; i++)
        // {
        //     if (weaponPlaces[i].transform.childCount > 0)
        //     {
        //         for (int k = 0; k < weaponHolders[i].childCount - 1; k++)
        //             weaponHolders[i].GetChild(k).gameObject.SetActive(false);

        //         weaponHolders[i].GetChild(FindChildIndex(i) + 1).gameObject.SetActive(true);
        //         smoke.Play();
        //     }
        //     else
        //         for (int j = 0; j < weaponHolders[i].childCount - 1; j++)
        //             weaponHolders[i].GetChild(j).gameObject.SetActive(false);
        // }
    }

    private void ControlStarter()
    {
        for (int i = 0; i < weaponPlaces.Length; i++)
        {
            if (weaponPlaces[i].transform.childCount > 0)
            {
                for (int k = 0; k < weaponHolders[i].childCount - 1; k++)
                    weaponHolders[i].GetChild(k).gameObject.SetActive(false);

                weaponHolders[i].GetChild(FindChildIndex(i)).gameObject.SetActive(true);
                smoke.Play();
            }
            else
                for (int j = 0; j < weaponHolders[i].childCount - 1; j++)
                    weaponHolders[i].GetChild(j).gameObject.SetActive(false);
        }
    }

    private int FindChildIndex(int placeIndex)
    {
        WeaponItem _weaponItem = weaponPlaces[placeIndex].transform.GetChild(0).GetComponent<WeaponItem>();

        if (_weaponItem.weaponType == WeaponType.Blaster)
            return _weaponItem.level;
        else
            return _weaponItem.level + _childIndex;
    }

    private void OnEnable() => EventManager.AddListener(GameEvent.NewWeaponInitiated, WeaponInformationControl);
    private void OnDisable() => EventManager.RemoveListener(GameEvent.NewWeaponInitiated, WeaponInformationControl);

}
