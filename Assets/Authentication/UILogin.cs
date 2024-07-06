using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private LoginController loginController;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        loginButton.onClick.AddListener(LoginButtonPressed);
        loginController.OnSignedIn += LoginController_OnSignedIn;
    }

    private void LoginController_OnSignedIn(PlayerInfo playerInfo, string playerName)
    {
        string idAndName = "id: " + playerInfo.Id.ToString() + "\nname: " + playerName;
        text.text = idAndName;
    }

    private async void LoginButtonPressed()
    {
        await loginController.InitSignIn();
    }

    private void OnDisable()
    {
        loginButton.onClick?.RemoveListener(LoginButtonPressed);
        loginController.OnSignedIn -= LoginController_OnSignedIn;
    }
}
