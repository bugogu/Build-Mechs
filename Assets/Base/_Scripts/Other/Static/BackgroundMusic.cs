using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    AudioSource _music;

    private GameObject[] _musics;
    private void Start()
    {
        _musics = GameObject.FindGameObjectsWithTag("Music");

        _music = GetComponent<AudioSource>();
        MusicEnabled();

        if (_musics.Length > 1)
            Destroy(_musics[1]);

        DontDestroyOnLoad(gameObject);
    }

    public void MusicEnabled()
    {
        if (PlayerPrefs.GetInt("soundStatus", 1) == 1)
            _music.Play();
        else
            _music.Stop();
    }
}
