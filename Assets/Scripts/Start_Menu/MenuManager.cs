using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private const int EASY_LIVES = 5;
    private const int MEDIUM_LIVES = 4;
    private const int HARD_LIVES = 3;

    private const int EASY_PROBABILITY = 10;
    private const int MEDIUM_PROBABILITY = 15;
    private const int HARD_PROBABILITY = 20;

    private const string EASY_TEXT = "Easy";
    private const string MEDIUM_TEXT= "Medium";
    private const string HARD_TEXT = "Hard";

    [SerializeField]
    private GameObject startScreen;

    [SerializeField]
    private GameObject settingsScreen;

    [SerializeField]
    private GameObject encylopediaScreen;

    [SerializeField]
    private Text difficultyText;

    private static int setLives = EASY_LIVES;
    private static int setProbability = EASY_PROBABILITY;
    private static string setString = EASY_TEXT;

    private void Start()
    {
        ManageMenuObjects(settingsScreen, false);
        ManageMenuObjects(encylopediaScreen, false);

        GameManager.playerLives = setLives;
        TileLogic.pickupProbability = setProbability;

        SetDifficultyText(setString);
    }

    public void EasyMode()
    {
        SetDifficultyText(EASY_TEXT);

        SetDifficulty(EASY_LIVES, EASY_PROBABILITY, EASY_LIVES, EASY_PROBABILITY, EASY_TEXT);

    }

    public void MediumMode()
    {
        SetDifficultyText(MEDIUM_TEXT);

        SetDifficulty(MEDIUM_LIVES, MEDIUM_PROBABILITY, MEDIUM_LIVES, MEDIUM_PROBABILITY, MEDIUM_TEXT);

    }

    public void HardMode()
    {
        SetDifficultyText(HARD_TEXT);

        SetDifficulty(HARD_LIVES, HARD_PROBABILITY, HARD_LIVES, HARD_PROBABILITY, HARD_TEXT);

    }

    // update difficulty depending on difficulty option selected by the user, set this also to default difficulty upon restart

    public void SetDifficulty(int Lives, int Probability, int SetLives, int SetProbability, string SetString)
    {
        GameManager.playerLives = Lives;
        TileLogic.pickupProbability = Probability;

        setLives = SetLives;
        setProbability = SetProbability;
        setString = SetString;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        ActivateOrDeactivateSettingsScreen(true);
    }

    public void Encyclopedia()
    {
        ActivateOrDeactivateEncyclopedia(true);
    }

    public void MainMenu()
    {
        ActivateOrDeactivateSettingsScreen(false);
        ActivateOrDeactivateEncyclopedia(false);
    }

    private void ActivateOrDeactivateEncyclopedia(bool ActivateOrDeactivate)
    {
        if (ActivateOrDeactivate)
        {
            ManageMenuObjects(settingsScreen, false);
            ManageMenuObjects(startScreen, false);
            ManageMenuObjects(encylopediaScreen, true);
        }
        else
        {
            ManageMenuObjects(encylopediaScreen, false);
        }
    }

    private void ActivateOrDeactivateSettingsScreen(bool ActivateOrDeactivate)
    {
        if (ActivateOrDeactivate)
        {
            ManageMenuObjects(settingsScreen, true);
            ManageMenuObjects(startScreen, false);
        }
        else
        {
            ManageMenuObjects(settingsScreen, false);
            ManageMenuObjects(startScreen, true);
        }
    }

    // activate or deactivate object instances

    private void ManageMenuObjects(GameObject Object, bool SetActiveOrInactive)
    {
        Object.SetActive(SetActiveOrInactive);
    }

    private void SetDifficultyText(string Difficulty)
    {
        difficultyText.text = "Difficulty: " + Difficulty;
    }

}
