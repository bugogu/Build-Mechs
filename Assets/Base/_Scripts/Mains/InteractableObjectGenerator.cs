using UnityEngine;

public class InteractableObjectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] interactObjectsPrefab;
    [SerializeField] private Transform[] itemSpawnPositions;

    private void Start() =>
        InvokeRepeating(nameof(ItemSpawner), 15, 20);

    private void ItemSpawner()
    {
        if (GameManager.bossLevel) return;
        if (GameManager.Instance.gameOver) return;

        var item = Instantiate(interactObjectsPrefab[Random.Range(0, interactObjectsPrefab.Length)],
          itemSpawnPositions[Random.Range(0, itemSpawnPositions.Length)]);

        item.GetComponent<BoxCollider>().enabled = true;
    }
}
