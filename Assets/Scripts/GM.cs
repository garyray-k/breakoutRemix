using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    // The Game Master class to handle core game logic and the like.

    public Text livesText;
    private string livesTextString = "Lives: ";
    public Text scoreText;
    public Text arrowInstructionsText;
    public Text beginGameText;

    public static bool longPaddleUpgrade;
    public static bool multiBallUpgrade;
    public static bool catchBallUpgrade;

    public int lives = 3;
    public static int numberOfBlocksDestroyed = 0;

    private static Ball ball;

    private Paddle paddle;
    public static bool hasStarted = false;

    private static int score;

    public UpgradeDropper UpgradeDropper;

    void Start()
    {
        ball = FindObjectOfType<Ball>();
        paddle = FindObjectOfType<Paddle>();
        UpgradeDropper = FindObjectOfType<UpgradeDropper>();
        RestartGame();
    }

    void RestartGame() {
        beginGameText.enabled = false;
        arrowInstructionsText.enabled = true;
        hasStarted = false;
        score = 0;
        lives = 3;
        Block.ResetBlocks();
        ball.hasStarted = false;
        numberOfBlocksDestroyed = 0;
        ball.ballStartSpeed = 2;
        ball.ballStartDirection = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted) {
            arrowInstructionsText.enabled = false;
            beginGameText.enabled = false;
        }

        livesText.text = string.Format(livesTextString + lives);
        scoreText.text = string.Format("Score: {0}", score);

        // update UI based on order of key strokes
        if (Input.GetKey(KeyCode.LeftArrow) && !hasStarted) {
            arrowInstructionsText.enabled = false;
            beginGameText.enabled = true;
        } else if (Input.GetKey(KeyCode.RightArrow) && !hasStarted) {
            arrowInstructionsText.enabled = false;
            beginGameText.enabled = true;
        } 

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
            beginGameText.enabled = false;
            hasStarted = true;
        }

        if (score == 256) {
            livesTextString = "WINNER! Lives: ";
            RestartGame();
        }
    }

    // don't like how I did this one but it works.
    public static void IncreaseScoreAndHandleBallSpeedOnBlockDestruction(Block block) {
        score += block.pointValue;
        numberOfBlocksDestroyed++;

        bool needToIncreaseBallSpeed =
                  numberOfBlocksDestroyed == 4 ||
                  numberOfBlocksDestroyed == 12 ||
                  block.pointValue == 5 || // block is orange
                  block.pointValue == 7; // block is red
                            
        if (needToIncreaseBallSpeed) {
            ball.IncreaseSpeed(4f);
        }
    }

    public void ReduceLives() {
        lives--;
        if (lives == 0) {
            RestartGame();
        }
    }

}
