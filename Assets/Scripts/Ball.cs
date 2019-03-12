using System;
using UnityEngine;

// the ball flying around the screen in game
public class Ball : MonoBehaviour
{
    // the float numbers used for ball velocity
    public float ballStartSpeed = 2f;
    public float ballStartDirection = 2f;

    // track game state for movement purposes
    public bool hasStarted = false;
    public bool BallCaught;

    // other game classes coupled to this one
    public UpgradeManager UpgradeManager;
    Paddle paddle; 

    // position used for ball to paddle offset on screen
    Vector3 paddleToBallVector;
    Rigidbody2D rigidbody;

    // Use this for initialization
    void Start()
    {
        // find other components that the ball needs to interact with/use
        paddle = FindObjectOfType<Paddle>(); 
        UpgradeManager = FindObjectOfType<UpgradeManager>();
        // find given component on ball istelf
        rigidbody = GetComponent<Rigidbody2D>();
        // math to position ball
        paddleToBallVector = transform.position - paddle.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // check if the game has started
        if (!hasStarted || BallCaught )
        {
            // move the ball with the paddle prior to starting the game
            this.transform.position = paddle.transform.position + paddleToBallVector;
            // check for space bar or shift keys to be released and launch ball once it is. 
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // start ball movement
                rigidbody.velocity = new Vector2(ballStartDirection, ballStartSpeed);
                // modify game state
                BallCaught = false;
                hasStarted = true;
            } else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) {
                // same as above but for shift keys.
                rigidbody.velocity = new Vector2(-ballStartDirection, ballStartSpeed);
                BallCaught = false;
                hasStarted = true;
            }
        }

        // used in testing to allow for some "cheating" if the ball gets caught in a physcis loop (straight up/down or left/right)
        // left it in game because i <3 cheat codes
        if (GM.hasStarted && Input.GetKey(KeyCode.Space)) {
            // nudges the ball
            rigidbody.AddForce(new Vector2(1f, 1f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // checks for "Catch Ball" upgrade and handles collision if it's on
        // also checks for Paddle collision
        if (collision.gameObject.name == "Paddle" && !BallCaught && UpgradeManager.BallCatchingEnabled) {
            BallCaught = true;
        }
    }

    public void IncreaseSpeed(float multiplier) {
        // increases speed of ball by nudging it a certain amount.
        this.rigidbody.AddForce(new Vector2(
                transform.position.x * multiplier,
                transform.position.y * multiplier));

    }
}
