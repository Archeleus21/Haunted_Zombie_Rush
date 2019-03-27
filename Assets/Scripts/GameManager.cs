using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager instance = null;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject coinCounter;
    [SerializeField] private Text coinCounterText;
    [SerializeField] private Text[] highScores;

    //-----------------------------------------------------------
    //encapsulated data
    //-------------------------------------------------------------
    private bool isPlayerActive = false; //check if player is playing
    private bool isGameOver = false;  //checks if game is over
    private bool isGameStarted = false;  //checks if game started
    private bool isReplaying = false; //checks if the game has been replayed
    private bool collectedCoin = false; //checks if coin was collected
    private int playerScore = 0;  //stores player score

    private int[] highScore;


    //----------------------------------------------------------
    //getters
    //----------------------------------------------------------
    public bool PlayerActive
    {
        get
        {
            return isPlayerActive;
        }
    }

    public bool GameOver
    {
        get
        {
            return isGameOver;
        }
    }

    public bool GameStarted
    {
        get
        {
            return isGameStarted;
        }
    }

    public bool Replaying
    {
        get
        {
            return isReplaying;
        }
    }

    public bool CollectedCoin
    {
        get
        {
            return collectedCoin;
        }
    }

    public int PlayerScore
    {
        get
        {
            return playerScore;
        }
    }

    //-----------------------------------------------------------------
    //singleton
    //------------------------------------------------------------------
    private void Awake()
    {
        //checks if there are empty slots in the inspector
        Assert.IsNotNull(mainMenu);
        Assert.IsNotNull(gameOverScreen);
        Assert.IsNotNull(coinCounter);

        if(instance == null)  //no instance of this class exists
        {
            instance = this;  //makes this the new instance
        }
        else if (instance != this)  //other instance is not this
        {
            Destroy(gameObject);  //destroy that game object
        }

        DontDestroyOnLoad(gameObject);  //dont destroy this gameobject when new scenes are loaded

        
    }

    private void Start()
    {
        gameOverScreen.SetActive(false);
    }

    //----------------------------------------------------------
    //setters
    //----------------------------------------------------------
    public void ReturnToMainMenu()
    {
        mainMenu.SetActive(true);
        gameOverScreen.SetActive(false);
        coinCounter.SetActive(false);
        isGameStarted = false;
        isPlayerActive = false;
        isGameOver = true;
    }

    public void PlayerCollided()
    {
        gameOverScreen.SetActive(true);
        coinCounter.SetActive(false);
        isPlayerActive = false;
        isGameOver = true;
        isGameStarted = false;
        StoreHighScore();
        DisplayHighScores();
    }

    public void PlayerStartedGame()
    {
        gameOverScreen.SetActive(false);
        isPlayerActive = true;
        isReplaying = false;

    }

    public void EnterGame()
    {
        SpawnPlayer();
        mainMenu.SetActive(false);
        coinCounter.SetActive(true);
        isGameStarted = true;
        isGameOver = false;
        isReplaying = true;
    }

    public void ReplayGame()
    {
        isReplaying = true;
        SpawnPlayer();
        gameOverScreen.SetActive(false);
        coinCounter.SetActive(true);
        isGameOver = false;
        isGameStarted = true;
        isPlayerActive = false;
        playerScore = 0;
    }

    public void CollidedWithCoin()
    {
        collectedCoin = true;
    }

    public void AddPlayerScore()
    {
        playerScore += 1;

        if(collectedCoin)
        {
            collectedCoin = false;
        }
    }

    //-----------------------------------------------------------
    //respawn new player
    //-----------------------------------------------------------
    void SpawnPlayer()
    {
        Instantiate(playerPrefab, new Vector3(playerPrefab.transform.position.x, 2f, playerPrefab.transform.position.z), playerPrefab.transform.rotation);
    }

    //--------------------------------------------------------
    //display scoure and update score
    //--------------------------------------------------------
    void DisplayScore()
    {
        coinCounterText.text = playerScore.ToString();
    }

    void DisplayHighScores()
    {     
        for (int i = 0; i < highScore.Length; i++)
        {
            PlayerPrefs.GetInt("High Score" + i, highScore[i]);
            highScores[i].text = highScore[i].ToString();
        }
     
    }

    private void Update()
    {
        DisplayScore();
    }

    //------------------------------------------------------
    //store high scores
    //------------------------------------------------------
    private void StoreHighScore()
    {
        //checks if file is empty and fills with defalut values
        if (PlayerPrefs.HasKey("High Score" + 0) == false)
        {
            highScore = new int[] { 0, 0, 0 } ;
        }
        else //if file exists creates empty array and fills with data
        {
            highScore = new int[3];

            for(int i =  0; i < 3; i++)
            {
                highScore[i] = PlayerPrefs.GetInt("High Score" + i);
            }
        }

        //swaps data with updated scores
        if (playerScore > highScore[0])
        {
            highScore[2] = highScore[1];
            highScore[1] = highScore[0];
            highScore[0] = playerScore;
        }
        else if (playerScore < highScore[0] && playerScore > highScore[1])
        {
            highScore[2] = highScore[1];
            highScore[1] = playerScore;
        }
        else if (playerScore < highScore[0] &&
                playerScore < highScore[1] &&
                playerScore > highScore[2])
        {
            highScore[2] = playerScore;
        }
        
        //stores new scores in file
        for(int i = 0; i < highScore.Length; i++)
        {
            PlayerPrefs.SetInt("High Score" + i, highScore[i]);
        }
    }
}
