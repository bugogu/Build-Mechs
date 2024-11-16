using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damageValue;
    [SerializeField] private GameObject destroyFX;

    private GameObject _spawnedFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            DamageGiven(other.GetComponent<PlayerManager>());

        if (other.CompareTag("Shield"))
            if (!GameManager.bossLevel)
                DestroySelf();

        if (other.CompareTag("ProjectileTrigger"))
            if (damageValue >= PlayerManager.Instance._activeHealth)
            {
                GameManager.playerFatality = true;
                CameraEndAnimation.Instance.StartCameraAnimation(.40f);
                if (GameManager.Instance.gameOver)
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
    }

    private void DamageGiven(PlayerManager player)
    {
        player.TakeDamage(damageValue);
        Destroy(gameObject);
    }

    private void DestroySelf()
    {
        _spawnedFX = Instantiate(destroyFX, transform);
        _spawnedFX.transform.parent = null;
        Invoke(nameof(CloseProjectileDestroyFX), 1.1f);
        Destroy(gameObject);
    }

    private void CloseProjectileDestroyFX() => _spawnedFX.SetActive(false);

    public void GenerateDestroyFX()
    {
        _spawnedFX = Instantiate(destroyFX, transform);
        _spawnedFX.transform.parent = null;
    }
}
