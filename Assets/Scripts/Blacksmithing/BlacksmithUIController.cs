using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Manages all UI elements for the Blacksmithing minigame.
/// Handles score, timer, gameplay display, and result screen.
/// </summary>
public class BlacksmithUIController : MonoBehaviour
{
    [Header("Gameplay UI")]
    public GameObject gameplayUI;       // Parent object for in-game UI
    public TextMeshProUGUI scoreText;   // Displays current score
    public TextMeshProUGUI timerText;   // Displays countdown timer

    [Header("Result Screen")]
    public GameObject resultScreen;             // Parent object for result panel
    public TextMeshProUGUI finalScoreText;      // Displays final click score
    public TextMeshProUGUI crownsEarnedText;    // Displays crowns earned
    public Button restartButton;                // Button to restart game
    public Button mainMenuButton;               // Button to return to main menu

    /// <summary>
    /// Called at game start to enable gameplay UI and hide results.
    /// </summary>
    public void ShowGameplayUI()
    {
        gameplayUI.SetActive(true);
        resultScreen.SetActive(false);
    }

    /// <summary>
    /// Updates the score display during gameplay.
    /// </summary>
    public void UpdateScoreDisplay(int score)
    {
        scoreText.text = "Score: " + score;
    }

    /// <summary>
    /// Updates the timer display each frame.
    /// </summary>
    public void UpdateTimerDisplay(float timeRemaining)
    {
        timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
    }

   ///<summary>
   ///Shows the final result screen and sets values.
   ///</summary>
   public void ShowResults(int finalScore, int crownsEarned)
    {
        gameplayUI.SetActive(false);
        resultScreen.SetActive(true);

        finalScoreText.text = "Final Score: " + finalScore;
        crownsEarnedText.text = "Crowns Earned " + crownsEarned;
    }
   
}
