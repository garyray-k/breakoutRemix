using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    // The Game Master class to handle core game logic and the like.

        // UI objects that are on the screen. drag and drop in editor to assign.
    public Text livesText;
    private string livesTextString = "Lives: "; // format variable for use later in a win condition
    public Text scoreText;
    public Text arrowInstructionsText;
    public Text beginGameText;

    // state for upgrades
    public static bool longPaddleUpgrade;
    public static bool multiBallUpgrade;
    public static bool catchBallUpgrade;

    // tracking numbers for win condition
    public int lives = 3;
    public static int numberOfBlocksDestroyed = 0;

    //object holder for gameobjects in scene
    private static Ball ball;
    private Paddle paddle;
    public UpgradeDropper UpgradeDropper;

    // state of the game
    public static bool hasStarted = false;

    // variable to hold the score that's not in the text
    // can be used for win condition
    private static int score;

    void Start()
    {
        // find objects in scene and assign to appropriate variables
        ball = FindObjectOfType<Ball>();
        paddle = FindObjectOfType<Paddle>();
        UpgradeDropper = FindObjectOfType<UpgradeDropper>();
        // run to set everything to the desired beginning value
        RestartGame();
    }

    void RestartGame() {
        // manage state to the right setting, set variables to a "cleansed" setting
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
            // don't show the unstruction text when the game has started
            arrowInstructionsText.enabled = false;
            beginGameText.enabled = false;
        }
        // update the UI text
        livesText.text = string.Format(livesTextString + lives);
        scoreText.text = string.Format("Score: {0}", score);

        // update UI based on order of key strokes
        // basically want arro key text to show first
        // then show the text on how to launch the ball
        // this ensure the player knows all the necessary keys for playing the game
        if (Input.GetKey(KeyCode.LeftArrow) && !hasStarted) {
            arrowInstructionsText.enabled = false;
            beginGameText.enabled = true;
        } else if (Input.GetKey(KeyCode.RightArrow) && !hasStarted) {
            arrowInstructionsText.enabled = false;
            beginGameText.enabled = true;
        } 
        // continue to see if the "start" buttons have been pressed.
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
            beginGameText.enabled = false;
            hasStarted = true;
        }

        // win condition based on score, since we know the number of blocks and point values for each
        // we can assess that a winning score == 256
        if (score == 256) {
            // this permanent changes the "Lives" UI text
            // to indicate that the game has been previously won.
            livesTextString = "WINNER! Lives: ";
            RestartGame();
        }
    }

    // don't like how I did this one but it works.
    public static void IncreaseScoreAndHandleBallSpeedOnBlockDestruction(Block block) {
        // increase the score and appropriate amount based in the given Block that is destroyed
        score += block.pointValue;
        // incrememnt the tracker
        numberOfBlocksDestroyed++;
        // check if conditions are met to increase the ball speed
        bool needToIncreaseBallSpeed =
                  numberOfBlocksDestroyed == 4 ||
                  numberOfBlocksDestroyed == 12 ||
                  block.pointValue == 5 || // block is orange
                  block.pointValue == 7; // block is red
         
        // if needed, call increase speed method on the ball, sending a multiplier to the method.               
        if (needToIncreaseBallSpeed) {
            ball.IncreaseSpeed(4f);
        }
    }

    public void ReduceLives() {
        // reduce lives on a lose condition
        // reset the game if all lives are lost
        lives--;
        if (lives == 0) {
            RestartGame();
        }
    }

}
