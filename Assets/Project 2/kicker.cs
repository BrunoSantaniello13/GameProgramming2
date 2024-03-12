using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class kicker : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(curBall.transform);
        Move();
    }
    internal override void Move()
    { 
        Vector3 dir = transform.TransfromDirection(Vector3.forward);
        dir *= mySpeed;
        myRB.AddForce(dir);
    }
}
