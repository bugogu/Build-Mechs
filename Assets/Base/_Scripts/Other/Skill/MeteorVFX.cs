using UnityEngine;

public class MeteorVFX : MonoBehaviour
{
    [SerializeField] private AudioClip triggerSound;

    private int _triggerCount;
    private ParticleSystem _particle;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 10);
    }

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();

        if (UIManager.timeScale == 1)
            _particle.playbackSpeed = 1;

        else
            _particle.playbackSpeed = 2;

        InvokeRepeating("TriggerSound", 0, UIManager.timeScale == 1 ? .2f : .1f);
    }

    private void Scan()
    {
        var _enemies = Physics.OverlapSphere(transform.position, 10);

        foreach (Collider enemy in _enemies)
        {
            if (enemy.gameObject.TryGetComponent(out Enemy target))
            {
                target.DamageTaken(UIManager.timeScale == 1 ? PlayerManager.Instance.DamagePerSecond : PlayerManager.Instance.DamagePerSecond / 2);
                GameManager.meteorDamage += Mathf.RoundToInt(UIManager.timeScale == 1 ? PlayerManager.Instance.DamagePerSecond : PlayerManager.Instance.DamagePerSecond / 2);
            }

        }
    }

    private void OnEnable() => InvokeRepeating("Scan", 0, UIManager.timeScale == 1 ? .4f : .2f);

    public void TriggerSound()
    {
        if (UIManager.timeScale == 1 ? _triggerCount >= 17 : _triggerCount >= 34) return;

        _triggerCount++;
        MyFunc.PlaySound(triggerSound, gameObject);
    }

}
