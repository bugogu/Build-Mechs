using UnityEngine;

public class LoadScene : MonoBehaviour
{

    [SerializeField] private TMPro.TMP_Text versionText;

    void Awake() => versionText.text = Application.version;

    void Start() => LoadGame();

    public void LoadGame()
    {
        StartCoroutine(LoadingScene());
    }

    System.Collections.IEnumerator LoadingScene()
    {
        yield return new WaitForSeconds(5.2f);
        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }
}