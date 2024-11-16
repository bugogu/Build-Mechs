using UnityEngine;

public class RocketBullet : MonoBehaviour
{
    [SerializeField] private float explosionDistance = 5;
    [SerializeField] private Vector3 explosionFXScale;
    [SerializeField] private AudioClip shootSfx;
    private float _bulletDamage;
    private Transform _explosionRocket;

    private void Awake()
    {
        _bulletDamage = transform.parent.parent.GetComponent<Weapon>().bulletDamage;
        _explosionRocket = transform.GetChild(0);
        MyFunc.PlaySound(shootSfx, gameObject);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.parent.parent.GetComponent<Weapon>().target), Time.deltaTime * 100);
        Invoke(nameof(DestroySelf), UIManager.timeScale == 1 ? 2 : 1);
    }

    private void DestroySelf() => Destroy(gameObject);

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        Collider[] _enemies = Physics.OverlapSphere(transform.position, explosionDistance);

        foreach (Collider enemy in _enemies)
            if (enemy.gameObject.TryGetComponent(out Enemy enemyScript))
                enemyScript.DamageTaken(_bulletDamage * GameManager.DamageMultiplier);

        if (_explosionRocket != null)
        {
            _explosionRocket.parent = null;
            _explosionRocket.localScale = explosionFXScale;
            _explosionRocket.gameObject.SetActive(true);
        }

        Destroy(gameObject);
    }

    // private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, explosionDistance);
}
