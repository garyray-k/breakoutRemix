using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float ballStartSpeed = 2f;
    public float ballStartDirection = 2f;
    public bool hasStarted = false;
    public UpgradeManager UpgradeManager;
    public bool BallCaught;

    Paddle paddle; 
    Vector3 paddleToBallVector;
    Rigidbody2D rigidbody;

    // Use this for initialization
    void Start()
    {
        // find other components that the ball needs to interact with/use
        paddle = FindObjectOfType<Paddle>();
        paddleToBallVector = transform.position - paddle.transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        UpgradeManager = FindObjectOfType<UpgradeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if the game has started
        if (!hasStarted || BallCaught )
        {
            // move the ball with the paddle prior to starting the game
            this.transform.position = paddle.transform.position + paddleToBallVector;
            // check for space bar to be releases and launch ball once it is. 
            if (Input.GetKeyUp(KeyCode.Space))
            {
                rigidbody.velocity = new Vector2(ballStartDirection, ballStartSpeed);
                BallCaught = false;
                hasStarted = true;
            } else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) {
                rigidbody.velocity = new Vector2(-ballStartDirection, ballStartSpeed);
                BallCaught = false;
                hasStarted = true;
            }
        }

        if (GM.hasStarted && Input.GetKey(KeyCode.Space)) {
            rigidbody.AddForce(new Vector2(1f, 1f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Paddle" && !BallCaught && UpgradeManager.BallCatchingEnabled) {
            BallCaught = true;
        }
    }

    public void IncreaseSpeed(float multiplier) {
        this.rigidbody.AddForce(new Vector2(
                transform.position.x * multiplier,
                transform.position.y * multiplier));

    }
}
