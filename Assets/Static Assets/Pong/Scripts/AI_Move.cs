using UnityEngine;

/** 
  * @desc This class holds the functions that define ai movement
  * examples MoveTowardsY()
  * @author Daniel Tian
  * @version September 25, 2017
  * @required none
*/
public class AI_Move : MonoBehaviour {

    //The speed at which the computer moves. Increasing this value will make the computer harder to beat.
    public float AIMoveSpeed = 6.5f;

    //A variable that references the ball when the game starts
    private GameObject ball;

    //A vector containing the position of a ball's previous position 
    private Vector3 previousLocation = Vector3.zero;

    //A bool that is true when the ball is moving up
    private bool isBallMovingUp;


    /**
      * @desc - Unity gameloop, updates are dependent on computer speed
    */
    private void Update () {

        ball = GameObject.FindWithTag("Ball");

        if (ball != null)
        {
            Vector3 curVel = (ball.transform.position - previousLocation) / Time.deltaTime;
            if (curVel.y > 0) // it's moving up
            {
                isBallMovingUp = true;
            }
            else // it's moving dow
            {
                isBallMovingUp = false;
            }

            MoveTowardsY(ball.transform.position);
            previousLocation = ball.transform.position;
        }          
       
	}


    /**
      * @desc Moves the ai only on the Y axis
      * @param Vector3 destination - the destination that the ai will move towards
      * @return void
    */
    private void MoveTowardsY(Vector3 destination)
    {
        //calculates the horizontal distance to the ball
        float distanceToBall = Mathf.Abs(ball.transform.position.x - transform.position.x); 
        Vector2 MovePos; //how much the ai will move in one method call

        if (distanceToBall < 1.75f) //called when the ball gets close enough to the ai, in order to to add some force to the ball
        {
            if (isBallMovingUp)
            {
                MovePos = new Vector2(transform.position.x, transform.position.y + 20 * Time.deltaTime);
            }
            else
            {
                MovePos = new Vector2(transform.position.x, transform.position.y + 20 * Time.deltaTime * -1);
            }
        }
        else
        {
            float Direction = Mathf.Sign(destination.y - transform.position.y);

            MovePos = new Vector2(
                transform.position.x, //MoveTowards on 1 axis
                transform.position.y + Direction * AIMoveSpeed * Time.deltaTime
            );
            
        }

        transform.position = MovePos; //sets the transform to the vector MovePos
    }
}
