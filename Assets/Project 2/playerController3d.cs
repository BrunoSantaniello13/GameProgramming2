using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController3d : MonoBehaviour
{
    public float speed = 10f;
    Rigidbody myRB;
    public Camera myCam;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myLook = myCam.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        //DeltaLook();
        Vector3 playerLook = myCam.transform.forward;
      //  Vector3 newLook = DeltaLook();
        myLook += DeltaLook() * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0f, myLook.x, 0f);
        myCam.transform.rotation = Quaternion.Euler(-myLook.y, myLook.x, 0f);
    }
    private void FixedUpdate()
    {
        myRB.AddForce(Dir() * speed * Time.fixedDeltaTime * 100);
    }

    //Basic Movement WASD
    Vector3 Dir()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 myDir = new Vector3(x,0,z);
        return myDir;
    }

    //Mouse Camera
    Vector3 DeltaLook()
    {
        Vector3 dLook = Vector3.zero;
        float rotY = Input.GetAxisRaw("Mouse Y") * speed;
        float rotX = Input.GetAxisRaw("Mouse X") * speed;
        dLook = new Vector3(rotY, rotX, 0);
        return dLook;
    }
}
