using UnityEngine;
using DG.Tweening;

public class MeteorSkill : MonoBehaviour
{
    [SerializeField] private ParticleSystem meteorVFX;
    [SerializeField] private GameObject meteorBar;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private UnityEngine.UI.Image meteorFill;
    [SerializeField] private float positionSpeed;

    private GameObject spawnedMetorRail;

    private void Update()
    {

        if (Input.touchCount > 0)
        {
            transform.position += (new Vector3(Input.GetTouch(0).deltaPosition.x, 0, Input.GetTouch(0).deltaPosition.y) * positionSpeed);
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
                GenerateMeteorRail();
        }

    }

    private void GenerateMeteorRail()
    {

        spawnedMetorRail = Instantiate(meteorPrefab, transform);
        spawnedMetorRail.transform.parent = null;
        Invoke(nameof(DestroyVFX), UIManager.timeScale == 1 ? 4 : 2);

        PlayerManager.Instance.MeteorSkillEnd();
        meteorBar.SetActive(true);
        meteorFill.FillImageAnimation(1, 0, UIManager.timeScale == 1 ? 4 : 2).SetEase(Ease.Linear).OnComplete(() => meteorBar.SetActive(false));

    }

    private void DestroyVFX() => Destroy(spawnedMetorRail);

}
