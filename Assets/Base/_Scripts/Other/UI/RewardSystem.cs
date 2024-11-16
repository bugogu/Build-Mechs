using UnityEngine;

public class RewardSystem : MonoSing<RewardSystem>
{
    [SerializeField] float countdownTime;
    [SerializeField] private GameObject activeImage;
    public TMPro.TMP_Text countdownText;

    [HideInInspector] public float currentTime;
    [HideInInspector] public bool isCounting = true;

    private void Start() => currentTime = PlayerPrefs.HasKey("RewardCW") ? /*PlayerPrefs.GetFloat("RewardCW", countdownTime)*/ 0 : 0;

    private void Update()
    {
        if (isCounting)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                isCounting = false;
                currentTime = 0f;
                countdownText.gameObject.SetActive(false);

                UIManager.Instance.rewardCollectable = true;
                UIManager.Instance.collectRewardText.SetActive(true);
                activeImage.SetActive(true);
            }

            UpdateCountdownText();
        }
    }

    private void UpdateCountdownText()
    {
        int hours = Mathf.FloorToInt(currentTime / 3600f);
        int minutes = Mathf.FloorToInt((currentTime % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        countdownText.text = string.Format("{0}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}