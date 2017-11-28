using UnityEngine;
using System.Collections;

/** 
  * @desc This class contains the functions that manipulate ball movement
  * examples SpawnBall(), ResetBallPosition()
  * @author Daniel Tian
  * @version September 25, 2017
  * @required none
*/
public class BallController : MonoBehaviour
{
    //A variable representing the speed of the ball
    public float ballSpeed = 100f;

    //A variable that represents the audio source that the ball will play sound from
    AudioSource soundEffect;


    /**
      * @desc - This method gets called just before any of the Update methods is called
    */
    private void Start()
    {
        StartCoroutine(SpawnBall());
        soundEffect = GetComponent<AudioSource>();
    }

    /**
      * @desc spawns a ball after waiting 1 second
      * @return - The yield return value specifies when the coroutine is resumed
    */
    private IEnumerator SpawnBall()
    {
        yield return new WaitForSeconds(1f);

        float randValue = Random.Range(0.0f, 1.0f); //generates a random value between 0 and 1, to randomize the direction of the ball spawn
        if (randValue <= 0.5f)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(ballSpeed, 10)); //shoot right
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-ballSpeed, -10)); //left
        }
    }

    /**
      * @desc resets the ball position after a player scores
      * @return - The yield return value specifies when the coroutine is resumed
    */
    public IEnumerator ResetBallPosition()
    {
        if (!GameController.isGameActive)
            Destroy(gameObject);

        GetComponent<Rigidbody2D>().velocity = new Vector2(); //resets the ball's velocity
        transform.position = new Vector3(0 , 0); //resets the ball's location

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SpawnBall());
    }

    /**
     * @desc triggered when the ball collides with something
     * @return - void
    */
    private void OnCollisionEnter2D(Collision2D collisionDetails)
    {
        if (collisionDetails.collider.tag == "Player")
        {
            Vector2 newPath = GetComponent<Rigidbody2D>().velocity;
            newPath.y = GetComponent<Rigidbody2D>().velocity.y/2f + collisionDetails.collider.attachedRigidbody.velocity.y/3f;
            GetComponent<Rigidbody2D>().velocity = newPath;
            soundEffect.pitch = Random.Range(0.8f, 1.2f); //randomizes the pitch
            soundEffect.Play();
        }
    }


}
