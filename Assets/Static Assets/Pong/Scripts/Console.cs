using UnityEngine;
using UnityEngine.UI;

/** 
  * @desc This class defines the functions that the console uses
  * examples ProcessCommand()
  * @author Daniel Tian
  * @version September 25, 2017
  * @required none
*/
public class Console : MonoBehaviour
{
    //A reference to the input field of the console
    public InputField inputField;

    //A reference to the gameobjects of players 1 and 2
    public GameObject player1, player2, 
        backgroundImage; //reference to the background image of the game

    /**
      * @desc - This method gets called just before any of the Update methods is called
    */
    void Start()
    {
        inputField.gameObject.SetActive(false);
    }

    /**
      * @desc - Unity gameloop, updates are dependent on computer speed
    */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) //if c is tapped, brings up console
        {
            inputField.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //When esc key is tapped, closes console
        {
            inputField.gameObject.SetActive(false);
        }
    }

    /**
      * @desc checks what has been typed into the console box, and executes the appropriate 
      * command or none depending on the string within the inputfield
      * @return - void
    */
    public void ProcessCommand()
    {
        if(inputField.text == "Player1.Enlarge")
        {
            //Lengthens player1 by 0.1
            player1.transform.localScale += new Vector3(0, 0.1f, 0);
        }
        else if(inputField.text == "Player1.Shrink")
        {
            //Shrinks player1 by 0.1
            player1.transform.localScale -= new Vector3(0, 0.1f, 0);
        }
        else if (inputField.text == "Player2.Enlarge")
        {
            //Lengthens player2 by 0.1
            player2.transform.localScale += new Vector3(0, 0.1f, 0);
        }
        else if (inputField.text == "Player2.Shrink")
        {
            //Shrinks player2 by 0.1
            player2.transform.localScale -= new Vector3(0, 0.1f, 0);
        }else if (inputField.text == "Background.ChangeColor")
        {
            backgroundImage.gameObject.SetActive(!backgroundImage.gameObject.activeSelf);
        }else if (inputField.text == "exit")
        {
            inputField.gameObject.SetActive(false);
        }
    }
}
