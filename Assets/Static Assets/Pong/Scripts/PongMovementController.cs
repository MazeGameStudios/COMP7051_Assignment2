using UnityEngine;

/** 
  * @desc This class contains the methods to control player movement
  * examples Update()
  * @author Daniel Tian
  * @version September 25, 2017
  * @required none
*/
public class PongMovementController : MonoBehaviour {

    //keycodes that can be set differently for different players.
    public KeyCode moveUp,
        moveDown;

    //a float representing the speed of the player
    public float speed = 20.0f;


    //Vector that tells the player how much to move each update
    Vector2 move;

    public enum DeviceInputState
    {
        MouseKeyboard,
        Controller
    };
    private DeviceInputState m_State = DeviceInputState.MouseKeyboard;


    /**
      * @desc - This method gets called just before any of the Update methods is called
    */
    void Start()
    {
        move = new Vector2(0, speed);

        switch (m_State)
        {  
            case DeviceInputState.MouseKeyboard:

                if (IsControllerInput())
                {
                    //called if a controller is detected
                    m_State = DeviceInputState.Controller;
                }
                break;
            case DeviceInputState.Controller:
                if (IsMouseKeyboard())
                {
                    m_State = DeviceInputState.MouseKeyboard;
                }
                break;
        }

    }

    /**
      * @desc - Unity gameloop, updates are dependent on computer speed
    */
    void Update () {

        if(m_State == DeviceInputState.MouseKeyboard)
        {
            if (Input.GetKey(moveUp)) //moves up
            {
                move.y = speed;

            }
            else if (Input.GetKey(moveDown)) //move down
            {
                move.y = speed * -1f;
            }
            else
            {
                move.y = 0;
            } 
        }else if (m_State == DeviceInputState.Controller)
        {
            if (Input.GetKey(KeyCode.Joystick1Button1)) //moves up - X on the ps4, for joystick 1
            {
                move.y = speed;

            }
            else if (Input.GetKey(KeyCode.Joystick1Button3)) //move down - triangle, for joystick 1
            {
                move.y = speed * -1f;
            }
            else
            {
                move.y = 0;
            }
        }

        GetComponent<Rigidbody2D>().velocity = move;
    }
    

    /**
      * @desc detects if a mouse and keyboard are being used
      * @return - success or failure
    */
    private bool IsMouseKeyboard()
    {
        // mouse & keyboard buttons
        if (Event.current.isKey ||
            Event.current.isMouse)
        {
            return true;
        }
        // mouse movement
        if (Input.GetAxis("Mouse X") != 0.0f ||
            Input.GetAxis("Mouse Y") != 0.0f)
        {
            return true;
        }
        return false;
    }

    /**
      * @desc detects if a controller is being used
      * @return - success or failure
    */
    private bool IsControllerInput()
    {
        //Reference: https://www.reddit.com/r/Unity3D/comments/1syswe/ps4_controller_map_for_unity/

        // joystick buttons
        if (Input.GetKey(KeyCode.Joystick1Button0) ||
           Input.GetKey(KeyCode.Joystick1Button1) ||
           Input.GetKey(KeyCode.Joystick1Button2) ||
           Input.GetKey(KeyCode.Joystick1Button3) ||
           Input.GetKey(KeyCode.Joystick1Button4) ||
           Input.GetKey(KeyCode.Joystick1Button5) ||
           Input.GetKey(KeyCode.Joystick1Button6) ||
           Input.GetKey(KeyCode.Joystick1Button7) ||
           Input.GetKey(KeyCode.Joystick1Button8) ||
           Input.GetKey(KeyCode.Joystick1Button9) ||
           Input.GetKey(KeyCode.Joystick1Button10) ||
           Input.GetKey(KeyCode.Joystick1Button11) ||
           Input.GetKey(KeyCode.Joystick1Button12) ||
           Input.GetKey(KeyCode.Joystick1Button13) ||
           Input.GetKey(KeyCode.Joystick1Button14) ||
           Input.GetKey(KeyCode.Joystick1Button15) ||
           Input.GetKey(KeyCode.Joystick1Button16) ||
           Input.GetKey(KeyCode.Joystick1Button17) ||
           Input.GetKey(KeyCode.Joystick1Button18) ||
           Input.GetKey(KeyCode.Joystick1Button19))
        {
            return true;
        }


        return false;
    }




}
