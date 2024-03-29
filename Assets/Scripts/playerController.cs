using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public float speed = 5.0f; // Adjust this value to control the speed
    public static int myScore;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player based on the input direction and speed.
        transform.Translate(Dir() * speed * Time.deltaTime);
    }

   
    public Vector3 Dir()
    {
        // Gets input values for vertical (up/down) and horizontal (left/right) axes.
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        // Creates a vector from the input values, representing the desired movement direction.
        Vector3 myDir = new Vector3(x, y, 0);

        // Normalizes the vector to ensure consistent speed in all directions.
        return myDir.normalized;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
       
      if(collision.gameObject.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            myScore++;
            Debug.Log("Score: " + myScore);
        }
    }
}
