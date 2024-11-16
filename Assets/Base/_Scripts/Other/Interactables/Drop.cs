using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class Drop : MonoBehaviour, IInteractable
{

    //[HideInInspector] public int maxPartCount;
    [SerializeField] private Transform[] partJumpTransforms;
    [SerializeField] private GameObject partPrefab;
    [SerializeField] private GameObject parachute;
    [SerializeField] private GameObject smokeFX;
    [SerializeField] private GameObject starFX;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private AudioClip clickSFX;

    [Space(20)]
    [Header("Part Jump")]
    [SerializeField] private float maxDuration;
    [SerializeField] private float minDuration;
    [SerializeField] private float maxPower;
    [SerializeField] private float minPower;

    private int _generatePartCount;

    private bool _isRotateable = false;

    private bool _isInteractable = false;

    private List<GameObject> _generatedPart = new List<GameObject>();

    private void OnEnable()
    {
        _generatePartCount = DropManager.Instance.partCount;

        for (int i = 0; i < _generatePartCount; i++)
        {
            var generatedPart = Instantiate(partPrefab, transform);

            generatedPart.transform.localScale /= 3;

            generatedPart.transform.parent = null;

            _generatedPart.Add(generatedPart);
        }
    }

    private void FixedUpdate()
    {
        if (!_isRotateable) return;

        transform.Rotate(Vector3.up * (UIManager.timeScale == 1 ? (Time.fixedDeltaTime * rotateSpeed) : (Time.fixedDeltaTime * rotateSpeed) * 2));
    }

    public void Interact()
    {
        if (!_isInteractable) return;

        GetComponent<BoxCollider>().enabled = false;

        MyFunc.PlaySound(clickSFX, gameObject);

        MechanicInteract();
        VisualInteract();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Base")) return;

        MyFunc.DoVibrate();
        _isRotateable = true;
        starFX.SetActive(true);
        parachute.SetActive(false);
        _isInteractable = true;
        starFX.transform.parent = null;
    }

    private void CloseFX() => smokeFX.SetActive(false);

    private void VisualInteract()
    {
        smokeFX.transform.parent = null;
        smokeFX.GetComponent<ParticleSystem>().Play();
        starFX.SetActive(false);
        gameObject.SetActive(false);
        Invoke(nameof(CloseFX), UIManager.timeScale == 1 ? 1.5f : .75f);
    }

    private void MechanicInteract()
    {
        var seq = DOTween.Sequence();

        foreach (GameObject part in _generatedPart)
            seq.Append(part.transform.DOLocalJump(partJumpTransforms[Random.Range(0, partJumpTransforms.Length)].position,
            Random.Range(minPower, maxPower), 1, Random.Range(minDuration, maxDuration)).
            OnComplete(() => part.gameObject.GetComponent<BoxCollider>().enabled = true));
    }
}
