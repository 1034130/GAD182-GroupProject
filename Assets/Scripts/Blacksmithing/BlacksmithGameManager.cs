using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the blacksmithing minigame lifecycle:
/// - Starts the game
/// - Tracks time and score
/// - Ends the game and calculates rewards
/// - Handles scene transitions
/// </summary>
public class BlacksmithGameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameDuration = 20f;    // Total game time in seconds

    [Header("References")]
    public BlacksmithUIController uiController;     // Reference to UI controller for updating the interface

    private float remainingTime;            // Countdown timer
    private int score = 0;                  // Total points earned from clicking
    private bool gameActive = false;        // Whether the game is currently running
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameActive) return;

        remainingTime -= Time.deltaTime;
        uiController.UpdateTimer.Display(remainingTime);

        if (remainingTime <= 0f)
        {
            EndGame();
        }
    }

    /// <summary>
    /// Initializes game variables, activates the game state,
    /// and shows the gameplay UI.
    /// </summary>
    public void StartGame()
    {
        remainingTime = gameDuration;
        score = 0;
        gameActive = true;

        uiController.ShowGameplayUI();
        uiController.UpdateScoreDisplay(score);
        uiController.UpdateTimerDisplay(remainingTime);
    }

    /// <summary>
    /// Increases score when the player clicks on the sword.
    /// </summary>
    public void AddClickScore()
    {
        if (!gameActive) return;

        score += 1;
        uiController.UpdateScoreDisplay(score);
    }

    /// <summary>
    /// Called when time runs out.
    /// Calculates rewards and shows results.
    /// </summary>
    private void EndGame()
    {
        gameActive = false;

        int crownsEarned = CalculateCrowns(score);
        uiController.ShowResults(score, crownsEarned);
    }

    /// <summary>
    /// Converts player score to in-game rewards.
    /// Modify this formula to adjust difficulty/reward balance.
    /// </summary>
    private int CalculateCrowns(int finalScore)
    {
        return Mathf.FloorToInt(finalScore / 5f);
    }


    /// <summary>
    /// Reloads the current scene, effectively restarting the minigame.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// Returns to the main menu of the full minigame collection.
    /// Ensure "RoyalTourneyMainMenu" is in Build Settings.
    /// </summary>
    public void ReturnToMain()
    {
        SceneManager.LoadScene("RoyalTourneyMainMenu");
    }
}
