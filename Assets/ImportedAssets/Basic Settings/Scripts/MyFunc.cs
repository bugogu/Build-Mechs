using UnityEngine;

[System.Serializable]
public class MyFunc
{
    public static void DoVibrate()
    {
        if (Setting.Settings.vibrateActive == 0) return;
        Handheld.Vibrate();
    }
    public static void PlaySound(AudioClip clip, GameObject sender)
    {
        if (Setting.Settings.soundActive == 0) return;
        SoundSystem.PlayOneShot(clip, "", 1, clip.length);
        Debug.Log("Sender" + sender, sender);
    }
}