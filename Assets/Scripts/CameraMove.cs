using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float camSpeed = .1f;
    public Vector3 camVelocity;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindAnyObjectByType<PlayerController>().canMove)
        {
            transform.position += Vector3.forward * camSpeed;
        }
        
        camVelocity = Vector3.forward * camSpeed;
    }
}
