using UnityEngine;
using DG.Tweening;
using GooglePlayGames;

public enum ActiveEnemy
{
    slime, turtle, wizzard
}

public class GameManager : MonoSing<GameManager>
{

    #region Props

    public static ActiveEnemy activeEnemy;

    public static int Coin
    {
        get => PlayerPrefs.GetInt("Coin", 200);
        set => PlayerPrefs.SetInt("Coin", value);
    }

    public static int Diamond
    {
        get => PlayerPrefs.GetInt("Diamond", 100);
        set => PlayerPrefs.SetInt("Diamond", value);
    }

    public static int DamagePrice
    {
        get => PlayerPrefs.GetInt("DamagePrice", 175);
        set => PlayerPrefs.SetInt("DamagePrice", value);
    }

    public static int HealthPrice
    {
        get => PlayerPrefs.GetInt("HealthPrice", 125);
        set => PlayerPrefs.SetInt("HealthPrice", value);
    }

    public static int IncomePrice
    {
        get => PlayerPrefs.GetInt("IncomePrice", 250);
        set => PlayerPrefs.SetInt("IncomePrice", value);
    }

    public static int DamageLevel
    {
        get => PlayerPrefs.GetInt("DamageLevel", 0);
        set => PlayerPrefs.SetInt("DamageLevel", value);
    }

    public static int HealthLevel
    {
        get => PlayerPrefs.GetInt("HealthLevel", 0);
        set => PlayerPrefs.SetInt("HealthLevel", value);
    }

    public static int IncomeLevel
    {
        get => PlayerPrefs.GetInt("IncomeLevel", 0);
        set => PlayerPrefs.SetInt("IncomeLevel", value);
    }

    public static float DamageMultiplier
    {
        get => PlayerPrefs.GetFloat("DamageMultiplier", 1);
        set => PlayerPrefs.SetFloat("DamageMultiplier", value);
    }

    public static float IncomeMultiplier
    {
        get => PlayerPrefs.GetFloat("IncomeMultiplier", 1);
        set => PlayerPrefs.SetFloat("IncomeMultiplier", value);
    }

    public static int Health
    {
        get => PlayerPrefs.GetInt("Health", 100);
        set => PlayerPrefs.SetInt("Health", value);
    }

    public static int Level
    {
        get => PlayerPrefs.GetInt("Level", 0);
        set => PlayerPrefs.SetInt("Level", value);
    }

    public static int Prestige
    {
        get => PlayerPrefs.GetInt("Prestige", 0);
        set => PlayerPrefs.SetInt("Prestige", value);
    }

    public static int LevelProgressCount
    {
        get => PlayerPrefs.GetInt("LevelProgress", 0);
        set => PlayerPrefs.SetInt("LevelProgress", value);
    }

    public static int PlayerLevel
    {
        get => PlayerPrefs.GetInt("PlayerLevel", 1);
        set => PlayerPrefs.SetInt("PlayerLevel", value);
    }

    public static int PlayerEXP
    {
        get => PlayerPrefs.GetInt("PlayerEXP", 0);
        set => PlayerPrefs.SetInt("PlayerEXP", value);
    }

    public static int CollectedPrize
    {
        get => PlayerPrefs.GetInt("CollectedPrize", 0);
        set => PlayerPrefs.SetInt("CollectedPrize", value);
    }

    public static int TotalDefeatedEnemy
    {
        get => PlayerPrefs.GetInt("TotalDefeatedEnemy", 0);
        set => PlayerPrefs.SetInt("TotalDefeatedEnemy", value);
    }

    public static int TotalGivenDamage
    {
        get => PlayerPrefs.GetInt("TotalGivenDamage", 0);
        set => PlayerPrefs.SetInt("TotalGivenDamage", value);
    }

    #endregion

    #region Variables
    public GameObject popupTextCoin, popupTextDiamond;
    public Color popupCoinColor, popupDiamondColor;
    public AudioClip enemyDeathSFX;
    public GameObject levelEndCamera;

    [HideInInspector] public bool gameOver;
    public static int tempCoin = 0, tempDiamond = 0;
    public static bool bossLevel;
    public static float landMineDamage;
    public static float damageBoost;
    public static bool playerFatality;
    public static float tempProgress;
    public static int rocketDamage;
    public static int meteorDamage;
    public static int mechDamage;

    [SerializeField] private Color activeLevelColor, completedLevelColor;
    [SerializeField] private Material[] dissolveMats;
    [SerializeField] private UnityEngine.UI.Image[] levelProgressImages;
    [SerializeField] private GameObject dissolveMech, animatedMech, mech, dissolveMechBackward;
    [SerializeField] private GameObject levelLoaderObject;
    [SerializeField] private AudioClip clickSFX;
    public Material mechBaseMaterial;
    public Color mechClaimedSkinColor;
    public Color mechUnClaimedSkinColor;
    public Material mechDissolveBaseMaterial;

    [Header("Achievements ID")]
    public string newbieAchievementID = "CgkIz7ix1aMCEAIQAQ";
    public string collectorAchievementID = "CgkIz7ix1aMCEAIQAw";
    public string millionaireAchievementID = "CgkIz7ix1aMCEAIQBA";
    public string jewelerAchievementID = "CgkIz7ix1aMCEAIQBQ";
    public string investorAchievementID = "CgkIz7ix1aMCEAIQBg";
    public string prizeOrientedAchievementID = "CgkIz7ix1aMCEAIQBw";
    public string challengerAchievementID = "CgkIz7ix1aMCEAIQCA";
    public string slimeDefeatAchievementID = "CgkIz7ix1aMCEAIQCQ";
    public string wasScrappedAchievementID = "CgkIz7ix1aMCEAIQAg";
    public string noStrangerToFightingAchievementID = "CgkIz7ix1aMCEAIQCg";

    private string FirstLoad
    {
        get => PlayerPrefs.GetString("FirstLoad", "True");
        set => PlayerPrefs.SetString("FirstLoad", value);
    }
    private bool isBegining = false;
    private float tempCountdown;
    private Ray _ray;
    private RaycastHit _hit;
    private Camera _cam;

    #endregion

    private void Awake()
    {
        Application.targetFrameRate = 30;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        levelLoaderObject.SetActive(true);

        InvokeRepeating("CheckInternetConnection", 1.5f, 5f);

        PlayGamesPlatform.Activate();

        if (FirstLoad == "True")
        {
            UnlockAchievement(newbieAchievementID);
            FirstLoad = "False";
        }

        _cam = Camera.main;

        if (PlayerPrefs.GetString("Skin", "Unclaimed") == "Claimed")
        {
            mechBaseMaterial.color = mechClaimedSkinColor;
            mechDissolveBaseMaterial.SetColor("_Albedo", mechClaimedSkinColor);
        }
        else
        {
            mechBaseMaterial.color = mechUnClaimedSkinColor;
            mechDissolveBaseMaterial.SetColor("_Albedo", mechUnClaimedSkinColor);
        }


        if (Level < 4)
            activeEnemy = ActiveEnemy.slime;

        if (Level > 4 && Level <= 9)
            activeEnemy = ActiveEnemy.turtle;

        if (Level > 9 && Level <= 14)
            activeEnemy = ActiveEnemy.wizzard;

        if ((Level + 1) % 5 == 0)
            bossLevel = true;
    }

    private void Start()
    {
        landMineDamage = ((Prestige * 15) + Level + 3) * DamageMultiplier;
        bossLevel = false;
        damageBoost = 1;
        playerFatality = false;
        tempProgress = 0.00f;

        rocketDamage = 0;
        meteorDamage = 0;
        mechDamage = 0;

        foreach (Material dissolveMat in dissolveMats)
            DOVirtual.Float(-5, 10, 2f, x => dissolveMat.SetFloat("_Cutoff_Height", x)).OnComplete(() => MechsVisiableControl(false, true));

        levelProgressImages[LevelProgressCount].color = activeLevelColor;
        for (int i = 0; i < LevelProgressCount; i++)
            levelProgressImages[i].color = completedLevelColor;
    }

    private void Update()
    {
        // if (isBegining)
        //     tempCountdown += Time.time;

        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mousePos = Input.mousePosition;
        _ray = _cam.ScreenPointToRay(mousePos);

        if (!Physics.Raycast(_ray, out _hit, 100)) return;

        if (!_hit.collider.TryGetComponent(out IInteractable interact)) return;

        interact.Interact();
    }

    public void UnlockAchievement(string achievemntID) =>
        Social.ReportProgress(achievemntID, 100, (bool success) => { Debug.Log("Success " + achievemntID); });

    public void CheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UIManager.Instance.OpenInternetPanel();
            Time.timeScale = 0.000001f;
        }
        else
        {
            Time.timeScale = 1f;
            UIManager.Instance.CloseInternetPanel();
        }
    }

    private void MechsVisiableControl(bool dissolveMechStatus = true, bool animatedMechStatus = false, bool mechStatus = false, bool dissolveMech2 = false)
    {
        dissolveMech.SetActive(dissolveMechStatus);
        animatedMech.SetActive(animatedMechStatus);
        mech.SetActive(mechStatus);
        dissolveMechBackward.SetActive(dissolveMech2);

        UIManager.Instance.playButton.enabled = true;
    }

    private void SetRewardCountdown()
    {
        isBegining = false;

        if (RewardSystem.Instance.currentTime <= tempCountdown)
            PlayerPrefs.SetFloat("RewardCW", 0);

        if (RewardSystem.Instance.currentTime > tempCountdown)
            PlayerPrefs.SetFloat("RewardCW", RewardSystem.Instance.currentTime - tempCountdown);
    }

    public void StartAction()
    {
        MechsVisiableControl(false, false, false, true);

        foreach (Material dissolveMat in dissolveMats)
            DOVirtual.Float(-5, 16, 1f, x => dissolveMat.SetFloat("_Cutoff_Height", x)).OnComplete(() => MechsVisiableControl(false, false, true, false));
    }

    private void OnEnable()
    {
        EventManager.AddListener(GameEvent.OnLevelEnded, SetRewardCountdown);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(GameEvent.OnLevelEnded, SetRewardCountdown);
    }

    private void OnApplicationQuit() => PlayerPrefs.SetFloat("RewardCW", RewardSystem.Instance.currentTime);

    private void OnApplicationPause() => PlayerPrefs.SetFloat("RewardCW", RewardSystem.Instance.currentTime);

}
