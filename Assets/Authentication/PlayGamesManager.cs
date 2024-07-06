using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using System;

public class PlayGamesManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI detailsText;
    private void Start()
    {
        SignIn();
    }
    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            try
            {
                string name = PlayGamesPlatform.Instance.GetUserDisplayName();
                string id = PlayGamesPlatform.Instance.GetUserId();
                string ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

                detailsText.text = $"Name: {name}\nID: {id}\nImage URL: {ImgUrl}";
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to retrieve user details: {ex.Message}");
                detailsText.text = "Failed to retrieve user details.";
            }
        }
        else
        {
            detailsText.text = $"Sign-in failed with status: {status}";
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            //PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        }
    }

}
