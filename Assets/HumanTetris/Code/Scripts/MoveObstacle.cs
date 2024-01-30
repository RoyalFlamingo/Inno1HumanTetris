using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public float speed;

    private Rigidbody obstacleRb;
    private bool fading = false;

    // Start is called before the first frame update
    void Start()
    {
        obstacleRb = GetComponent<Rigidbody>();
		obstacleRb.velocity = Vector3.left * speed;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		obstacleRb.velocity = Vector3.left * speed;

        if(!fading && transform.position.x < -16)
        {
            fading = true;
            gameObject.GetComponent<FadeOut>().startFading();
        }
        else if(transform.position.x < -40)
        {
            Destroy(gameObject);
        }
    }
}
