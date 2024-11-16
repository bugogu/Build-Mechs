using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float bulletDamage = 3;
    [HideInInspector] public Vector3 target;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float bulletForce = 20;
    [SerializeField] private Transform barrel;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private PlayerManager playerScript;
    private Transform _currentEnemy;

    private void Fire()
    {
        _currentEnemy = playerScript.GetComponent<PlayerManager>()._currentTarget.transform;
        if (!_currentEnemy) return;

        Vector3 dir = _currentEnemy.transform.position - transform.position;
        dir.y = 0; target = dir;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-dir), UIManager.timeScale == 1 ? (Time.deltaTime * 100) : (Time.deltaTime * 50));

        GameObject _spawnedBullet = Instantiate(bulletPrefab, barrel);

        _spawnedBullet.transform.localPosition = Vector3.zero;

        _spawnedBullet.transform.parent = null;

        Vector3 targetDirection = _currentEnemy.position - transform.position;

        _spawnedBullet.transform.GetComponent<Rigidbody>().AddForce(targetDirection.normalized * (UIManager.timeScale == 1 ? bulletForce : bulletForce * 2), ForceMode.Impulse);

    }

    private void OnEnable() =>
        StartCoroutine(FireLoop());

    private void OnDisable() =>
         StopCoroutine(FireLoop());

    private System.Collections.IEnumerator FireLoop()
    {
        while (true)
        {
            while (true)
            {
                yield return new WaitForSeconds(UIManager.timeScale == 1 ? fireRate : fireRate / 2);

                try
                {
                    Fire();
                }
                catch (System.Exception e)
                {
                    Debug.Log("FireLoop Coroutine Error: " + e.Message);
                }
            }
        }
    }

}
