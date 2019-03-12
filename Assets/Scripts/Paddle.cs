using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {
    // boolean for auto-testing the game
    public bool autoPlay = false;
    // variable to change paddle speed
    public float paddleSpeed = 0.2f;

    // objects to hold other objects from within the scene
    private Camera mainCamera;
    private Ball ball;
    private Rect cameraRect;

    private void Start() {
        ball = FindObjectOfType<Ball>();

        // get camera and boundaries of camera
        mainCamera = FindObjectOfType<Camera>();
        var bottomLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
        var topRight = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight));
        cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);
    }

    void Update() {
        // determine if in autoplay mode or use the keys
        if (autoPlay) {
            AutoPlay();
        } else {
            MoveWithKeys();
        }
    }

    private void MoveWithKeys() {
        // move with the arrow keys
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.position += Vector3.left * paddleSpeed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            transform.position += Vector3.right * paddleSpeed * Time.deltaTime;
        }
        // prevent paddle from going off screen
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, cameraRect.xMin + 0.5f, cameraRect.xMax - 0.5f),
            transform.position.y,
            transform.position.z);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        // detect ball collision for upgrade tracking purposes
        if (collision.gameObject.name.Contains("Ball")) {
            UpgradeManager.TurnCounter--;
        }
    }

    // follow the ball. buggy with an extra ball though and some other things.
    void AutoPlay() {
        Vector3 paddlePos = new Vector3(0.5f, transform.position.y, 0f);
        float ballPosition = ball.transform.position.x;
        paddlePos.x = Mathf.Clamp(ballPosition, -15f, 15f);
        transform.position = paddlePos;
    }
}
