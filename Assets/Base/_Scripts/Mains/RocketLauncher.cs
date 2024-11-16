using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class RocketLauncher : MonoBehaviour
{
    public AudioClip rocketSendSFX;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject rocketReloadCanvas;
    [SerializeField] private ParticleSystem rocketSmokeFX;
    [SerializeField] private UnityEngine.UI.Image rocketReloadFill;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private Vector3 lastPos;

    [Space]

    [SerializeField] private float velocity;

    private GameObject _currentRocket;
    private bool _sendable;
    private Ray _ray;
    private Camera _cam;
    private RaycastHit _hit;
    private Queue<GameObject> _pooledObjects;

    private void Awake()
    {
        _cam = Camera.main;
        _pooledObjects = new Queue<GameObject>();
    }

    private void GeneratePool()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject rocket = Instantiate(rocketPrefab);
            rocket.SetActive(false);
            _pooledObjects.Enqueue(rocket);
        }
    }

    private GameObject GetPooledObject()
    {
        GameObject rocket = _pooledObjects.Dequeue();
        rocket.SetActive(false);
        rocket.SetActive(true);
        _pooledObjects.Enqueue(rocket);
        return rocket;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mousePos = Input.mousePosition;
        _ray = _cam.ScreenPointToRay(mousePos);

        if (!Physics.Raycast(_ray, out _hit, 100)) return;

        if (!_hit.collider.TryGetComponent(out Enemy enemy)) return;

        if (!_sendable) return;

        if (_currentRocket == null) return;

        SendRocket(enemy.transform);
    }

    private void GenerateRocket()
    {
        GameObject activatedRocket = GetPooledObject();

        activatedRocket.transform.localPosition = spawnPos;
        activatedRocket.transform.localRotation = Quaternion.Euler(Vector3.zero);
        activatedRocket.transform.DOLocalMove(lastPos, UIManager.timeScale == 1 ? .5f : .25f).SetEase(Ease.InOutBack).OnComplete(() => _sendable = true);

        _currentRocket = activatedRocket;
    }

    private void SendRocket(Transform target)
    {
        rocketSmokeFX?.Play();
        MyFunc.PlaySound(rocketSendSFX, gameObject);
        _sendable = false;

        _currentRocket.transform.localRotation = Quaternion.Euler(Vector3.right * 90);

        if (!GameManager.bossLevel)
            _currentRocket.transform.DOLocalJump(target.position, UIManager.timeScale == 1 ? velocity : velocity * 2, 1, UIManager.timeScale == 1 ? .5f : .25f);
        else
            _currentRocket.transform.DOLocalJump(target.position + Vector3.forward * 2, velocity, 1, UIManager.timeScale == 1 ? .5f : .25f);

        rocketReloadCanvas.SetActive(true);
        rocketReloadFill.FillImageAnimation(1, 0, UIManager.timeScale == 1 ? .5f : .25f).SetEase(Ease.Linear).OnComplete(() => rocketReloadCanvas.SetActive(false));
        _currentRocket = null;
        GenerateRocket();
    }

    private void LauncherStartAnimation() => transform.DOMoveY(1.0497f, 1).SetEase(Ease.InOutBack).OnComplete(() => GenerateRocket());

    private void OnEnable()
    {
        GeneratePool();

        EventManager.AddListener(GameEvent.OnLevelStarted, LauncherStartAnimation);
    }

    private void OnDisable() => EventManager.RemoveListener(GameEvent.OnLevelStarted, LauncherStartAnimation);
}
