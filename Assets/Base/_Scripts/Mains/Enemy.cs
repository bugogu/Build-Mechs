using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    #region Variables

    public enum AttackType { Close, Range }
    public enum EnemyType { Normal, Boss }

    public Transform targetLocation;
    public float damageValue;

    [HideInInspector] public EnemyType enemyType;
    [SerializeField] private AttackType attackType;
    [SerializeField] private UnityEngine.UI.Image healthFill;
    [SerializeField] private UnityEngine.UI.Image effectFill;
    [SerializeField] private ParticleSystem hitBloodSplat;
    [SerializeField] private float bossRunSpeed;
    [SerializeField] private float bossHealthMultiplier;
    [SerializeField] private float bossScaleMultiplier;
    [SerializeField] private float runSpeed;
    [SerializeField] private float health;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject floatingCanvas;
    [SerializeField] private TMPro.TMP_Text floatingText;
    [SerializeField] private GameObject floatingExpCanvas;
    [SerializeField] private TMPro.TMP_Text floatingExpText;
    [SerializeField] private RectTransform floatingTextTransform;
    [SerializeField] private SkinnedMeshRenderer _smr;
    [SerializeField] private Material _hitEffect;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private AnimationClip attackAnimation;
    [SerializeField] private AnimationClip dieAnimation;
    [SerializeField] private AnimationClip getHitAnimation;
    [SerializeField] private Transform projectilePosition;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject criticDamageImage;
    [SerializeField] private RectTransform criticDamageTransform;


    private bool _isDeath;
    private Animator _anim;
    private Material _orginalMat;
    private Coroutine _hitRoutine;
    private Transform _player;
    private bool _hitted;
    private float _currentHealth;
    private bool _isAttacking = true;
    private GameObject _generated;

    #endregion

    private void Start() => Initial();

    private void FixedUpdate()
    {
        if (GameManager.Instance.gameOver) return;
        if (_isDeath) return;

        if (enemyType == EnemyType.Normal)
        {
            if (attackType == AttackType.Close)
                if (!_hitted)
                    transform.position += transform.forward * (UIManager.timeScale == 1 ? runSpeed : runSpeed * 2) * Time.fixedDeltaTime;

            if (attackType == AttackType.Range)
            {
                if (!_hitted)
                    if (Vector3.Distance(transform.position, _player.position) > attackRange)
                        transform.position += transform.forward * (UIManager.timeScale == 1 ? runSpeed : runSpeed * 2) * Time.fixedDeltaTime;
                    else
                        RangeAttack();
            }
        }
        else
            transform.position += transform.forward * (UIManager.timeScale == 1 ? bossRunSpeed : bossRunSpeed * 2) * Time.fixedDeltaTime;
    }

    public void DamageTaken(float damageValue)
    {
        GameManager.TotalGivenDamage += Mathf.RoundToInt(damageValue);

        if (GameManager.TotalGivenDamage == 1000)
            GameManager.Instance.UnlockAchievement(GameManager.Instance.noStrangerToFightingAchievementID);

        if (enemyType == EnemyType.Normal)
            hitBloodSplat?.Play();

        damageValue *= GameManager.damageBoost;

        EnemySpawner.Instance.DamageTake += Mathf.RoundToInt(damageValue);

        if (enemyType == EnemyType.Normal)
        {
            if (Random.Range(0, 101) <= 7.5f)
            {
                damageValue *= 2;
                DamagePopUp(Mathf.RoundToInt(damageValue), true);
            }

            else
                DamagePopUp(Mathf.RoundToInt(damageValue), false);
        }

        if (enemyType == EnemyType.Normal)
            if (!_hitted)
            {
                _hitted = true;
                Invoke(nameof(EnableRunning), getHitAnimation.length);
            }

        HitEffect();

        healthFill.fillAmount -= damageValue / health;
        _currentHealth -= damageValue;

        DOVirtual.Float(effectFill.fillAmount, healthFill.fillAmount, 1, x => effectFill.fillAmount = x);

        if (enemyType == EnemyType.Normal)
            _anim.SetTrigger("Hit");

        if (_currentHealth <= 0)
            DestroySelf();
    }

    private void DestroySelf()
    {
        GetComponent<Collider>().enabled = false;
        GameManager.tempProgress += 1.666666666666667f;
        UIManager.Instance.progressPercentText.text = "%" + GameManager.tempProgress.ToString("0.00");

        GameManager.tempCoin += 1;
        GameManager.tempDiamond += 1;
        GameManager.PlayerEXP += 1;
        MyFunc.PlaySound(GameManager.Instance.enemyDeathSFX, gameObject);
        _isDeath = true;
        _anim.SetTrigger("Die");
        _smr.material = _orginalMat;
        ExpPopUp();
        Instantiate(deathVFX, transform);
        Destroy(gameObject, dieAnimation.length - .2f);
        Invoke(nameof(LastEnemy), dieAnimation.length - .3f);
        EnemySpawner.Instance.KilledEnemyCount++;

        if (_generated != null)
        {
            _generated.GetComponent<EnemyBullet>().GenerateDestroyFX();
            _generated.SetActive(false);
        }

        if (EnemySpawner.Instance.killedEnemys >= 60)
        {
            GameManager.playerFatality = true;
            CameraEndAnimation.Instance.StartCameraAnimation(.20f);
        }

        if (enemyType == EnemyType.Boss)
        {
            UIManager.Instance.EnableLevelEnd(true);

            if (((GameManager.Prestige * 15) + GameManager.Level) == 0)
                GameManager.Instance.UnlockAchievement(GameManager.Instance.challengerAchievementID);
        }

        GameManager.TotalDefeatedEnemy++;

        if (GameManager.TotalDefeatedEnemy == 100)
            GameManager.Instance.UnlockAchievement(GameManager.Instance.slimeDefeatAchievementID);
    }

    private void Attack()
    {
        if (_isDeath) return;

        if (damageValue >= PlayerManager.Instance._activeHealth)
            GameManager.playerFatality = true;

        EnemySpawner.Instance.killedEnemys++;
        _isDeath = true;
        GetComponent<Collider>().enabled = false;
        _anim.SetTrigger("Attack");
        _smr.material = _orginalMat;

        if (enemyType == EnemyType.Boss)
            UIManager.Instance.EnableLevelEnd(false);

        Invoke(nameof(PlayFX), attackAnimation.length - .15f);
        Invoke(nameof(LastEnemy), dieAnimation.length - .3f);
        Destroy(gameObject, attackAnimation.length);
    }

    private void RangeAttack()
    {
        if (_isDeath) return;
        if (!_isAttacking) return;
        _isAttacking = false;

        Invoke(nameof(EnableAttacking), 3);

        if (enemyType == EnemyType.Boss)
            UIManager.Instance.EnableLevelEnd(false);

        _anim.SetTrigger("Attack");
    }

    public void GenerateProjectile()
    {
        _generated = Instantiate(projectilePrefab, projectilePosition);
        _generated.transform.parent = null;
        _generated.GetComponent<EnemyBullet>().damageValue = this.damageValue;

        var direction = _player.position - _generated.transform.position;

        _generated.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileSpeed, ForceMode.Impulse);
    }

    private void PlayFX()
    {
        GameObject spawnedFX = Instantiate(deathVFX, transform);
        spawnedFX.transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
            Attack();

        if (other.CompareTag("Rocket"))
            RocketTrigger(other.gameObject);

        if (other.CompareTag("Shield"))
            if (enemyType != EnemyType.Boss)
                DestroySelf();

    }

    private void RocketTrigger(GameObject rocket)
    {
        GameManager.rocketDamage += Mathf.RoundToInt((((GameManager.Prestige * 15) + GameManager.Level) / 4 + 1) * GameManager.DamageMultiplier);
        DamageTaken((((GameManager.Prestige * 15) + GameManager.Level) / 4 + 1) * GameManager.DamageMultiplier);
        rocket.SetActive(false);
    }

    public void DamageGivenToPlayer() => PlayerManager.Instance.TakeDamage(damageValue);

    private System.Collections.IEnumerator HitRoutine()
    {
        _smr.material = _hitEffect;
        yield return new WaitForSeconds(0.005f);
        _smr.material = _orginalMat;
        _hitRoutine = null;
    }

    public void HitEffect()
    {
        if (_hitRoutine != null)
        {
            StopCoroutine(_hitRoutine);
        }
        _hitRoutine = StartCoroutine(HitRoutine());
    }

    private void Initial()
    {
        health = enemyType == EnemyType.Normal ? (GameManager.Prestige * 15) + (GameManager.Level * 3) + health : health * bossHealthMultiplier;
        damageValue = enemyType == EnemyType.Normal ? GameManager.Prestige * 15 + (GameManager.Level) + damageValue : GameManager.Health;
        _anim = GetComponent<Animator>();
        _orginalMat = _smr.material;
        _currentHealth = health;

        if (enemyType == EnemyType.Boss)
            transform.localScale *= bossScaleMultiplier;

        if (attackType == AttackType.Range)
            _player = GameObject.FindWithTag("Player").transform;
    }

    private void EnableRunning() => _hitted = false;

    private void LastEnemy()
    {
        if (enemyType == EnemyType.Normal)
            if (EnemySpawner.Instance.killedEnemys == 3 * EnemySpawner.Instance.enemysPerWave) UIManager.Instance.EnableLevelEnd(true);
    }

    private void DamagePopUp(int damageValue, bool criticialHit)
    {
        if (criticialHit)
        {
            criticDamageImage.SetActive(true);
            floatingText.color = Color.red;
        }
        else
        {
            criticDamageImage.SetActive(false);
            floatingText.color = Color.white;
        }

        floatingCanvas.SetActive(true);
        floatingText.text = "-" + damageValue.ToString();
        criticDamageTransform.DOMoveY(0, .2f).From();
        floatingTextTransform.DOMoveY(0, .2f).From().OnComplete(() => floatingCanvas.SetActive(false));
    }

    private void ExpPopUp()
    {
        if (enemyType == EnemyType.Boss) return;
        floatingExpCanvas.SetActive(true);
        floatingExpText.transform.DOMoveY(0, .2f).From();
    }

    private void EnableAttacking() => _isAttacking = true;

    public void FatalityAnimation() =>
        CameraEndAnimation.Instance.StartCameraAnimation();
}
