using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoSing<PlayerManager>
{

    public float DamagePerSecond
    {
        get =>
            PlayerPrefs.GetFloat("MeteorDamage", (GameManager.Prestige * 15 + GameManager.Level) * GameManager.DamageMultiplier + 1.5f);
        set =>
            PlayerPrefs.SetFloat("MeteorDamage", (GameManager.Prestige * 15 + GameManager.Level + 1.5f) * value);
    }

    #region Fields

    [Header("General")]
    public ParticleSystem boomTextFX;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private ParticleSystem damageTakenFX;
    [SerializeField] private float scanRate;
    [SerializeField] private float maxDistance = 50;
    [SerializeField] private RectTransform healthBarParent;
    [SerializeField] private Transform attackRadiusSprite;
    [SerializeField] private Transform upperBody;
    [SerializeField] private float rotationSpeed = 200;
    [SerializeField] private Transform selectionObject;
    [SerializeField] private float selectionObjectSpeed;
    [SerializeField] private AudioClip damageTakenSFX;
    [SerializeField] private AudioClip deathSFX;

    [Space]
    [Header("Health")]
    [SerializeField] private UnityEngine.UI.Image fillImage;
    [SerializeField] private UnityEngine.UI.Image effectFillImage;
    [SerializeField] private TMPro.TMP_Text healthText;

    [Space]
    [Header("Skills")]
    [SerializeField] private ParticleSystem shieldVFX;
    [SerializeField] private GameObject shieldBar;
    [SerializeField] private UnityEngine.UI.Image shieldFill;

    [Space]
    [SerializeField] private GameObject meteorCanvas;
    [SerializeField] private GameObject meteorAreaSprite;
    [SerializeField] private UnityEngine.UI.Image meteorTimeFill;
    [SerializeField] private TMPro.TMP_Text meteorTimeText;

    [Space]
    [Header("Other")]
    [SerializeField] private GameObject brokenFX;
    [SerializeField] private GameObject brokenVFX;
    [SerializeField] private GameObject itemChoosePanel;

    [HideInInspector] public int _activeHealth;
    private Collider[] _enemies;
    [HideInInspector] public Enemy _currentTarget;
    private Vector3 _initialMeteorAreaPosition;
    private Vector3 _initialRotation;
    private bool _shakeable = true;
    private Vector3 _initialSelectionPosition;

    private static bool firstDeath;

    #endregion

    private void Awake()
    {
        _initialMeteorAreaPosition = meteorAreaSprite.transform.position;
        _initialRotation = upperBody.eulerAngles;
        _initialSelectionPosition = selectionObject.position;
    }

    private void Start()
    {
        _activeHealth = GameManager.Health;

        healthText.text = _activeHealth + "/" + GameManager.Health;

        itemChoosePanel.SetActive(true);
    }

    public void TakeDamage(float damageValue)
    {
        MyFunc.PlaySound(damageTakenSFX, gameObject);

        if (_shakeable)
        {
            Camera.main.DOShakePosition(.2f, 5, 20, 150);
            _shakeable = false;
            Invoke(nameof(ShakeEnabled), 2f);
            Invoke(nameof(ResetCameraPos), 1);
        }

        MyFunc.DoVibrate();
        damageTakenFX.Play();
        fillImage.fillAmount -= (damageValue / (float)GameManager.Health);
        healthText.text = (_activeHealth - damageValue) + "/" + GameManager.Health;
        _activeHealth -= (int)damageValue;

        DOVirtual.Float(effectFillImage.fillAmount, fillImage.fillAmount, 1, x => effectFillImage.fillAmount = x);

        if (_activeHealth <= 0)
        {
            if (!GameManager.bossLevel)
                if (EnemySpawner.Instance.killedEnemys != 3 * EnemySpawner.Instance.enemysPerWave)
                    UIManager.Instance.EnableLevelEnd(false);

            OnKill();
        }

        if (_activeHealth <= GameManager.Health / 2)
            brokenFX.SetActive(true);

        if (_activeHealth <= GameManager.Health / 4)
            brokenVFX.SetActive(true);

    }

    public void TakeHeal(float healValue)
    {
        if (healValue > (GameManager.Health - _activeHealth))
            healValue = GameManager.Health - _activeHealth;

        fillImage.fillAmount += (healValue / (float)GameManager.Health);
        healthText.text = (_activeHealth + healValue) + "/" + GameManager.Health;
        _activeHealth += (int)healValue;

        if (_activeHealth >= GameManager.Health / 2)
            brokenFX.SetActive(false);

        if (_activeHealth >= GameManager.Health / 4)
            brokenVFX.SetActive(false);
    }

    private void OnKill()
    {
        if (!firstDeath)
        {
            GameManager.Instance.UnlockAchievement(GameManager.Instance.wasScrappedAchievementID);
            firstDeath = true;
        }



        MyFunc.PlaySound(deathSFX, gameObject);
        MyFunc.DoVibrate();
        boomTextFX?.Play();
        deathVFX.SetActive(true);
        gameObject.SetActive(false);
    }

    public void InitialAction()
    {
        if (_currentTarget) return;
        selectionObject.SmoothPosition(_initialSelectionPosition, selectionObjectSpeed * Time.deltaTime);
        upperBody.rotation = Quaternion.Slerp(upperBody.rotation, Quaternion.LookRotation(_initialRotation), Time.deltaTime * (rotationSpeed / 2));
    }

    private void ScanArea()
    {
        float distance = 100;

        _enemies = Physics.OverlapSphere(transform.position, maxDistance);

        foreach (Collider enemy in _enemies)
        {
            if (enemy.gameObject.TryGetComponent(out Enemy target))
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);

                if (dist <= distance)
                {
                    _currentTarget = target;
                    distance = dist;
                }
            }
        }

        if (_currentTarget)
            selectionObject.SmoothPosition(_currentTarget.transform.position, UIManager.timeScale == 1 ? selectionObjectSpeed * Time.deltaTime : ((selectionObjectSpeed / 2) * Time.deltaTime));

        if (!_currentTarget)
        {
            InitialAction();
            selectionObject.SmoothPosition(_initialSelectionPosition, UIManager.timeScale == 1 ? selectionObjectSpeed * Time.deltaTime : ((selectionObjectSpeed / 2) * Time.deltaTime));
        }

        if (!_currentTarget) return;

        Vector3 dir = _currentTarget.transform.position - upperBody.position;
        dir.y = 0;

        upperBody.rotation = Quaternion.Slerp(upperBody.rotation, Quaternion.LookRotation(dir), UIManager.timeScale == 1 ? (Time.deltaTime * rotationSpeed) : (Time.deltaTime * (rotationSpeed * 2)));

    }

    private void OnEnable() => InvokeRepeating(nameof(ScanArea), 0, UIManager.timeScale == 1 ? scanRate : scanRate / 2);

    private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, maxDistance);

    public void SkillSiheld()
    {
        shieldVFX.gameObject.SetActive(true);
        shieldBar.SetActive(true);
        shieldFill.fillAmount = 1;
        Invoke(nameof(CloseShield), UIManager.timeScale == 1 ? shieldVFX.main.startLifetimeMultiplier : shieldVFX.main.startLifetimeMultiplier / 2);
        shieldFill.FillImageAnimation(shieldFill.fillAmount, 0, UIManager.timeScale == 1 ? shieldVFX.main.startLifetimeMultiplier : shieldVFX.main.startLifetimeMultiplier / 2).SetEase(Ease.Linear);
    }

    private void CloseShield()
    {
        shieldVFX.gameObject.SetActive(false);
        shieldBar.SetActive(false);
    }

    public void SkillMeteor()
    {
        meteorCanvas.SetActive(true);

        meteorAreaSprite.SetActive(true);
        Time.timeScale = .05f;
        meteorTimeText.IncrementalText(10, 0, .5f).SetEase(Ease.Linear);
        meteorTimeFill.FillImageAnimation(1, 0, .5f).SetEase(Ease.Linear).OnComplete(() => MeteorSkillEnd());
    }

    public void MeteorSkillEnd()
    {
        meteorCanvas.SetActive(false);
        meteorAreaSprite.SetActive(false);
        Time.timeScale = 1;
        meteorAreaSprite.transform.position = _initialMeteorAreaPosition;
    }

    private void ShakeEnabled() => _shakeable = true;

    private void ResetCameraPos() => Camera.main.transform.SmoothPosition(CameraEndAnimation.Instance._initialPosition, 1);
}
