using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls scene transitions from the main menu.
/// Allows the player to start the Joust minigame or return to the main menu.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Scene Names")]
    public string joustSceneName = "JoustScene";                // Name of the Joust minigame scene
    public string mainMenuSceneName = "RoyalTourneyMainMenu";   // Name of the main hub menu scene


    /// <summary>
    /// Loads the Joust minigame scene when the "Start" button is clicked.
    /// </summary>
    public void StartJoust()
    {
        SceneManager.LoadScene(joustSceneName);
    }

    /// <summary>
    /// Returns the player to the main menu scene.
    /// Useful from in-game post menus or credits.
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

}
