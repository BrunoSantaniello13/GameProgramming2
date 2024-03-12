using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalie : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start() {   }

    // Update is called once per frame
    //void Update() {   }}

    internal override void Move()
    {
     float zDiff = curball.transform.position.z - transfrom.position.z;
     Vector3 dir = new Vector3(0f, 0f, zDiff).normalized;
     CapsuleDirection2D *= mySpeed;
     myRB.AddForce(dir);
    }

}