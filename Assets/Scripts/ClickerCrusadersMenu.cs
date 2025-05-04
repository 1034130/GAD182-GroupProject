using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickerCrusadersMenu : MonoBehaviour
{
    public Button joustingChampionshipButton;
    public Button miningButton;
    public Button blacksmithingButton;
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        joustingChampionshipButton.onClick.AddListener(() => LoadGame("MainMenu"));
        miningButton.onClick.AddListener(() => LoadGame("MiningGameMenu"));
        blacksmithingButton.onClick.AddListener(() => LoadGame("Blacksmithing"));
        exitButton.onClick.AddListener(QuitGame);
        
    }

    void LoadGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
