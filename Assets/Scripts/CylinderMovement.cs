using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;

public class CylinderMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed, dist;
    private float minx, maxx;
    public bool right, dontmove, stop;
    void Start()
    {
        maxx = transform.position.x+dist;
        minx = transform.position.x-dist;

    }

    // Update is called once per frame
    void Update()
    {
        if(!stop && !dontmove)
        {
            if(right)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
                if(transform.position.x >= maxx)
                {
                    right = false;
                }
            }
            else
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
                if (transform.position.x <= minx)
                {
                    right = true;
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="White" && collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude>1 || collision.gameObject.tag == "Player")
        {
            stop = true;
            GetComponent<Rigidbody>().freezeRotation = false;
        }
    }
}
