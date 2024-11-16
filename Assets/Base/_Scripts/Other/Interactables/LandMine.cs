using UnityEngine;
using DG.Tweening;

public class LandMine : MonoBehaviour, IInteractable
{
    [SerializeField] private float explosionRadius = 5;
    [SerializeField] private ParticleSystem explosionFX;
    [SerializeField] private AudioClip explosionSFX;
    [SerializeField] private Transform radiusSptire;
    [SerializeField] private AudioClip clickSFX;

    private Tween _shakeScale;

    private void Start()
    {
        radiusSptire.parent = null;
        _shakeScale = transform.DOShakeScale(1, 20, 20, 120).SetEase(Ease.Linear).SetLoops(100, LoopType.Restart);
    }

    public void Interact()
    {
        GetComponent<BoxCollider>().enabled = false;
        MyFunc.PlaySound(clickSFX, gameObject);
        MyFunc.PlaySound(explosionSFX, gameObject);
        MyFunc.DoVibrate();
        explosionFX.Play();

        Camera.main.DOShakePosition(.2f, 5, 20, 150);
        PlayerManager.Instance.boomTextFX?.Play();

        var enemies = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider enemy in enemies)
        {
            if (enemy.gameObject.TryGetComponent(out Enemy target))
                target.DamageTaken(GameManager.landMineDamage);
        }

        radiusSptire.gameObject.SetActive(false);
        _shakeScale.Kill();
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 2.5f);

    }

    private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, explosionRadius);
}
