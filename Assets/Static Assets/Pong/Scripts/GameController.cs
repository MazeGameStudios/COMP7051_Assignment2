using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/** 
  * @desc This class contains the methods to control the state of the game, such as switching
  * between lobby and different game modes, and calculating the score to determine if a player has won or not.
  * examples SetLobbyUI(bool), StartPlayerVsPlayer(), StartPlayerVsAI(), QuitGame(), InstantiateBall(),
  * IsGameWon(), GameWon()
  * @author Daniel Tian
  * @version September 25, 2017
  * @required none
*/
public class GameController : MonoBehaviour
{
    #region Game state data 

    //*********************//
    // Public member data  //
    //*********************//

    //variables that contain the scores of both players (or the ai, which in this case is player 2)
    public static int playerScore1 = 0,
        playerScore2 = 0;

    //bool indicating whether the game is in session or not
    public static bool isGameActive = false;

    //an integer which dictates how many points are required in order to win the game
    public int winScore = 3;

    //reference too the main camera
    public Camera mainCamera;

    //a reference to the gui skin
    public GUISkin skin;

    //Reference to the ball prefab, players 1 and 2
    public GameObject ball, player1, player2;

    //A reference to the buttons so that we can control the ui within our code
    public Button btnHuman, btnComputer, btnQuit;

    //text indicating the status of the game
    public Text statusText;

    //*********************//
    // Private member data //
    //*********************//

    //player2 can also subsitute for the ai. However, the movement controls must be disabled if playing vs ai
    private PongMovementController player2MovementController; 

    //A reference to the ai movement controller script
    private AI_Move player2AIController;

    //A reference to the ball currently in play
    private GameObject currentBall;

    #endregion  

    /**
      * @desc - This method gets called just before any of the Update methods is called
    */
    private void Start()
    {
        SetLobbyUI(true); //is in lobby
        player2MovementController = player2.GetComponent<PongMovementController>();
        player2AIController = player2.GetComponent<AI_Move>();
        player1.SetActive(false);
        player2.SetActive(false);
        statusText.text = "5 points to win";
    }

    /**
      * @desc - Unity gameloop, updates are dependent on computer speed
    */
    private void Update()
    {
        IsGameWon();

        if (Input.GetKeyDown(KeyCode.Space) && !isGameActive)
        {
            StartPlayerVsAI();
        }
    }

    #region Game state management

    /**
      * @desc changes the game state from lobby to an active game, in player versus computer mode
      * @return - void
    */
    private void SetLobbyUI(bool isVisible)
    {
        btnHuman.gameObject.SetActive(isVisible);
        btnComputer.gameObject.SetActive(isVisible);
        btnQuit.gameObject.SetActive(!isVisible);
    }

    /**
      * @desc changes the game state to the lobby, after resetting the game
      * @return - void
    */
    private void QuitGame()
    {
        Destroy(currentBall);
        SetLobbyUI(true); //back to lobby
        playerScore1 = 0;
        playerScore2 = 0;
        player1.SetActive(false);
        player2.SetActive(false);
        isGameActive = false;
        player2MovementController.enabled = true;
        statusText.text = "";
    }

    /**
      * @desc changes the game ui from lobby to an active game, and sets the players to the active state
      * @return - void
    */
    public void StartGame()
    {
    	statusText.text = "";
        SetLobbyUI(false);
        isGameActive = true;
        player1.SetActive(true);
        player2.SetActive(true);
    }

    /**
      * @desc changes the game state from lobby to an active game, in player versus player mode
      * @return - void
    */
    public void StartPlayerVsPlayer()
    {
        StartGame();
        player2AIController.enabled = false;
        InstantiateBall();
    }

    /**
      * @desc changes the game state from lobby to an active game, in player versus computer mode
      * @return - void
    */
    public void StartPlayerVsAI()
    {
        StartGame();
        player2MovementController.enabled = false;
        player2AIController.enabled = true;
        InstantiateBall();
    }

    /**
     * @desc called when the ball collides with the left or right wall, and updates the player's scores appropriately
     * @param string wallName - the name of the wall that the ball has collided with
     * @return - void
   */
    public static void Score(string wallName)
    {
        if (wallName == "rightCollider")
        {
            playerScore1++;
        }
        else
        {
            playerScore2++;
        }

    }

    /**
      * @desc Checks if a game has been won by comparing the scores of 2 players
      * @return - void
    */
    public void IsGameWon()
    {
        if (playerScore1 >= winScore)
        {
            StartCoroutine(GameWon(1));
        }
        else if (playerScore2 >= winScore)
        {
            StartCoroutine(GameWon(2));
        }
    }


    /**
      * @desc executes the game over sequence: displays who won in text, and then quits game
      * therefore returning to the lobby
      * @return - The yield return value specifies when the coroutine is resumed
    */
    IEnumerator GameWon(int playerNum)
    {

        if (playerScore2 > playerScore1 && player2AIController.enabled)
        {
            statusText.text = "computer wins!";
        }
        else
        {
            statusText.text = "Player " + playerNum + " has won!";
        }
        yield return new WaitForSeconds(3.0f);
        statusText.text = "";
        QuitGame();
    }

    #endregion

    /**
      * @desc updates the gui multiple times per second
      * @return - void
    */
    private void OnGUI()
    {
        if (isGameActive)
        {
            GUI.skin = skin;
            GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "P1: " + playerScore1);
            GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "P2: " + playerScore2);
        }
    }

    /**
      * @desc spawns the ball at the middle of the game space
      * @return - void
    */
    private void InstantiateBall() {
        currentBall = Instantiate(ball);
        ball.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

}
