using UnityEngine;

public class MovementController : MonoBehaviour{

    public float speed = 10.0f;
    public float sprintModifier = 3.0f;
    public float jumpForce = 400f;

    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);     // limits diagonal movement to the same speed as movement along an axis
        movement *= Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift)) movement *= sprintModifier;
        
        transform.Translate(movement);

        if (Input.GetKeyDown(KeyCode.Space)) GetComponent<Rigidbody>().AddForce(0, jumpForce, 0);
        if (Input.GetKeyDown(KeyCode.F)) GetComponent<Rigidbody>().AddForce(0, -jumpForce, 0);
        


    }
}
