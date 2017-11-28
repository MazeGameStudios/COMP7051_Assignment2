using UnityEngine;

/** 
  * @desc This class sets up the boundaries of the game
  * examples Start()
  * @author Daniel Tian
  * @version September 25, 2017
  * @required none
*/
public class PongSetup : MonoBehaviour {

    //reference to the main camera
    public Camera mainCamera;

    //reference to the 4 boundaries that contain the game via box colliders
    public BoxCollider2D northBoundary, eastBoundary, southBoundary, westBoundary;

    //reference to the transforms of players 1 and 2
    public Transform player1, player2;

    /**
      * @desc - This method gets called just before any of the Update methods is called
    */
    void Start () {

        //move each boundary to the edge location of the screen
        northBoundary.size = new Vector2(mainCamera.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
        northBoundary.offset = new Vector2(0f, mainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y);

        southBoundary.size = new Vector2(mainCamera.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
        southBoundary.offset = new Vector2(0f, mainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y * -1f);

        westBoundary.size = new Vector2(1f, mainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y); //left
        westBoundary.offset = new Vector2(mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x, 0f);

        eastBoundary.size = new Vector2(1f, mainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y); //right wall
        eastBoundary.offset = new Vector2(mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x, 0f);

        //set up player positions
        player1.localPosition = new Vector2(mainCamera.ScreenToWorldPoint(new Vector3(125f, 0f, 0f)).x, player1.localPosition.y);
        player2.localPosition = new Vector2(mainCamera.ScreenToWorldPoint(new Vector3(Screen.width - 125f, 0f, 0f)).x, player2.localPosition.y); 
    }

}
