using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoseCollider : MonoBehaviour {

    private Ball ball;
    private GM GM;

    private void Start() {
        ball = FindObjectOfType<Ball>();
        GM = FindObjectOfType<GM>();
    }

    void OnTriggerEnter2D(Collider2D trigger)
	{

	}

	void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.name == "Ball") {
            ball.hasStarted = false;
            GM.numberOfBlocksDestroyed = 0;
            GM.ReduceLives();
        }
        if (collision.gameObject.name == "Ball(Clone)") {
            Destroy(collision.gameObject);
        }
    }
}
