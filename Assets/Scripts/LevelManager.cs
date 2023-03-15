using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManager;

    private bool isInLevelOne = true;
    private bool isInLevelTwo = false;
    private bool isInLevelThree = false;
    private bool isInLevelFour = false;

    private const int LEVEL_TWO_INDEX = 2;
    private const int LEVEL_THREE_INDEX = 3;
    private const int LEVEL_FOUR_INDEX = 4;
    private const int LEVEL_FIVE_INDEX = 5;

    private const int ZERO_TILES = 0;

    private static LevelManager instance = null;

    private const float LEVEL_FIVE_X_MINIMUM = -6.8f;
    private const float LEVEL_FIVE_X_MAXIMUM = 6.8f;

    public static LevelManager Instance
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

    public void DestroyInstance()
    {
        Destroy(gameObject);
        instance = null;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

    }

    private void Update()
    {
        // load up the next level if the score for that level is equal to that levels maximum score

        if (gameManager.tileList.Count == ZERO_TILES)
        {
            if (isInLevelOne)
            {
                LoadLevelTwo();

                isInLevelOne = false;
                isInLevelTwo = true;
            }
            else if (isInLevelTwo)
            {
                LoadLevelThree();

                isInLevelTwo = false;
                isInLevelThree = true;
            }
            else if (isInLevelThree)
            {
                LoadLevelFour();

                isInLevelThree = false;
                isInLevelFour = true;
            }
            else if (isInLevelFour)
            {
                LoadLevelFive();

                isInLevelFour = false;
            }
        }
    }

    private void LoadLevelTwo()
    {
        LoadLevelAndReset(LEVEL_TWO_INDEX);

    }

    private void LoadLevelThree()
    {
        LoadLevelAndReset(LEVEL_THREE_INDEX);

    }

    private void LoadLevelFour()
    {
        LoadLevelAndReset(LEVEL_FOUR_INDEX);
    }

    private void LoadLevelFive()
    {
        LoadLevelAndReset(LEVEL_FIVE_INDEX);
    }

    private void LoadLevelAndReset(int LevelIndex)
    {
        gameManager.activePowerUps = new List<int>();
        Player.Instance.gameObject.transform.position = Player.Instance.originalPosition;

        Player.Instance.maximumX = LEVEL_FIVE_X_MAXIMUM;
        Player.Instance.minimumX = LEVEL_FIVE_X_MINIMUM;

        Player.Instance.gameObject.transform.localScale = Player.Instance.originalScale;

        SceneManager.LoadScene(LevelIndex);
    }

}
