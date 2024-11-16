using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DailyReward : MonoBehaviour
{
    public const string lastClaimTime = "LastClaimTime";
    public Transform dailyElementsParent;
    public GameObject dailyBonusActive;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI remainingText;
    [SerializeField] private Button claimButton;
    [SerializeField] private AudioClip clickSFX;

    public static int ActiveDay
    {
        get => PlayerPrefs.GetInt("ActiveDay", 1);
        set => PlayerPrefs.SetInt("ActiveDay", value);
    }

    private void Start()
    {
        string _lastTime = PlayerPrefs.GetString(lastClaimTime, "");

        DateTime _lastClaimTime;

        if (!string.IsNullOrEmpty(_lastTime))
            _lastClaimTime = DateTime.Parse(_lastTime);

        else
            _lastClaimTime = DateTime.MinValue;

        if (DateTime.Today > _lastClaimTime)
        {
            claimButton.interactable = true;
            dailyBonusActive.SetActive(true);
        }


        else
            claimButton.interactable = false;

        titleText.text = "Daily Login Prizes<color=#19F461>  " + ActiveDay + "</color>/7";
    }

    private string GetTimeToNextClaim()
    {
        int hours = Mathf.FloorToInt((float)(DateTime.Today.AddDays(1) - DateTime.Now).TotalHours);
        int minutes = Mathf.FloorToInt((float)(DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes) % 60;
        return ("Remaining to prize " + hours + " Hours " + minutes + " Minutes");
    }

    public void UpdateRemainingText() => remainingText.text = GetTimeToNextClaim();

    public void OnClaimButtonPressed()
    {
        MyFunc.PlaySound(clickSFX, gameObject);
        PlayerPrefs.SetString(lastClaimTime, DateTime.Now.ToString());
        ClaimGift();
    }

    private void ClaimGift()
    {
        claimButton.interactable = false;

        dailyBonusActive.SetActive(false);

        CheckClaimedPrizes();

        dailyElementsParent.GetChild(ActiveDay - 1).GetComponent<DailyPrize>().ClaimDailyPrize();
        ActiveDay += 1;
        dailyElementsParent.GetChild(ActiveDay - 2).GetComponent<DailyPrize>().ClaimedEffect();

        if (ActiveDay == 8)
            titleText.text = "Daily Login Prizes<color=#19F461>  " + (ActiveDay - 1) + "</color>/7";

        else
            titleText.text = "Daily Login Prizes<color=#19F461>  " + ActiveDay + "</color>/7";

        UpdateRemainingText();
    }

    public void CheckClaimedPrizes()
    {
        for (int i = ActiveDay - 2; i > -1; i--)
            dailyElementsParent.GetChild(i).GetComponent<DailyPrize>().ClaimedEffect();
    }

    public void RestartDaily()
    {
        ActiveDay = 1;
        titleText.text = "Daily Login Prizes<color=#19F461>  " + ActiveDay + "</color>/7";
        UpdateRemainingText();
        CheckClaimedPrizes();
    }
}
