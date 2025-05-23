using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the core gameplay loop of the jousting game.
/// Manages countdowns, round timers, marker evaluation, opponent progression, win/loss outcomes, and championship logic.
/// </summary>
public class JoustGameManager : MonoBehaviour
{
    [Header("Core Components")]
    public TimingBarController timingBar;       // Reference to the timing bar controller
    public JoustUiController ui;                // UI controller for displaying countdowns and results
    public KnightMovement playerKnight;         // Controller for Knight movement (Player knight)
    public KnightMovement opponentKnight;       // Controller for Knight movement (Opponent knight)
    public Transform side1CenterPoint;         // Point that player charges toward
    public Transform side2CenterPoint;       // Point that opponent charges toward
    public Transform playerStartPosition;       // Transform marking thhe player's start position
    public Transform opponentStartPosition;     // Transform marking the opponent's start position

    private bool flipSides = false;             // Tracks which side each knight is on
    private int roundCounter = 1;               // Resets each new opponent
    private int totalCrowns = 0;                // Resets after
    private int championBonus = 0;              // Extra amount received for succesfully beating the championship  

    [Header("Timing Bar Settings")]
    public float baseSpeed = 100f;              // Speed for first opponent
    public float speedIncreasePerOpponent = 50f;// Speed increase per opponent


    [Header("Round Settings")]
    public float countdownTime = 3f;           // Duration of the pre-round countdown
    public float contactTime = 3f;             // Max time player has to input before automatic dismount
    public int totalOpponents = 6;             // Number of opponents needed to win championship


    [Header("Timing Delays")]
    public float delayAfterRound = 3f;      // Delay between rounds (win, draw)
    public float delayAfterChampionship = 5f; // Delay after championship win

    private float roundTimer = 0f;             // Tracks time since round began
    private bool roundActive = false;          // Whether the round is currently active
    private bool inputReceived = false;        // Tracks if player has already pressed space
    private int currentOpponent = 1;           // Current opponent index (1 = easiest)

    [Header("Opponent Sprite Sets")]
    public KnightSpriteSet[] opponentSprites;

    private void Start()
    {
       ui.restartButton.onClick.AddListener(RestartGame);
       ui.mainMenuButton.onClick.AddListener(ReturnToMainMenu);
       StartRound(); // Begin first round on game start
    }

    /// <summary>
    /// Starts a new round; resets state, hides UI, begins countdown.
    /// </summary>
    public void StartRound()
    {
        // Assign player's chosen sprite set from KnightSelector
        playerKnight.AssignSpriteSet(KnightSelector.SelectedKnightSet);

        bool playerIsSide1 = !flipSides;

        playerKnight.SetSide(playerIsSide1);
        opponentKnight.SetSide(!playerIsSide1);


        Time.timeScale = 1f;

        Transform playerStart = flipSides ? opponentStartPosition : playerStartPosition;
        Transform opponentStart = flipSides ? playerStartPosition : opponentStartPosition;

        playerKnight.startPoint = playerStart;
        opponentKnight.startPoint = opponentStart;

        inputReceived = false;
        roundTimer = 0f;
        roundActive = false;

        // Sets which timing bar to be used (Easy, Medium or Hard)
        ui.SetTimingBarDifficulty(currentOpponent);
        ui.UpdateMatchAndRound(currentOpponent, roundCounter);

        // Reset UI
        ui.HideResultText();

        // Reset knight positions
        playerKnight.ResetKnight();
        opponentKnight.ResetKnight();

        timingBar.ResetMarkerPosition();

        ui.ShowCountdown(countdownTime, BeginJoust);   // Countdown before joust
    }

    /// <summary>
    /// Called when countdown ends. Starts marker movement and round timer.
    /// </summary>
    private void BeginJoust()
    {
        roundActive = true;

        // Increase difficulty by speeding up the marker slightly each round
        timingBar.StartBar();
        timingBar.OnMarkerStopped += HandleMarkerResult;

        roundTimer = 0f;

        // Start both knights charging towad the center
        Transform playerTarget = flipSides ? side2CenterPoint : side1CenterPoint;
        Transform opponentTarget = flipSides ? side1CenterPoint : side2CenterPoint;

        playerKnight.StartCharge(playerTarget.position);
        opponentKnight.StartCharge(opponentTarget.position);
        playerKnight.SetCharging(true);
        opponentKnight.SetCharging(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!roundActive) return;

        roundTimer += Time.deltaTime;

        // If 5 seconds pass and no input, force a loss
        if (roundTimer >= contactTime && !inputReceived)
        {
            roundActive = false;
            timingBar.ForceStop();  // Force result evaluation (will ressult in a miss)
        }

        // Handle spacebar input (can be replaced with other input logic)
        if (Input.GetKeyDown(KeyCode.Space) && !inputReceived)
        {
            inputReceived = true;
            timingBar.StopAndEvaluate();
        }
    }
    /// <summary>
    /// Callback from TimingBarController when the marker is stopped.
    /// Evaluates result and determines outcome.
    /// </summary>
    private void HandleMarkerResult(TimingBarController.HitResult result)
    {
        roundActive = false;
        timingBar.OnMarkerStopped -= HandleMarkerResult; // Unsubscribe to avoid double-calling

        switch (result)
        {
            case TimingBarController.HitResult.SweetSpot:
                HandleRoundWin(true);
                break;

            case TimingBarController.HitResult.GreenZone:
                // 25% chance to dismount opponent
                bool dismounted = Random.Range(0f, 1f) <= 0.25f;
                if (dismounted)
                    HandleRoundWin(false);
                else
                    HandleRoundDraw();
                break;

            case TimingBarController.HitResult.Miss:
                HandleRoundLoss();
                break;
        }
    }

    /// <summary>
    /// Called when plaayer wins the round - either guranteed or by chance.
    /// </summary>
    private void HandleRoundWin(bool guaranteed)
    {
        int reward = guaranteed ? 150 : 100; // Sweet spot gets extra
        totalCrowns += reward;
        ui.UpdateCrownDisplay(totalCrowns);  // Ensure UI shows updated value

        ui.ShowResultText(guaranteed ?
            "Perfect hit! You dismounteed your opponent!" :
            "You held steady and dismounted your opponent!");

        currentOpponent++;
        roundCounter = 1;   // Reset round counter on new match

        // Player becomes champion if all opponents are defeated
        if (currentOpponent <= totalOpponents)
        {
            KnightSpriteSet chosenSet = opponentSprites[Random.Range(0, opponentSprites.Length)];
            opponentKnight.idleSide1 = chosenSet.idleSide1;
            opponentKnight.chargeSide1 = chosenSet.chargeSide1;
            opponentKnight.idleSide2 = chosenSet.idleSide2;
            opponentKnight.chargeSide2 = chosenSet.chargeSide2;

        }

        if (currentOpponent > totalOpponents)
        {
            HandleChampionshipWin();
        }
        else
        {
            flipSides = false;
            // Start next opponent after delay
            Invoke(nameof(StartRound), delayAfterRound); // Replay same opponent
        }
    }

    /// <summary>
    /// Called when player survived but didn't win - joust again.
    /// </summary>
    private void HandleRoundDraw()
    {
        ui.ShowResultText("You survived the joust. Prepare for another charge!");
        roundCounter++;
        Invoke(nameof(StartRound), delayAfterRound); // Replay same opponent
        flipSides = !flipSides;
    }

    /// <summary>
    /// Called when the player loses by missing the timing zone or timeout.
    /// </summary>
    private void HandleRoundLoss()
    {
        ui.ShowResultText("You were dismounted!");

        totalCrowns = championBonus;    // Only keep champion bonus
        ui.UpdateCrownDisplay(totalCrowns);

        currentOpponent = 1;
        roundCounter = 1;
        flipSides = false;              // Ensure player starts on correct side after restart

        Invoke(nameof(ShowLossPostGame), delayAfterRound); // Restart from easiest opponent
    }

    /// <summary>
    /// Called after final opponent is defeated. Adds champion reward and pauses game.
    /// </summary>
    private void HandleChampionshipWin()
    {
        championBonus = 500;
        totalCrowns += championBonus;
        ui.UpdateCrownDisplay(totalCrowns);

        ui.ShowResultText("You are the Champion!\nRelish in your spoils!");
        currentOpponent = 1;
        roundCounter++;
        flipSides = false;
        Invoke(nameof(ShowChampionshipPostGame), delayAfterChampionship);  // Restart game loop
    }

    /// <summary>
    /// Shows the loss postgame screen and pauses the game.
    /// </summary>
    private void ShowLossPostGame()
    {
        Time.timeScale = 0f;
        ui.ShowPostGameMenu("You were dismounted!");
    }

    /// <summary>
    /// Shows the championship win postgame screen and pauses the game.
    /// </summary>
    private void ShowChampionshipPostGame()
    {
        Time.timeScale = 0f;
        ui.ShowPostGameMenu("You are the Champion!\nRelish in your spoils!");
    }

    /// <summary>
    /// Fully resets game state after restart from postgame menu.
    /// </summary>
    private void RestartGame()
    {
        totalCrowns = 0;
        championBonus = 0;
        currentOpponent = 1;
        roundCounter = 1;
        flipSides = false;

        ui.HidePostGameMenu();
        StartRound();
    }

    /// <summary>
    /// Loads the central main menu scene for the minigame collection.
    /// </summary>
    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
