using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{

    [HideInInspector] public int weaponLevel;
    public WeaponType weaponType;
    public int level = 0;
    public int damageValue;

    [SerializeField] private AudioClip clickSound;
    [SerializeField] private TMPro.TMP_Text damageInfoText;
    public GameObject infoParent;
    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private bool _closeTarget;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData) => rectTransform.anchoredPosition += eventData.delta;

    public void OnEndDrag(PointerEventData eventData) => DropAction();
    public void OnBeginDrag(PointerEventData eventData)
    {
        MyFunc.PlaySound(clickSound, gameObject);
    }

    private void DropAction()
    {
        RectTransform closestRectTransform;
        float closestDistance = 1000;

        foreach (RectTransform targetRectTransform in DropManager.Instance.frames)
        {
            // Frame ile item arasındaki mesafeyi hesapladı.
            Vector3 targetPosition = targetRectTransform.position;
            float distance = Vector2.Distance(rectTransform.position, targetPosition);

            // Ölçülen mesafe son ölçülenden küçük çıktı.
            if (distance < closestDistance)
            {
                // 6 frame den item nesnesine en yakın olan arasındaki mesafe 50 den 
                // büyük veya eşit ise item başlangıç konumuna döndü değilse yakın olan frame değişkene atandı.
                closestDistance = distance;


                if (closestDistance <= 50)
                {
                    closestRectTransform = targetRectTransform;

                    if (closestRectTransform.CompareTag("PlaceFrame")) // Place frame işlemleri.
                    {
                        if (closestRectTransform.childCount > 0)
                        {
                            if (closestRectTransform.GetChild(0).GetComponent<WeaponItem>().weaponType == weaponType &&
                            closestRectTransform.GetChild(0).GetComponent<WeaponItem>().level == level &&
                            closestRectTransform.GetChild(0).GetComponent<WeaponItem>().level != 2) // closestRectTransform.childCount > 1
                                MergeWeapon(closestRectTransform.GetChild(0).GetComponent<RectTransform>(), false); // Place frame de bulunan aynı özellikteki silahı geliştir.
                            else
                                ChangePlace(closestRectTransform); // Place framelerdeki 2 silahın yerini değiştir.
                        }
                        else
                            PlaceAction(closestRectTransform); // Boş olan place frame e bir silahı yerleştir.
                    }
                    else // Merge frame işlemleri.
                    {
                        if (closestRectTransform.childCount > 0)
                        {
                            if (closestRectTransform.GetChild(0).GetComponent<WeaponItem>() == null)
                                rectTransform.SmoothPosition(initialPosition, .5f);
                            else if (closestRectTransform.GetChild(0).GetComponent<WeaponItem>().weaponType == weaponType &&
                            closestRectTransform.GetChild(0).GetComponent<WeaponItem>().level == level &&
                            closestRectTransform.GetChild(0).GetComponent<WeaponItem>().level != 2)
                                MergeWeapon(closestRectTransform.GetChild(0).GetComponent<RectTransform>(), true); // Merge frame de bulunan aynı özellikteki silahı geliştir.
                            else
                                rectTransform.SmoothPosition(initialPosition, .5f);
                        }
                        else
                            rectTransform.SmoothPosition(initialPosition, .5f);
                    }
                    _closeTarget = true;
                    break;
                }
            }
        }
        if (!_closeTarget)
            rectTransform.SmoothPosition(initialPosition, .5f);
        else
            _closeTarget = false;
    }

    private void PlaceAction(RectTransform placeFrame)
    {
        if (!rectTransform.parent.CompareTag("PlaceFrame"))
            rectTransform.localScale *= .70f;
        rectTransform.parent = placeFrame;
        rectTransform.localPosition = Vector3.zero;
        infoParent.SetActive(false);
        initialPosition = rectTransform.position;
        EventManager.Brodcast(GameEvent.NewWeaponInitiated);
    }

    private void ChangePlace(RectTransform frame)
    {
        if (rectTransform.parent.CompareTag("PlaceFrame"))
        {
            rectTransform.SmoothPosition(initialPosition, .5f);
        }
        else
        {
            Transform _parent = rectTransform.parent;

            rectTransform.localScale *= .70f;
            infoParent.SetActive(false);
            rectTransform.parent = frame;
            rectTransform.localPosition = Vector3.zero;

            frame.GetChild(0).GetComponent<WeaponItem>().infoParent.SetActive(true);
            frame.GetChild(0).localScale = Vector3.one;
            frame.GetChild(0).parent = _parent;
            _parent.GetChild(0).localPosition = Vector3.zero;

            initialPosition = rectTransform.position;
            EventManager.Brodcast(GameEvent.NewWeaponInitiated);
        }
    }

    private void MergeWeapon(RectTransform secondWeapon, bool mergeFrame)
    {
        MergeManager.Instance.wowFX.Play();
        if (!mergeFrame)
        {
            if (weaponType == WeaponType.Blaster)
            {
                GameObject upgradedWeapon = Instantiate(MergeManager.Instance.blasterVariants[secondWeapon.GetComponent<WeaponItem>().level], secondWeapon.parent);
                upgradedWeapon.transform.localPosition = Vector3.zero;
                upgradedWeapon.GetComponent<WeaponItem>().SetLevel(level + 1);
                MergeManager.Instance.mergedWeapons.Add(upgradedWeapon);

                MergeManager.Instance.mergedWeapons.Remove(gameObject);
                MergeManager.Instance.mergedWeapons.Remove(secondWeapon.gameObject);

                upgradedWeapon.transform.localScale *= .70f;
                upgradedWeapon.GetComponent<WeaponItem>().infoParent.SetActive(false);

                Destroy(secondWeapon.gameObject);
                Destroy(gameObject);
            }
            else
            {
                GameObject upgradedWeapon = Instantiate(MergeManager.Instance.rocketVariants[secondWeapon.GetComponent<WeaponItem>().level], secondWeapon.parent);
                upgradedWeapon.transform.localPosition = Vector3.zero;
                upgradedWeapon.GetComponent<WeaponItem>().SetLevel(level + 1);
                MergeManager.Instance.mergedWeapons.Add(upgradedWeapon);

                MergeManager.Instance.mergedWeapons.Remove(gameObject);
                MergeManager.Instance.mergedWeapons.Remove(secondWeapon.gameObject);

                upgradedWeapon.transform.localScale *= .70f;
                upgradedWeapon.GetComponent<WeaponItem>().infoParent.SetActive(false);

                Destroy(secondWeapon.gameObject);
                Destroy(gameObject);
            }
        }

        else
        {
            if (weaponType == WeaponType.Blaster)
            {
                GameObject upgradedWeapon = Instantiate(MergeManager.Instance.blasterVariants[secondWeapon.GetComponent<WeaponItem>().level], secondWeapon.parent);
                upgradedWeapon.transform.localPosition = Vector3.zero;
                upgradedWeapon.GetComponent<WeaponItem>().SetLevel(level + 1);
                MergeManager.Instance.mergedWeapons.Add(upgradedWeapon);

                MergeManager.Instance.mergedWeapons.Remove(gameObject);
                MergeManager.Instance.mergedWeapons.Remove(secondWeapon.gameObject);

                Destroy(secondWeapon.gameObject);
                Destroy(gameObject);
            }
            else
            {
                GameObject upgradedWeapon = Instantiate(MergeManager.Instance.rocketVariants[secondWeapon.GetComponent<WeaponItem>().level], secondWeapon.parent);
                upgradedWeapon.transform.localPosition = Vector3.zero;
                upgradedWeapon.GetComponent<WeaponItem>().SetLevel(level + 1);
                MergeManager.Instance.mergedWeapons.Add(upgradedWeapon);

                MergeManager.Instance.mergedWeapons.Remove(gameObject);
                MergeManager.Instance.mergedWeapons.Remove(secondWeapon.gameObject);

                Destroy(secondWeapon.gameObject);
                Destroy(gameObject);
            }
        }
        EventManager.Brodcast(GameEvent.NewWeaponInitiated);
    }

    public void SetLevel(int value) => level = value;

    public void UpgradeAction(GameObject upgradeItem)
    {
        MergeManager.Instance.wowFX.Play();
        Destroy(upgradeItem);

        if (weaponType == WeaponType.Blaster)
        {
            GameObject upgradedWeapon = Instantiate(MergeManager.Instance.blasterVariants[level], transform.parent);
            upgradedWeapon.transform.localPosition = Vector3.zero;
            upgradedWeapon.GetComponent<WeaponItem>().SetLevel(level + 1);
            MergeManager.Instance.mergedWeapons.Add(upgradedWeapon);

            MergeManager.Instance.mergedWeapons.Remove(gameObject);

            if (upgradedWeapon.transform.parent.CompareTag("PlaceFrame"))
            {
                upgradedWeapon.transform.localScale *= .70f;
                upgradedWeapon.GetComponent<WeaponItem>().infoParent.SetActive(false);
                EventManager.Brodcast(GameEvent.NewWeaponInitiated);
            }

            Destroy(gameObject);
        }
        else
        {
            GameObject upgradedWeapon = Instantiate(MergeManager.Instance.rocketVariants[level], transform.parent);
            upgradedWeapon.transform.localPosition = Vector3.zero;
            upgradedWeapon.GetComponent<WeaponItem>().SetLevel(level + 1);
            MergeManager.Instance.mergedWeapons.Add(upgradedWeapon);

            MergeManager.Instance.mergedWeapons.Remove(gameObject);

            if (upgradedWeapon.transform.parent.CompareTag("PlaceFrame"))
            {
                upgradedWeapon.transform.localScale *= .70f;
                upgradedWeapon.GetComponent<WeaponItem>().infoParent.SetActive(false);
                EventManager.Brodcast(GameEvent.NewWeaponInitiated);
            }

            Destroy(gameObject);
        }
    }

    private void OnEnable() => damageInfoText.text = damageValue.ToString();
}

