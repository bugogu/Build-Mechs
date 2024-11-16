using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoSing<UIManager>
{

    #region Variables

    [HideInInspector] public bool rewardCollectable = false;
    [HideInInspector] public int selledItemCount = 0;
    public static int _partSelledCount = 0;
    public static int timeScale;

    [Space(order = 1), Header("General")]
    public Button playButton;
    public AudioClip coinSFX;
    public RectTransform trash;
    [SerializeField] private UnityEngine.Events.UnityEvent cameraPosSetter;
    [SerializeField] GameObject[] menuCanvases;
    [SerializeField] private GameObject gameplayCanvas;
    [SerializeField] private RectTransform[] gameplayElements;
    [SerializeField] private float durationStartAnimation;
    [SerializeField] private AudioClip upgradeSFX;
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioClip winSFX;
    [SerializeField] private AudioClip loseSFX;
    [SerializeField] private LevelLoader loader;


    [Space(20), Header("Upgradeable Animation")]
    [SerializeField] private float upgradeableAnimationDuration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;
    [SerializeField] private float randomness;


    [Space, Header("Texts")]
    public GameObject collectRewardText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text diamondText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text damagePriceText, healthPriceText, incomePriceText;
    [SerializeField] private TMP_Text damageLevelText, healthLevelText, incomeLevelText;
    [SerializeField] private TMP_Text damageValueText, healthValueText, incomeValueText;
    [SerializeField] private TMP_Text levelEndCoinText, levelEndDiamondText;
    [SerializeField] private TMP_Text playerLevelText, playerEXPText;
    [SerializeField] private TMP_Text levelBonusText;
    [SerializeField] private TMP_Text coinRewardText, diamondRewardText;
    [SerializeField] private TMP_Text timeSpeed1x, timeSpeed2x;

    [Space, Header("Buttons")]
    [SerializeField] private Button damageUpgradeButton;
    [SerializeField] private Button healthUpgradeButton;
    [SerializeField] private Button incomeUpgradeButton;
    [SerializeField] private Button levelEndCollectButton;
    [SerializeField] private Button rewardButton, closeRewardPanelButton;

    [Space, Header("GameObjects")]
    public GameObject bossTextShadow;
    public GameObject itemChoosePanel;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject levelEndCanvas;
    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private GameObject waveCanvas;
    [SerializeField] private GameObject skillCanvas;
    [SerializeField] private GameObject damageLockShadow;
    [SerializeField] private GameObject healthLockShadow;
    [SerializeField] private GameObject incomeLockShadow;
    [SerializeField] private GameObject groundObject, groundImage;
    [SerializeField] private GameObject upgradeDamageVFX, upgradeHealthVFX, upgradeIncomeVFX;
    [SerializeField] private GameObject[] environmentObjects;
    [SerializeField] private GameObject levelEndWinText, levelEndLoseText;
    [SerializeField] private GameObject levelEndClosingPanel;
    [SerializeField] private GameObject interactableManager;
    // [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject menuButtonsParent;
    [SerializeField] private GameObject menuCanvasesParent;
    [SerializeField] private GameObject internetPanel;
    [SerializeField] private GameObject rewardAdsNotReadyText;
    [SerializeField] private GameObject tutorialObject;
    [SerializeField] private GameObject firstLoadBackground;
    [SerializeField] private GameObject animatedMech;
    [SerializeField] private GameObject dailyPanel;
    [SerializeField] private GameObject activeImage;
    [SerializeField] private GameObject backwardGround;

    [Space, Header("Transforms")]
    [SerializeField] private RectTransform coinTextTransform;
    [SerializeField] private RectTransform diamondTextTransform;
    [SerializeField] private RectTransform levelEndPanelTransform;
    [SerializeField] private RectTransform playButtonParentTrasnform;
    [SerializeField] private RectTransform handRect;
    [SerializeField] private RectTransform extraDamagePanel;

    [Space, Header("Images")]
    [SerializeField] private RectTransform damageButtonImage;
    [SerializeField] private RectTransform healthButtonImage;
    [SerializeField] private RectTransform incomeButtonImage;
    [SerializeField] private RectTransform levelEndCoinImage, levelEndDiamondImage;
    [SerializeField] private RectTransform selectedTimeSpeedImage;

    [Space, Header("Pause Menu")]
    public TMP_Text progressPercentText;
    [SerializeField] private RectTransform pauseMenuPanel;
    [SerializeField] private GameObject pauseMenuBackground;

    [Space, Header("Detail Panel")]
    [SerializeField] private Button detailButton;
    [SerializeField] private Button detailPanelCloseButton;
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private RectTransform[] detailBanners;
    [SerializeField] private TMP_Text rocketDamageText, meteorDamageText, mechDamageText;
    // [SerializeField] private TMP_Text rocketDamagePercent, meteorDamagePercent, mechDamagePercent;
    [SerializeField] private TMP_Text totalDamageText;
    // [SerializeField] private Image rocketDamageBar, meteorDamageBar, mechDamageBar;

    [Space, Header("Other")]
    [SerializeField] private CanvasGroup transportingText;
    [SerializeField] private Transform damageVFXLocation, healthVFXLocation, incomeVFXLocation;
    [SerializeField] private ParticleSystem purchaseCoinFX, purchaseDiamondFX;
    [SerializeField] private PlayerManager playerScript;
    [SerializeField] private HorizontalLayoutGroup gridLayoutGroup;
    [SerializeField] private DailyReward dailyReward;


    private bool playerWin;
    private bool _endPanelOpened = false;
    private bool _isUpgradeable = true;
    private int _activeButtonIndex = 4;

    #endregion

    private int DamageTextValue
    {
        get => PlayerPrefs.GetInt("DamageText", 100);
        set => PlayerPrefs.SetInt("DamageText", value);
    }

    private int HealthTextValue
    {
        get => PlayerPrefs.GetInt("HealthText", 100);
        set => PlayerPrefs.SetInt("HealthText", value);
    }

    private int IncomeTextValue
    {
        get => PlayerPrefs.GetInt("IncomeText", 100);
        set => PlayerPrefs.SetInt("IncomeText", value);
    }

    private void Awake()
    {
        SetOnclicks();

        timeScale = 1;
    }

    private void Start()
    {
        Initial();
        Invoke(nameof(OpenRewardPanel), 2.2f);

        DOVirtual.Float(1, .2f, .2f, x => transportingText.alpha = x).SetLoops(10, LoopType.Yoyo).OnComplete(() => transportingText.gameObject.SetActive(false));

        coinText.IncrementalText(0, GameManager.Coin, 2).OnComplete(() =>
        coinTextTransform.DOScale(coinTextTransform.localScale * 1.25f, .15f).SetLoops(2, LoopType.Yoyo));

        diamondText.IncrementalText(0, GameManager.Diamond, 2).OnComplete(() =>
        diamondTextTransform.DOScale(diamondTextTransform.localScale * 1.25f, .15f).SetLoops(2, LoopType.Yoyo));

        playButtonParentTrasnform.DOScale(playButtonParentTrasnform.localScale * 1.10f, .5f).SetLoops(100, LoopType.Yoyo);
    }

    public void OpenRewardPanel()
    {
        if ((GameManager.Prestige * 15) + (GameManager.Level) < 1) return;

        // if (PlayerPrefs.GetInt("AdsActive", 1) == 0) return;

        rewardPanel.SetActive(true);
        rewardPanel.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.2f).From().SetEase(Ease.InOutBack);

        coinRewardText.text = (((GameManager.Prestige * 15 + (GameManager.Level) + 1)) * 100).ToString();
        diamondRewardText.text = ((GameManager.Prestige * 15) + GameManager.Level + 10).ToString();
    }

    public void CloseRewardPanel()
    {
        MyFunc.PlaySound(clickSFX, gameObject);
        rewardPanel.SetActive(false);
    }

    public void RewardAdsNotReady()
    {
        rewardAdsNotReadyText.SetActive(true);
        rewardAdsNotReadyText.GetComponent<RectTransform>().DOShakePosition(.5f, 10, 30, 150).OnComplete(() => rewardAdsNotReadyText.SetActive(false));
    }

    public void OpenInternetPanel() => internetPanel.SetActive(true);

    public void CloseInternetPanel() =>
        internetPanel.SetActive(false);

    private void SetOnclicks()
    {
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(StartButton);

        damageUpgradeButton.onClick.RemoveAllListeners();
        damageUpgradeButton.onClick.AddListener(UpgradeDamage);

        healthUpgradeButton.onClick.RemoveAllListeners();
        healthUpgradeButton.onClick.AddListener(UpgradeHealth);

        incomeUpgradeButton.onClick.RemoveAllListeners();
        incomeUpgradeButton.onClick.AddListener(UpgradeIncome);

        levelEndCollectButton.onClick.RemoveAllListeners();
        levelEndCollectButton.onClick.AddListener(LevelEndCollectButton);

        detailButton.onClick.RemoveAllListeners();
        detailButton.onClick.AddListener(DetailPanelOpen);

        detailPanelCloseButton.onClick.RemoveAllListeners();
        detailPanelCloseButton.onClick.AddListener(() => detailPanel.SetActive(false));
    }

    private void StartButton()
    {
        playerScript.DamagePerSecond = GameManager.DamageMultiplier;

        EventManager.Brodcast(GameEvent.OnLevelStarted);
        if (GameManager.bossLevel)
            bossTextShadow.SetActive(true);

        GameManager.Instance.StartAction();
        MyFunc.PlaySound(clickSFX, gameObject);

        DeActiveCanvases();

        gameplayCanvas.SetActive(true);
        interactableManager.SetActive(true);
        //settingsPanel.SetActive(false);
        backwardGround.SetActive(true);

        StartAnimation();

        groundObject.SetActive(false);
        groundImage.SetActive(true);
        enemySpawner.SetActive(true);
        skillCanvas.SetActive(true);

        Camera.main.transform.DOMove(new Vector3(0.15f, 18.1599998f, -14.88f), 1).OnComplete(() => waveCanvas.SetActive(true))
        .OnComplete(() => cameraPosSetter?.Invoke());
        Camera.main.transform.DORotate(new Vector3(35, 0, 0), 1);
    }

    private void DeActiveCanvases()
    {
        for (int i = 0; i < menuCanvases.Length; i++)
            menuCanvases[i].SetActive(false);
    }

    private void StartAnimation()
    {
        var seq = DOTween.Sequence();

        foreach (RectTransform element in gameplayElements)
            seq.Append(element.DOScale(Vector3.zero, durationStartAnimation).From().SetEase(Ease.InOutBack));
    }

    public void UpdateCoin(int comingCoin)
    {
        if (comingCoin == 1000)
            GameManager.Instance.UnlockAchievement(GameManager.Instance.collectorAchievementID);

        if (comingCoin == 3000)
            GameManager.Instance.UnlockAchievement(GameManager.Instance.millionaireAchievementID);

        EventManager.Brodcast(GameEvent.OnCoinCollected);
        PopUpText.GeneratePopup(comingCoin, "Coin");
        GameManager.Coin += comingCoin;
        coinText.IncrementalText(GameManager.Coin - comingCoin, GameManager.Coin, 2.2f).OnComplete(() =>
        coinTextTransform.DOScale(coinTextTransform.localScale * 1.25f, .15f).SetLoops(2, LoopType.Yoyo));

        Invoke("DisableCoinPopText", 1.001f);

        if (GameManager.Coin >= GameManager.DamagePrice && mainCanvas.activeSelf)
            damageLockShadow.SetActive(false);

        if (GameManager.Coin >= GameManager.HealthPrice && mainCanvas.activeSelf)
            healthLockShadow.SetActive(false);

        if (GameManager.Coin >= GameManager.IncomePrice && mainCanvas.activeSelf)
            incomeLockShadow.SetActive(false);
    }

    public void UpdateDiamond(int comingDiamond)
    {
        if (comingDiamond == 70)
            GameManager.Instance.UnlockAchievement(GameManager.Instance.jewelerAchievementID);

        if (comingDiamond == 300)
            GameManager.Instance.UnlockAchievement(GameManager.Instance.investorAchievementID);

        EventManager.Brodcast(GameEvent.OnDiamondCollected);
        PopUpText.GeneratePopup(comingDiamond, "Diamond");
        GameManager.Diamond += comingDiamond;
        diamondText.IncrementalText(GameManager.Diamond - comingDiamond, GameManager.Diamond, 2.2f).OnComplete(() =>
        diamondTextTransform.DOScale(diamondTextTransform.localScale * 1.25f, .15f).SetLoops(2, LoopType.Yoyo));
    }

    private void DisableCoinPopText() => GameManager.Instance.popupTextCoin.SetActive(false);

    private void UpgradeDamage()
    {
        if (!_isUpgradeable) return;
        _isUpgradeable = false;
        Invoke("EnableUpgrade", .5f);

        MyFunc.PlaySound(upgradeSFX, gameObject);
        var generatedVFX = Instantiate(upgradeDamageVFX, damageVFXLocation);
        Destroy(generatedVFX, 2f);
        purchaseCoinFX.Play();

        GameManager.Coin -= GameManager.DamagePrice;
        GameManager.DamagePrice += 75;
        GameManager.DamageLevel += 1;
        GameManager.DamageMultiplier += .05f;
        DamageTextValue += 5;
        playerScript.DamagePerSecond = GameManager.DamageMultiplier;

        coinText.text = GameManager.Coin.ToString();
        damagePriceText.text = GameManager.DamagePrice.ToString();
        damageLevelText.text = GameManager.DamageLevel.ToString();
        damageValueText.text = "%" + DamageTextValue;

        if (GameManager.Coin < GameManager.DamagePrice)
            damageLockShadow.SetActive(true);

        if (GameManager.Coin < GameManager.HealthPrice)
            healthLockShadow.SetActive(true);

        if (GameManager.Coin < GameManager.IncomePrice)
            incomeLockShadow.SetActive(true);
    }

    private void UpgradeHealth()
    {
        if (!_isUpgradeable) return;
        _isUpgradeable = false;
        Invoke("EnableUpgrade", .5f);

        MyFunc.PlaySound(upgradeSFX, gameObject);
        var generatedVFX = Instantiate(upgradeHealthVFX, healthVFXLocation);
        Destroy(generatedVFX, 2f);
        purchaseCoinFX.Play();

        GameManager.Coin -= GameManager.HealthPrice;
        GameManager.HealthPrice += 50;
        GameManager.HealthLevel += 1;
        GameManager.Health += 50;
        HealthTextValue += 50;

        coinText.text = GameManager.Coin.ToString();
        healthPriceText.text = GameManager.HealthPrice.ToString();
        healthLevelText.text = GameManager.HealthLevel.ToString();
        healthValueText.text = HealthTextValue.ToString();

        if (GameManager.Coin < GameManager.DamagePrice)
            damageLockShadow.SetActive(true);

        if (GameManager.Coin < GameManager.HealthPrice)
            healthLockShadow.SetActive(true);

        if (GameManager.Coin < GameManager.IncomePrice)
            incomeLockShadow.SetActive(true);
    }

    private void UpgradeIncome()
    {
        if (!_isUpgradeable) return;
        _isUpgradeable = false;
        Invoke("EnableUpgrade", .5f);

        MyFunc.PlaySound(upgradeSFX, gameObject);
        var generatedVFX = Instantiate(upgradeIncomeVFX, incomeVFXLocation);
        Destroy(generatedVFX, 2f);
        purchaseCoinFX.Play();

        GameManager.Coin -= GameManager.IncomePrice;
        GameManager.IncomePrice += 150;
        GameManager.IncomeLevel += 1;
        GameManager.IncomeMultiplier += .1f;
        IncomeTextValue += 10;

        coinText.text = GameManager.Coin.ToString();
        incomePriceText.text = GameManager.IncomePrice.ToString();
        incomeLevelText.text = GameManager.IncomeLevel.ToString();
        incomeValueText.text = "%" + IncomeTextValue;

        if (GameManager.Coin < GameManager.DamagePrice)
            damageLockShadow.SetActive(true);

        if (GameManager.Coin < GameManager.HealthPrice)
            healthLockShadow.SetActive(true);

        if (GameManager.Coin < GameManager.IncomePrice)
            incomeLockShadow.SetActive(true);
    }

    public void CoinRewardButton()
    {
        if (!rewardCollectable) return;

        GameManager.CollectedPrize++;

        if (GameManager.CollectedPrize >= 10)
            GameManager.Instance.UnlockAchievement(GameManager.Instance.prizeOrientedAchievementID);

        MyFunc.PlaySound(coinSFX, gameObject);
        MyFunc.PlaySound(clickSFX, gameObject);

        rewardCollectable = false;
        collectRewardText.SetActive(false);

        activeImage.SetActive(false);

        UpdateCoin((GameManager.Prestige * 15 + (GameManager.Level) + 1) * 100);

        RewardSystem.Instance.currentTime = 122f;
        RewardSystem.Instance.isCounting = true;
        RewardSystem.Instance.countdownText.gameObject.SetActive(true);
    }

    private void Initial()
    {
        MainBaseButton();

        if ((GameManager.Prestige * 15) + (GameManager.Level) < 1)
        {
            handRect.gameObject.SetActive(true);
            firstLoadBackground.SetActive(true);
            handRect.DOAnchorPos(new Vector2(92, 24), .5f).SetLoops(100, LoopType.Yoyo);
        }

        coinText.text = GameManager.Coin.ToString();
        diamondText.text = GameManager.Diamond.ToString();
        levelText.text = "Stage " + (GameManager.Prestige * 15 + (GameManager.Level + 1));

        damagePriceText.text = GameManager.DamagePrice.ToString();
        healthPriceText.text = GameManager.HealthPrice.ToString();
        incomePriceText.text = GameManager.IncomePrice.ToString();

        damageLevelText.text = GameManager.DamageLevel.ToString();
        healthLevelText.text = GameManager.HealthLevel.ToString();
        incomeLevelText.text = GameManager.IncomeLevel.ToString();

        damageValueText.text = "%" + DamageTextValue;
        healthValueText.text = HealthTextValue.ToString();
        incomeValueText.text = "%" + IncomeTextValue;

        if (GameManager.Coin < GameManager.DamagePrice)
            damageLockShadow.SetActive(true);

        if (GameManager.Coin < GameManager.HealthPrice)
            healthLockShadow.SetActive(true);

        if (GameManager.Coin < GameManager.IncomePrice)
            incomeLockShadow.SetActive(true);

        for (int i = 0; i < environmentObjects.Length; i++)
            environmentObjects[i].SetActive(true);

        _partSelledCount = 0;

        if (PlayerPrefs.GetInt("PlayerEXP") >= 300)
        {
            GameManager.PlayerEXP -= 300;
            GameManager.PlayerLevel += 1;
        }

        playerEXPText.text = GameManager.PlayerEXP.ToString() + " / 300";
        playerLevelText.text = GameManager.PlayerLevel.ToString();

        InvokeRepeating("UpgradeableAnimation", 1, 1);
    }

    public void OnClickAnimation(RectTransform uiRect)
    {
        if (!_isUpgradeable) return;

        uiRect.DOScale(uiRect.localScale * 1.25f, .15f).SetLoops(2, LoopType.Yoyo);
    }

    public System.Collections.IEnumerator LevelEndEvent(bool playerWinn)
    {
        GameManager.Instance.levelEndCamera.SetActive(true);

        if (!_endPanelOpened)
        {
            _endPanelOpened = true;

            levelEndClosingPanel.SetActive(false);

            int rewardCoin = GameManager.bossLevel ? Mathf.RoundToInt(GameManager.Level * 100 * GameManager.IncomeMultiplier * GameManager.PlayerLevel)
            : Mathf.RoundToInt(GameManager.tempCoin * GameManager.IncomeMultiplier * GameManager.PlayerLevel);

            int rewardDiamond = GameManager.bossLevel ? GameManager.Level * 30 * GameManager.PlayerLevel :
            Mathf.RoundToInt((GameManager.tempDiamond / 3) + ((GameManager.Prestige * 15) + GameManager.Level * GameManager.PlayerLevel));

            if (playerWinn)
            {
                levelEndWinText.SetActive(true);
                playerWin = true;
                MyFunc.PlaySound(winSFX, gameObject);
            }
            else
            {
                MyFunc.PlaySound(loseSFX, gameObject);
                levelEndLoseText.SetActive(true);
                if (GameManager.bossLevel)
                {
                    rewardCoin = 0; rewardDiamond = 0;
                }
            }

            GameManager.Instance.gameOver = true;
            yield return new WaitForSeconds(2);
            levelEndCanvas.SetActive(true);

            levelEndPanelTransform.DOScale(Vector3.zero, 1f).From().SetEase(Ease.OutBack).OnComplete(

                () => levelEndCoinText.IncrementalText(0, rewardCoin, 1).OnComplete(

                    () => levelEndCoinImage.DOScale(Vector3.one * 1.5f, .5f).From().SetLoops(1, LoopType.Yoyo).OnComplete(

                        () => levelEndDiamondText.IncrementalText(0, rewardDiamond, 1).OnComplete(

                            () => levelEndDiamondImage.DOScale(Vector3.one * 1.5f, .5f).From().SetLoops(1, LoopType.Yoyo).OnComplete(

                                () => levelBonusText.IncrementalText(0, GameManager.PlayerLevel, 1).OnComplete(

                                    () => levelBonusText.transform.DOScale(Vector3.one * 1.5f, .5f).From().SetLoops(1, LoopType.Yoyo).OnComplete(

                                        () => levelEndCollectButton.enabled = true)))))));
        }

    }

    public void EnableLevelEnd(bool winStatus) => StartCoroutine(LevelEndEvent(winStatus));

    private void LevelEndCollectButton()
    {
        levelEndCollectButton.interactable = false;
        MyFunc.PlaySound(clickSFX, gameObject);
        if (playerWin)
            if (GameManager.bossLevel)
            {
                GameManager.Coin += Mathf.RoundToInt(GameManager.Level * 100 * GameManager.IncomeMultiplier);
                GameManager.Diamond += Mathf.RoundToInt(GameManager.Level * 30);
            }
            else
            {
                GameManager.Coin += Mathf.RoundToInt(GameManager.tempCoin * GameManager.IncomeMultiplier);
                GameManager.Diamond += Mathf.RoundToInt(GameManager.tempDiamond / 3);
            }
        else
        {
            if (!GameManager.bossLevel)
            {
                GameManager.Coin += Mathf.RoundToInt(GameManager.tempCoin * GameManager.IncomeMultiplier);
                GameManager.Diamond += Mathf.RoundToInt(GameManager.tempDiamond / 3);
            }
        }

        if (GameManager.bossLevel)
            if (playerWin)
            {
                if (GameManager.Level >= 14)
                {
                    GameManager.Level = 0;
                    GameManager.Prestige += 1;
                    GameManager.LevelProgressCount = 0;
                }
                else
                {
                    GameManager.Level += 1;
                    GameManager.LevelProgressCount = GameManager.LevelProgressCount == 4 ? 0 : GameManager.LevelProgressCount + 1;
                }
            }
            else
            {
                GameManager.Level -= 4;
                GameManager.LevelProgressCount = 0;
            }
        else
        {
            if (playerWin)
            {
                if (GameManager.Level >= 14)
                {
                    GameManager.Level = 0;
                    GameManager.Prestige += 1;
                    GameManager.LevelProgressCount = 0;
                }
                else
                {
                    GameManager.Level += 1;
                    GameManager.LevelProgressCount = GameManager.LevelProgressCount == 4 ? 0 : GameManager.LevelProgressCount + 1;
                }
            }
        }

        loader.LoadNextLevel();

        if (PlayerPrefs.GetInt("AdsActive", 1) == 1)
        {
            ADSManager.Instance.ShowInterstitialAd();
        }

        // PlayerPrefs.SetFloat("RewardCW", RewardSystem.Instance.currentTime); Arızalı
    }

    private void EnableUpgrade() => _isUpgradeable = true;

    private void UpgradeableAnimation()
    {
        if (GameManager.Coin >= GameManager.DamagePrice)
            damageButtonImage.DOShakeAnchorPos(upgradeableAnimationDuration, strength, vibrato, randomness);

        if (GameManager.Coin >= GameManager.HealthPrice)
            healthButtonImage.DOShakeAnchorPos(upgradeableAnimationDuration, strength, vibrato, randomness);

        if (GameManager.Coin >= GameManager.IncomePrice)
            incomeButtonImage.DOShakeAnchorPos(upgradeableAnimationDuration, strength, vibrato, randomness);
    }

    #region Pause Menu

    public void OpenPauseMenu()
    {
        MyFunc.PlaySound(clickSFX, gameObject);
        pauseMenuBackground.gameObject.SetActive(true);
        pauseMenuPanel.DOScale(Vector3.one, 1).SetEase(Ease.OutBack).OnComplete(() => Time.timeScale = 0);
    }

    public void ClosePauseMenu()
    {
        MyFunc.PlaySound(clickSFX, gameObject);
        Time.timeScale = 1;
        pauseMenuPanel.DOScale(Vector3.zero, 1).SetEase(Ease.OutBack).OnComplete(() => pauseMenuBackground.gameObject.SetActive(false));
    }

    public void ContinueButton()
    {
        MyFunc.PlaySound(clickSFX, gameObject);
        Time.timeScale = 1;
        pauseMenuPanel.DOScale(Vector3.zero, 1).SetEase(Ease.OutBack).OnComplete(() => pauseMenuBackground.gameObject.SetActive(false));
    }

    public void BaseButton()
    {
        MyFunc.PlaySound(clickSFX, gameObject);
        loader.LoadNextLevel();
        Time.timeScale = 1;
        GameManager.PlayerEXP += 1;

        // PlayerPrefs.SetFloat("RewardCW", RewardSystem.Instance.currentTime); Arızalı
    }

    #endregion

    private void DetailPanelOpen()
    {
        detailPanel.SetActive(true);
        MyFunc.PlaySound(clickSFX, gameObject);

        var seq = DOTween.Sequence();

        foreach (RectTransform element in detailBanners)
            seq.Append(element.DOScale(Vector3.zero, .6f).From().SetEase(Ease.InOutBack));

        rocketDamageText.IncrementalText(0, GameManager.rocketDamage, 2f);
        meteorDamageText.IncrementalText(0, GameManager.meteorDamage, 2f);
        mechDamageText.IncrementalText(0, GameManager.mechDamage, 2f);
        totalDamageText.IncrementalText(0, EnemySpawner.Instance.DamageTake, 3);

    }

    public void ChooseItemPanelContinueButton()
    {
        itemChoosePanel.SetActive(false);
        Time.timeScale = 1;
        MyFunc.PlaySound(clickSFX, gameObject);

        OpenExtraDamagePanel();
    }

    public void OpenExtraDamagePanel()
    {
        extraDamagePanel.gameObject.SetActive(true);
        extraDamagePanel.DOScale(Vector3.zero, 0.2f).From().SetEase(Ease.InOutBack).OnComplete(() => Time.timeScale = 0.00001f);
    }

    public void CloseExtraDamagePanel()
    {
        Time.timeScale = 1f;
        extraDamagePanel.gameObject.SetActive(false);
    }

    public void InfoButton()
    {
        handRect.gameObject.SetActive(false);
        firstLoadBackground.SetActive(false);
        MyFunc.PlaySound(clickSFX, gameObject);
        tutorialObject.SetActive(true);
    }

    public void CloseInfoButton()
    {
        MyFunc.PlaySound(clickSFX, gameObject);
        tutorialObject.SetActive(false);
    }

    public void OpenDailyPanel()
    {
        MyFunc.PlaySound(clickSFX, gameObject);
        dailyReward.UpdateRemainingText();
        dailyReward.CheckClaimedPrizes();
        dailyPanel.SetActive(true);
    }

    public void CloseDailyPanel()
    {
        MyFunc.PlaySound(clickSFX, gameObject);
        dailyPanel.SetActive(false);

        if (PlayerPrefs.GetInt("ActiveDay") >= 7)
        {
            dailyReward.RestartDaily();

            for (int i = 0; i < dailyReward.dailyElementsParent.transform.childCount; i++)
                dailyReward.dailyElementsParent.GetChild(i).GetComponent<DailyPrize>().ReverseClaimedEffect();
        }

    }

    #region Menu Button
    public void MainBaseButton()
    {
        if (animatedMech.activeSelf != false) MyFunc.PlaySound(clickSFX, gameObject);

        if (_activeButtonIndex == 6) return;

        menuButtonsParent.transform.GetChild(_activeButtonIndex - 2).gameObject.SetActive(false);
        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).GetComponent<RectTransform>().DOAnchorPosY
        (menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y - 50, .5f);

        menuCanvasesParent.transform.GetChild(_activeButtonIndex - 4).gameObject.SetActive(false);

        _activeButtonIndex = 6;

        menuButtonsParent.transform.GetChild(_activeButtonIndex - 3).gameObject.SetActive(true);
        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).GetComponent<RectTransform>().DOAnchorPosY
        (menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y + 50, .5f);
    }

    public void MainStoreButton()
    {
        if (animatedMech.activeSelf != false) MyFunc.PlaySound(clickSFX, gameObject);

        if (_activeButtonIndex == 4) return;

        menuButtonsParent.transform.GetChild(_activeButtonIndex - 3).gameObject.SetActive(false);
        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).GetComponent<RectTransform>().DOAnchorPosY
        (menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y - 50, .5f);

        _activeButtonIndex = 4;

        menuCanvasesParent.transform.GetChild(_activeButtonIndex - 4).gameObject.SetActive(true);

        menuButtonsParent.transform.GetChild(_activeButtonIndex - 2).gameObject.SetActive(true);
        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).GetComponent<RectTransform>().DOAnchorPosY
        (menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y + 50, .5f);
    }

    public void MainSkillButton()
    {
        if (_activeButtonIndex == 5) return;

        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).gameObject.SetActive(false);
        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().DOAnchorPosY
        (menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y - 50, .5f);

        _activeButtonIndex = 5;

        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).gameObject.SetActive(true);
        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().DOAnchorPosY
        (menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 50, .5f);
    }

    public void MainEventButton()
    {
        if (_activeButtonIndex == 7) return;

        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).gameObject.SetActive(false);
        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().DOAnchorPosY
        (menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y - 50, .5f);

        _activeButtonIndex = 7;

        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).gameObject.SetActive(true);
        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().DOAnchorPosY
        (menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 50, .5f);
    }

    public void MainInventoryButton()
    {
        if (_activeButtonIndex == 8) return;

        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).gameObject.SetActive(false);
        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().DOAnchorPosY
        (menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y - 50, .5f);

        _activeButtonIndex = 8;

        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(0).gameObject.SetActive(true);
        menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().DOAnchorPosY
        (menuButtonsParent.transform.GetChild(_activeButtonIndex).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 50, .5f);
    }
    #endregion

    public void TimeSpeed2x()
    {
        if (timeScale == 1)
            selectedTimeSpeedImage.DOLocalMoveY(selectedTimeSpeedImage.localPosition.y + 70, .3f);

        timeSpeed1x.color = Color.white;
        timeSpeed2x.color = Color.green;
        timeScale = 2;
    }

    public void TimeSpeed1x()
    {
        if (timeScale == 2)
            selectedTimeSpeedImage.DOLocalMoveY(selectedTimeSpeedImage.localPosition.y - 70, .3f);

        timeSpeed2x.color = Color.white;
        timeSpeed1x.color = Color.green;
        timeScale = 1;
    }

    public void Secret()
    {
        PlayerPrefs.SetString("Skin", "Claimed");
        GameManager.Instance.mechBaseMaterial.color = GameManager.Instance.mechClaimedSkinColor;
        GameManager.Instance.mechDissolveBaseMaterial.SetColor("_Albedo", GameManager.Instance.mechClaimedSkinColor);
    }
}
