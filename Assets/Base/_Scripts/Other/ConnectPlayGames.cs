using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class ConnectPlayGames : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text signDetailText;

    public void Start()
    {
        // Social.localUser.Authenticate((bool success) =>
        // {
        //     if (success)
        //     {
        //         Debug.Log("Google Play Games Services'e başarıyla oturum açıldı.");
        //     }
        //     else
        //     {
        //         Debug.Log("Google Play Games Services oturum açma başarısız oldu.");
        //     }
        // });

        SignIn();
    }

    private void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            string imageUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

            signDetailText.text = "Success Login " + name;
        }
        else
        {
            signDetailText.text = "<color=#DE1F24>  Google Play Games Authentication Failed! </color>";
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }
}
