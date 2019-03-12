using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    // injects for other gameObjects, publicly exposed so you can drag and drop when in the Unity editor.
    public Paddle Paddle;
    public Ball Ball;
    public Text upgradeText;

    public static int TurnCounter; // a turn will be defined as the main ball hitting the paddle. use this to count them

    // set upgrade String for use in switch statement later
    public string CurrentUpgrade = "nothing"; 

    // state management booleans
    public bool NoUpgradeExists = true;
    public bool isPaddleElongated = false;
    public bool MultipleBalls = false;
    public bool BallCatchingEnabled = false;

    // a ball to hold an extra one when the upgrade is obtained
    private static Ball ExtraBall;

    // used to quickly access amount when adjusting the paddle on upgrade.
    private Vector3 resizeAmount = new Vector3(2f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        // the game doesn't start with any upgrades
        upgradeText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // check if the upgrade needs to go away
        if (TurnCounter == 0) {
            // set string for status check later
            CurrentUpgrade = "nothing";

            // see which upgrade was enabled and then run the method to kill the upgrade
            if (isPaddleElongated) {
                Debug.Log("Shrinking Paddle");
                DelongatePaddle();
            } else if (MultipleBalls) {
                DeleteExtraBall();
            } else if (BallCatchingEnabled) {
                DisableBallCatching();
            }
        }

        switch (CurrentUpgrade) {
            case "nothing":
                // update state to reflect nothingness
                upgradeText.enabled = false;
                NoUpgradeExists = true;
                isPaddleElongated = false;
                BallCatchingEnabled = false;
                MultipleBalls = false;
                break;
            case "longPaddle":
                // show the upgrade text and then call the method, but not if the method has been called already.
                upgradeText.enabled = true;
                if (!isPaddleElongated) {
                    ElongatePaddle();
                }
                NoUpgradeExists = false; 
                // show the player what upgrade is active and the number of turns left
                upgradeText.text = "Long Paddle: " + TurnCounter;
                break;
            case "multiBall":
                // same as above
                upgradeText.enabled = true;
                if (!MultipleBalls) {
                    LaunchExtraBall();
                }
                NoUpgradeExists = false;
                upgradeText.text = "Multi Ball: " + TurnCounter;
                break;
            case "ballCatcher":
                // same as above
                upgradeText.enabled = true;
                if (!BallCatchingEnabled) {
                    EnableBallCatch();
                }
                NoUpgradeExists = false;
                upgradeText.text = "Ball Catcher: " + TurnCounter;
                break;
        }

        // manage the UpgradeDropper so it doesn't spawn upgrades when one is active
        if (GM.hasStarted && NoUpgradeExists) {
            UpgradeDropper.Activated = true;
        } else {
            UpgradeDropper.Activated = false;
        }
    }

    // methods to grow and reduce paddle. and manage the state.
    void ElongatePaddle() {
        Paddle.transform.localScale += resizeAmount;
        isPaddleElongated = true;
    }

    void DelongatePaddle() {
        Paddle.transform.localScale -= resizeAmount;
        isPaddleElongated = false;
    }

    void LaunchExtraBall() {
        // create a new position for the extra ball to start at, base it off the paddle we injected.
        var newBallPosition = Paddle.transform.position + new Vector3(0f, .5f, 0);

        // create the ball
        ExtraBall = Instantiate(Ball, newBallPosition, new Quaternion());

        // setup the numbers for starting the ball's velocity
        ExtraBall.ballStartDirection = 2f;
        ExtraBall.ballStartSpeed = 2f;

        //get components from the extra ball
        var newBallRigidbody = ExtraBall.GetComponent<Rigidbody2D>();
        var newBallRenderer = ExtraBall.GetComponent<Renderer>();

        //set the extra ball to a different color
        newBallRenderer.material.color = Color.grey;

        // start the ball moving
        newBallRigidbody.velocity = new Vector2(ExtraBall.ballStartDirection, ExtraBall.ballStartSpeed);

        // update state
        MultipleBalls = true;
    }

    private void DeleteExtraBall() {
        // remove the extra ball and manage state
        Destroy(ExtraBall.gameObject);
        MultipleBalls = false;
    }

    // these methods manage the state for ball catching. other objects will use this for ensuring it gets enacted
    void EnableBallCatch() {
        BallCatchingEnabled = true;
    }

    private void DisableBallCatching() {
        BallCatchingEnabled = false;
    }
}
