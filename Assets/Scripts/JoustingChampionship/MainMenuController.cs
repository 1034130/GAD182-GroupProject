using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string joustSceneName = "JoustScene";
    public string mainMenuSceneName = "RoyalTourneyMainMenu";

    public void StartJoust()
    {
        SceneManager.LoadScene(joustSceneName);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

}
