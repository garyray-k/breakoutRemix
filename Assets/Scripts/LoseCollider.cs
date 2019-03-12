using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the bottom most border determines if the players loses a life or not
public class LoseCollider : MonoBehaviour {
    // objects to hold other objects in scene
    private Ball ball;
    private GM GM;

    private void Start() {
        // find relevant objects and assign them to containers
        ball = FindObjectOfType<Ball>();
        GM = FindObjectOfType<GM>();
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.name == "Ball") {
            // set state so ball returns to starting position
            ball.hasStarted = false;
            // reset speed tracking variabl in GameManager
            GM.numberOfBlocksDestroyed = 0;
            // does what it says
            GM.ReduceLives();
        }
        // see if it's an extra ball from an upgrade.
        if (collision.gameObject.name == "Ball(Clone)") {
            Destroy(collision.gameObject);
        }
    }
}
