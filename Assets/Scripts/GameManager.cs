using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ballObject;
    [SerializeField]
    private GameObject tileObject;
    [SerializeField]
    private GameObject parentObject;
    [SerializeField]
    private GameObject gameoverMenu;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject youWinMenu;
    [SerializeField]
    private GameObject bossAI;

    [SerializeField]
    private AudioClip decrementLifeSound;

    [SerializeField]
    private AudioClip youWinSound;

    [SerializeField]
    private AudioClip youLoseSound;

    [SerializeField]
    private Transform originPosition;

    [SerializeField]
    private Transform bossAISpawnPosition;

    [SerializeField]
    private GameObject[] lifeUI;

    [HideInInspector]
    public List<int> activePowerUps = new List<int>();

    [HideInInspector]
    public static int playerLives = 3;

    [SerializeField]
    private Text scoreUI;

    [SerializeField]
    private Text highScoreUI;

    [HideInInspector]
    public int gameScore = 0;

    int highScore = 0;

    [HideInInspector]
    public List<GameObject> tileList;

    private const float WIN_DELAY_TIMER = 2.0f;

    private const float SPAWN_Y = 0.5f;
    private const float Y_DEPSAWN = -10.5f;

    private const int SCORE_GAINED = 20;
    private const int PLAYER_DEATH_INTEGER = 0;
    private const int MAIN_MENU_SCENE_INTEGER = 0;
    private const int POWER_UP_LIST = 1;

    private const string PLAYER_PREFS_STRING = "highScore";
    private const string SCORE_TEXT = "Score: ";
    private const string HIGHSCORE_TEXT = "High Score: ";

    private bool hasSpawnedBossAI = false;

    public bool isPaused = false;

    public void IncrementPowerUpList()
    {
        activePowerUps.Add(POWER_UP_LIST);
    }

    public void DecrementPowerUpList()
    {
        activePowerUps.Remove(POWER_UP_LIST);
    }

    public float GetDespawnCoordinate()
    {
        return Y_DEPSAWN;
    }

    public Transform GetOriginCoordinate()
    {
        return originPosition;
    }

    [Header("Grid")]

    [SerializeField]
    private float xMinimum = -7.0f;
    [SerializeField]
    private float xMaximum = 6.0f;
    [SerializeField]
    private float yMinimum = -5.0f;
    [SerializeField]
    private float yMaximum = 1.0f;
    [SerializeField]
    private float gridxInterval = 1.5f;
    [SerializeField]
    private float gridyInterval = 1.0f;

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        highScore = PlayerPrefs.GetInt(PLAYER_PREFS_STRING, 0);

        Time.timeScale = 1.0f;

        gameoverMenu.SetActive(false);
        pauseMenu.SetActive(false);

        scoreUI.text = SCORE_TEXT + gameScore;
        highScoreUI.text = HIGHSCORE_TEXT + highScore;

        for (int I = 0; I < lifeUI.Length; I++)
        {
            lifeUI[I].SetActive(I < playerLives);
        }

        // load up grid

        LoadUpGrid(xMinimum, xMaximum, gridxInterval, yMinimum, yMaximum, gridyInterval);

    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Pause(pauseMenu);
        }

    }

    public void RespawnBall()
    {
        Instantiate(ballObject, originPosition.transform.position, Quaternion.identity);
        DecrementLives();
    }

    public void DecrementLives()
    {
        playerLives--;
        AudioManager.Instance.PlaySound(decrementLifeSound);

        for (int I = 0; I < lifeUI.Length; I++)
        {
            lifeUI[I].SetActive(I < playerLives);
        }

        if (playerLives <= PLAYER_DEATH_INTEGER)
        {
            Pause(gameoverMenu);
            AudioManager.Instance.PlaySound(youLoseSound);
        }
    }

    public void InstantDeath()
    {
        playerLives = PLAYER_DEATH_INTEGER;

        for (int I = 0; I < lifeUI.Length; I++)
        {
            lifeUI[I].SetActive(I < playerLives);
        }

        if (playerLives <= PLAYER_DEATH_INTEGER)
        {
            Pause(gameoverMenu);
            AudioManager.Instance.PlaySound(youLoseSound);
        }
    }


    public void UpdateScore(int Amount = SCORE_GAINED)
    {
        gameScore += Amount;

        if (gameScore > highScore)
        {
            PlayerPrefs.SetInt(PLAYER_PREFS_STRING, highScore = gameScore);
            highScoreUI.text = HIGHSCORE_TEXT + highScore;
        }

        scoreUI.text = SCORE_TEXT + gameScore;
    }

    public void Pause(GameObject UI_Instance)
    {
        UI_Instance.SetActive(true);
        Time.timeScale = 0.0f;

        isPaused = true;
    }

    public void ReturnToMenu()
    {
        // clear all singletons relevent 

        DestroyGameManager();
        PersistThroughLevels.Instance.DestroyInstance();
        LevelManager.Instance.DestroyInstance();
        Player.Instance.DestroyPlayer();
        ObjectPool.Instance.DestroyInstance();
        SceneManager.LoadScene(MAIN_MENU_SCENE_INTEGER);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;

        isPaused = false;
    }

    public void DestroyGameManager() 
    {
        Destroy(gameObject); 
        instance = null; 
    }

    public void YouWin()
    {
        StartCoroutine(YouWinDelay());
    }

    public IEnumerator YouWinDelay()
    {
        yield return new WaitForSeconds(WIN_DELAY_TIMER);
        AudioManager.Instance.PlaySound(youWinSound);
        Pause(youWinMenu);
    }

    public void LoadUpGrid(float XMinimum, float XMaximum, float GridXInterval, float YMinimum, float YMaximum, float GridYInterval)
    {
        // load up tile grid

        tileList.Clear();

        for (float I = XMinimum; I < XMaximum; I = I + GridXInterval)
        {
            for (float J = YMinimum; J < YMaximum; J = J + GridYInterval)
            {
                GameObject SpawnedTile = Instantiate(tileObject, new Vector3(I, SPAWN_Y, J), Quaternion.identity, parentObject.transform);

                tileList.Add(SpawnedTile);
            }
        }

    }

    public void SpawnBoss()
    {
        if (!hasSpawnedBossAI)
        {
            Instantiate(bossAI, bossAISpawnPosition.transform.position, Quaternion.identity);
            hasSpawnedBossAI = true;
        }
    }
}
