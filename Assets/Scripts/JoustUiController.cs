using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all UI updates for the jousting game.
/// Controls countdowns, result messages, crowns display, and visual feedback.
/// </summary>
public class JoustUiController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI countdownText;   // Displays countdown at the start of each round
    public TextMeshProUGUI resultText;      // Dispalys round result ("You Won!" etc)
    public TextMeshProUGUI crownsEarnedText;// Dispalys total Royal Crowns

    [Header("Timing Bars")]
    public GameObject easyTimingBar;        // Easy difficulty timing bar
    public GameObject mediumTimingBar;      // Medium difficulty timing bar
    public GameObject hardTimingBar;        // Hard difficulty timing bar

    private Coroutine countdownCoroutine;






    ///<summary>
    ///Starts a countdown from a given number and calls a callback when finishesd.
    ///</summary>
    public void ShowCountdown(float countdownTime, System.Action onCountdownComplete)
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        countdownCoroutine = StartCoroutine(CountdownRoutine(countdownTime, onCountdownComplete));
    }

    ///<summary>
    ///Coroutine to handlee countdown visual updates.
    ///</summary>
    private IEnumerator CountdownRoutine(float countdownTime, System.Action onCountdownComplete)
    {
        countdownText.gameObject.SetActive(true);
        float remainingTime = countdownTime;

        while (remainingTime > 0)
        {
            countdownText.text = Mathf.CeilToInt(remainingTime).ToString();
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        countdownText.text = "JOUST!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
        onCountdownComplete?.Invoke();
    }

    ///<summary>
    ///Show a result message (win, loss, survive) after a joust round.
    ///</summary>
    public void ShowResultText(string message)
    {
        resultText.text = message;
        resultText.gameObject.SetActive(true);
    }

    ///<summary>
    ///Hides the result text (called before new round starts).
    ///</summary>
    public void HideResultText()
    {
        resultText.gameObject.SetActive(false);
    }

    ///<summary>
    ///Updates the crowns earned UI text with the latest amount.
    ///</summary>
    public void UpdateCrownsEarned(int crowns)
    {
        crownsEarnedText.text = $"Crowns: {crowns}";

    }

    ///<summary>
    ///Activates the correct timing bar based on difficulty.
    ///</summary>
    public void SetTimingBarDifficulty(int opponentRank)
    {
        if (opponentRank <= 2)
        {
            easyTimingBar.SetActive(true);
            mediumTimingBar.SetActive(false);
            hardTimingBar.SetActive(false);
            FindObjectOfType<JoustGameManager>().timingBar = easyTimingBar.GetComponent<TimingBarController>();
        }
        else if (opponentRank <= 4)
        {
            easyTimingBar.SetActive(false);
            mediumTimingBar.SetActive(true);
            hardTimingBar.SetActive(false);
            FindObjectOfType<JoustGameManager>().timingBar = mediumTimingBar.GetComponent<TimingBarController>();
        }
        else
        {
            easyTimingBar.SetActive(false);
            mediumTimingBar.SetActive(false);
            hardTimingBar.SetActive(true);
            FindObjectOfType<JoustGameManager>().timingBar = hardTimingBar.GetComponent<TimingBarController>();
        }
    }
}

