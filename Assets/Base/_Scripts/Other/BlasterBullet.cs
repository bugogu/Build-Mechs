using UnityEngine;

public class BlasterBullet : MonoBehaviour
{
    [SerializeField] private AudioClip shootSfx;
    private float _bulletDamage;
    private void Awake()
    {
        _bulletDamage = transform.parent.parent.GetComponent<Weapon>().bulletDamage;
        MyFunc.PlaySound(shootSfx, gameObject);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.parent.parent.GetComponent<Weapon>().target), Time.deltaTime * 100);
        Invoke(nameof(DestroySelf), UIManager.timeScale == 1 ? 2 : 1);
    }

    private void DestroySelf() => Destroy(gameObject);

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        other.GetComponent<Enemy>().DamageTaken(_bulletDamage * GameManager.DamageMultiplier);
        Destroy(gameObject);
    }
}
