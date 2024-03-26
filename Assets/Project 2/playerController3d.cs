using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class playerController3D : MonoBehaviour
{
    [Header("Base Vars")]
    public float speed = 10f;
    public float lookSpeed = 100f;

    [Header("Jump Vars")]
    public float jumpForce = 50f;
    public bool canJump;
    public bool jumped;

    public float jumpTimer = 50f; // Timer for tracking jump duration
    public float maxJumpTime = 50.0f; // Maximum duration for which the player can jump


    [Header("Kick Vars")]
    public Transform myFoot;
    public float kickForce = 50f;
    public float upForce = 10f;
    public float legLength = 5f;


    Rigidbody myRB;
    public Camera myCam;
    public float camLock; //maxlook up/down

    Vector3 myLook;
    float onStartTimer;

   

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        myLook = transform.localEulerAngles;
    }


    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        canJump = true;
        jumped = false;
        onStartTimer = 0f;
        //get the current mouse position
        //zero out our rotations based off that value

    }
    // Update is called once per frame
    void Update()
    {
        onStartTimer += Time.deltaTime;
        //camera forward direction
        myLook += DeltaLook() * Time.deltaTime;
        Debug.DrawRay(transform.position, myCam.transform.forward * 3f, Color.green);

        //clamp the magnitude to keep the player from looking fully upside down
        myLook.y = Mathf.Clamp(myLook.y, -camLock, camLock);

        //Debug.Log("myLook: " + myLook);
        transform.rotation = Quaternion.Euler(0f, myLook.x, 0f);
        myCam.transform.rotation = Quaternion.Euler(-myLook.y, myLook.x, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            jumped = true;
            jumpTimer = 0f; // Reset jump timer when space bar is pressed
        }
        else if (Input.GetKey(KeyCode.Space) && jumped && jumpTimer < maxJumpTime)
        {
            Jump();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            jumped = false;
        }

        if (Input.GetKey(KeyCode.Return))
        {
            Kick();
        }
    }

    void FixedUpdate()
    {
        Vector3 pMove = transform.TransformDirection(Dir());
        myRB.AddForce(pMove * speed * Time.fixedDeltaTime);

        //player raw input - in magenta
        //Debug.DrawRay(transform.position, pMove * 5f, Color.magenta);
        //Debug.DrawRay(transform.position, Vector3.up, Color.magenta);

        //combined velocity of the rigidbody in black
        //Debug.DrawRay(transform.position + Vector3.up, myRB.velocity.normalized*5f, Color.black);

        if (jumped && canJump)
        {
            jumpTimer += Time.fixedDeltaTime; // Increment jump timer
        }
    }

    Vector3 Dir()
    {
        //reference Unity Input Manager virtual axes here
        //horizontal and vertical for WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 myDir = new Vector3(x, 0, z);

        //remove console clutter by only logging direction when input is pressed
        if (myDir != Vector3.zero)
        {
            //Debug.Log("Player Move Dir: " + myDir);
        }

        return myDir;
    }

    Vector3 DeltaLook()
    {
        Vector3 dLook;
        float rotY = Input.GetAxisRaw("Mouse Y") * lookSpeed;
        float rotX = Input.GetAxisRaw("Mouse X") * lookSpeed;
        dLook = new Vector3(rotX, rotY, 0);

        if (dLook != Vector3.zero)
        {
            //Debug.Log("delta look: " + dLook);
        }

        if(onStartTimer < 1f)
        {
            dLook = Vector3.ClampMagnitude(dLook, onStartTimer * 10f);
        }

        return dLook;  
    }

    //add a jumpForce and flip boolean for jump request (jumped) to false
    void Jump()
    {
        myRB.AddForce(Vector3.up * jumpForce * Time.fixedDeltaTime);
    }


    void Kick()
    {
        RaycastHit hit;
        bool rayCast = false; ;
        //bool rayCast = Physics.Raycast(myFoot.position, myCam.transform.forward, out hit, 5f);
        if (Physics.SphereCast(myFoot.position, 1f, myCam.transform.position, out hit, legLength)) { rayCast = true; }
        Debug.DrawRay(myFoot.position, myCam.transform.forward * legLength, Color.blue);
        Debug.Log("raycast: " + hit);

        if(rayCast)
        {
            hit.rigidbody.AddExplosionForce(kickForce,hit.point,legLength,upForce);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Platform"))
        {
            Debug.Log("OnCollisionStay method called.");
            canJump = true;
        }


        if (collision.gameObject.tag == "Platform")
        {
            Debug.Log("hit platform");
            StartCoroutine(killPlaform(2f, collision.gameObject));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain") { canJump = false; }
    }

    IEnumerator killPlaform(float time, GameObject target)
    {
        Debug.Log("started coroutine" + time);
        yield return new WaitForSeconds(time);
        target.GetComponent<Rigidbody>().isKinematic = false;
        Debug.Log("ended coroutine");
    }

}
