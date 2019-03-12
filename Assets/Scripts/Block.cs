using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public static int numOfBlocksRemainingOnScreen = 64;
    public static GameObject[] blocksInScene;
    public int pointValue = 1;

    Camera cam;

	// Use this for initialization
	void Start () {
        cam = Camera.main; 
        blocksInScene = GameObject.FindGameObjectsWithTag("Block");
    }

	void Update () {

	}

    public static void ResetBlocks() {
        numOfBlocksRemainingOnScreen = 64;
        foreach (var block in blocksInScene) {
            block.SetActive(true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // make sure we're hitting a block
        bool isBlock = (this.tag == "Block");
        if (isBlock) {
            HandleHits();
        }
    }

    void HandleHits () {
        // smoke and disbale GameObject
        print("Block hit!");
        numOfBlocksRemainingOnScreen--;
        // don't like this one but maybe we'll change it
        GM.IncreaseScoreAndHandleBallSpeedOnBlockDestruction(this);
        gameObject.SetActive(false);
    }

}
