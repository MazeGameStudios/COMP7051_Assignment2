using UnityEngine;

/** 
  * @desc This class defines the behaviour of the left and right walls
  * examples OnTriggerEnter2D()
  * @author Daniel Tian
  * @version September 25, 2017
  * @required none
*/
public class SideBounds : MonoBehaviour {

    //A variable that represents the audio source that the ball will play sound from
    AudioSource soundEffect;

    /**
      * @desc - This method gets called just before any of the Update methods is called
    */
    private void Start()
    {
        soundEffect = GetComponent<AudioSource>();
    }

    /**
      * @desc this is called when the ball hits the right or left "side walls"
      * @return - void
    */
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {

        if (hitInfo.name.Contains("ball")) //if the object making contact with the walls is actually a ball
        {
            string sideName = transform.name; //checks the name of the wall that has been hit (either left or right)
            soundEffect.pitch = Random.Range(0.8f, 1.0f); //randomizes the pitch
            soundEffect.Play(); //plays victory sound
            GameController.Score(sideName); //have the game controller change the score of the 

            //resets the ball position by sending a message telling the ball to call it's ResetBallPosition() method
            hitInfo.gameObject.SendMessage("ResetBallPosition"); }
    }
}
