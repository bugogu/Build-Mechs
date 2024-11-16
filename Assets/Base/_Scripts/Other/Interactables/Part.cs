using UnityEngine;
using DG.Tweening;

public class Part : MonoBehaviour, IInteractable
{
    [SerializeField] private Material[] trailMaterials;
    [SerializeField] private Vector3 movePosition;
    [SerializeField] private AudioClip clickSFX;
    private TrailRenderer _trail;

    public void Interact()
    {
        GetComponent<BoxCollider>().enabled = false;
        MyFunc.PlaySound(clickSFX, gameObject);
        transform.SmoothPosition(movePosition, UIManager.timeScale == 1 ? 1 : .5f).OnComplete(() => SearchEmptyPlace());
    }


    private void OnEnable()
    {
        _trail = GetComponent<TrailRenderer>();

        _trail.material = trailMaterials[Random.Range(0, trailMaterials.Length)];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Base")) return;

        transform.SmoothPosition(movePosition, UIManager.timeScale == 1 ? 1 : .5f).OnComplete(() => SearchEmptyPlace());
    }

    private void SearchEmptyPlace()
    {
        for (int i = 0; i < DropManager.Instance.mergePlaces.Length; i++)
            if (DropManager.Instance.mergePlaces[i].transform.childCount < 1)
            {
                DropManager.Instance.GenerateItem(i);
                break;
            }

        gameObject.SetActive(false);
    }
}