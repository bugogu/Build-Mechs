using UnityEngine;
using DG.Tweening;

public class EnemySpawner : MonoSing<EnemySpawner>
{

    [HideInInspector] public int killedEnemys;
    [HideInInspector] public int takedDamage;
    [HideInInspector] public float enemysPerWave = 20;
    [SerializeField] private float wavesBetweenTimeout = 4;
    [SerializeField] private float spawnRate = .7f;

    [Space]
    [SerializeField] private GameObject[] enemys;
    [SerializeField] private GameObject[] spawnLocations;
    [SerializeField] private GameObject[] spawnEffects;

    [Space]
    [SerializeField] private TMPro.TMP_Text waveText;
    [SerializeField] private TMPro.TMP_Text timeoutText;
    [SerializeField] private TMPro.TMP_Text killedText;
    [SerializeField] private TMPro.TMP_Text givenDamageText;

    [Space]
    [Header("Boss Wave")]
    [SerializeField] private GameObject bossPortal;
    [SerializeField] private GameObject[] bosses;

    public int KilledEnemyCount
    {
        get => killedEnemys;
        set
        {
            killedEnemys = value;
            killedText.text = killedEnemys.ToString();
        }
    }

    public int DamageTake
    {
        get => takedDamage;
        set
        {
            takedDamage = value;
            givenDamageText.text = takedDamage.ToString();
        }
    }

    private Transform mechTransform;
    private int activeEnemyIndex;
    private int _randomIndexHolder;

    private void Awake()
    {
        mechTransform = GameObject.FindWithTag("Player").transform;

        if (GameManager.Prestige == 0)
        {
            if (GameManager.activeEnemy == ActiveEnemy.slime)
                activeEnemyIndex = 0;

            if (GameManager.activeEnemy == ActiveEnemy.turtle)
                activeEnemyIndex = 1;

            if (GameManager.activeEnemy == ActiveEnemy.wizzard)
                activeEnemyIndex = 2;
        }
    }

    private System.Collections.IEnumerator Spawner()
    {
        if (!GameManager.Instance.gameOver)
        {
            if (GameManager.Level != 4 && GameManager.Level != 9 && GameManager.Level != 14)
            {

                for (int j = 0; j < 3; j++)
                {

                    if (j == 0)
                    {
                        yield return new WaitForSeconds(UIManager.timeScale == 1 ? .6f : .3f);
                        DropManager.Instance.SendDrop();
                    }
                    else
                        DropManager.Instance.SendDrop();

                    yield return new WaitForSeconds(UIManager.timeScale == 1 ? 1.5f : .75f);
                    waveText.text = "Wave " + (j + 1) + "/3";
                    timeoutText.gameObject.SetActive(true);
                    timeoutText.IncrementalText(10, 0, UIManager.timeScale == 1 ? 10 : 5).SetEase(Ease.Linear).OnComplete(() => timeoutText.gameObject.SetActive(false));

                    yield return new WaitForSeconds(UIManager.timeScale == 1 ? wavesBetweenTimeout : wavesBetweenTimeout / 2);

                    for (int i = 0; i < enemysPerWave; i++)
                    {
                        yield return new WaitForSeconds(UIManager.timeScale == 1 ? spawnRate : spawnRate / 2);
                        var randomLoc = Random.Range(0, spawnLocations.Length);
                        _randomIndexHolder = randomLoc;
                        spawnEffects[randomLoc].SetActive(true);
                        spawnEffects[randomLoc].transform.DOScale(new Vector3(2.285296f, 2.285296f, 2.285296f), .5f).
                        OnComplete(() => EnemySpawn(randomLoc));
                        Invoke(nameof(ClosePortal), UIManager.timeScale == 1 ? .6f : .3f);
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(UIManager.timeScale == 1 ? 1f : .5f);

                DropManager.Instance.SendDrop();

                yield return new WaitForSeconds(UIManager.timeScale == 1 ? 1f : .5f);

                DropManager.Instance.SendDrop();

                waveText.text = "Boss Incoming";
                waveText.color = Color.red;
                timeoutText.gameObject.SetActive(true);

                timeoutText.IncrementalText(15, 0, UIManager.timeScale == 1 ? 15 : 7.5f).SetEase(Ease.Linear).
                OnComplete(() => CloseTexts());

                yield return new WaitForSeconds(UIManager.timeScale == 1 ? (wavesBetweenTimeout + 5) : (wavesBetweenTimeout + 5) / 2);

                bossPortal.SetActive(true);
                bossPortal.transform.DOScale(Vector3.zero, .5f).From().OnComplete(() => BossSpawn());

            }
        }
    }

    private void ClosePortal()
    {
        spawnEffects[_randomIndexHolder].transform.DOScale(Vector3.zero, UIManager.timeScale == 1 ? .5f : .25f).OnComplete(
            () => spawnEffects[_randomIndexHolder].SetActive(false));
    }

    private void EnemySpawn(int randomIndex)
    {
        if (GameManager.Instance.gameOver) return;

        if (GameManager.Prestige == 0)
        {
            GameObject spawnedEnemy = Instantiate(enemys[activeEnemyIndex], spawnLocations[randomIndex].transform, false);
            spawnedEnemy.transform.rotation = Quaternion.LookRotation(mechTransform.position - spawnedEnemy.transform.position);
        }
        else
        {
            GameObject spawnedEnemy = Instantiate(enemys[Random.Range(0, enemys.Length)], spawnLocations[randomIndex].transform, false);
            spawnedEnemy.transform.rotation = Quaternion.LookRotation(mechTransform.position - spawnedEnemy.transform.position);
        }
    }

    private void BossSpawn()
    {
        GameObject spawnedBoss = Instantiate(bosses[activeEnemyIndex], bossPortal.transform, false);
        spawnedBoss.GetComponent<Enemy>().enemyType = Enemy.EnemyType.Boss;
        spawnedBoss.transform.rotation = Quaternion.LookRotation(mechTransform.position - spawnedBoss.transform.position);
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(Spawner));
    }

    private void CloseTexts()
    {
        timeoutText.gameObject.SetActive(false);
        waveText.gameObject.SetActive(false);
        UIManager.Instance.bossTextShadow.SetActive(false);
    }
}
