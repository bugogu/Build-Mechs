using UnityEngine;

public class CameraEndAnimation : MonoSing<CameraEndAnimation>
{
    [HideInInspector] public Vector3 _initialPosition;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private Transform endAnimationPosition;

    private bool _isTrigger = true;

    public void StartCameraAnimation(float animationDurationOption = 0)
    {
        if (!GameManager.playerFatality) return;

        if (!_isTrigger) return;

        _isTrigger = false;

        if (animationDurationOption > 0)
        {
            transform.SmoothPosition(endAnimationPosition.position, animationDurationOption);

            Time.timeScale = .15f;
            Invoke(nameof(EndSlowMotion), animationDurationOption);
            Invoke(nameof(RestartPosition), 1);
        }
        else
        {
            transform.SmoothPosition(endAnimationPosition.position, animationDuration);

            Time.timeScale = .15f;
            Invoke(nameof(EndSlowMotion), animationDuration);
        }

    }

    private void EndSlowMotion() => Time.timeScale = 1;
    private void RestartPosition()
    {
        if (GameManager.Instance.gameOver) return;
        transform.SmoothPosition(_initialPosition, 1);
        Invoke("EnableTrigger", 10);
    }

    public void SetInitialPos() =>
        _initialPosition = transform.position;

    private void EnableTrigger() => _isTrigger = true;
}
