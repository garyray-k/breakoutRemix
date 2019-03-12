using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public string CurrentUpgrade = "nothing";
    public Paddle Paddle;
    public Ball Ball;
    public Text upgradeText;
    public static int TurnCounter; // a turn will be defined as the main ball hitting the paddle.
    public bool NoUpgradeExists = true;
    public bool isPaddleElongated = false;
    public bool MultipleBalls = false;
    public bool BallCatchingEnabled = false;

    private static Ball ExtraBall;

    private Vector3 resizeAmount = new Vector3(2f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        upgradeText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnCounter == 0) {
            CurrentUpgrade = "nothing";
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
                upgradeText.enabled = false;
                NoUpgradeExists = true;
                isPaddleElongated = false;
                BallCatchingEnabled = false;
                MultipleBalls = false;
                break;
            case "longPaddle":
                upgradeText.enabled = true;
                if (!isPaddleElongated) {
                    ElongatePaddle();
                }
                NoUpgradeExists = false; 
                upgradeText.text = "Long Paddle: " + TurnCounter;
                break;
            case "multiBall":
                upgradeText.enabled = true;
                if (!MultipleBalls) {
                    LaunchExtraBall();
                }
                NoUpgradeExists = false;
                upgradeText.text = "Multi Ball: " + TurnCounter;
                break;
            case "ballCatcher":
                upgradeText.enabled = true;
                if (!BallCatchingEnabled) {
                    EnableBallCatch();
                }
                NoUpgradeExists = false;
                upgradeText.text = "Ball Catcher: " + TurnCounter;
                break;
        }

        if (GM.hasStarted && NoUpgradeExists) {
            UpgradeDropper.Activated = true;
        } else {
            UpgradeDropper.Activated = false;
        }
    }

    void ElongatePaddle() {
        Paddle.transform.localScale += resizeAmount;
        isPaddleElongated = true;
    }

    void DelongatePaddle() {
        Paddle.transform.localScale -= resizeAmount;
        isPaddleElongated = false;
    }

    void LaunchExtraBall() {
        var newBallPosition = Paddle.transform.position + new Vector3(0f, .5f, 0);
        ExtraBall = Instantiate(Ball, newBallPosition, new Quaternion());
        ExtraBall.ballStartDirection = 2f;
        ExtraBall.ballStartSpeed = 2f;
        var newBallRigidbody = ExtraBall.GetComponent<Rigidbody2D>();
        var newBallRenderer = ExtraBall.GetComponent<Renderer>();
        newBallRenderer.material.color = Color.grey;
        newBallRigidbody.velocity = new Vector2(ExtraBall.ballStartDirection, ExtraBall.ballStartSpeed);
        MultipleBalls = true;
    }

    private void DeleteExtraBall() {
        Destroy(ExtraBall.gameObject);
        MultipleBalls = false;
    }

    void EnableBallCatch() {
        BallCatchingEnabled = true;
    }

    private void DisableBallCatching() {
        BallCatchingEnabled = false;
    }
}
