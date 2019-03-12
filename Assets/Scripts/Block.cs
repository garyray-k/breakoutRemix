using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    // track for win condition and reset purposes
    public static int numOfBlocksRemainingOnScreen = 64;
    public static GameObject[] blocksInScene;

    //publicly expose to be easily changed in Unity Editorfor different values
    public int pointValue = 1;
    // object holder
    Camera cam;

	// Use this for initialization
	void Start () {
        // find objects in scene and assign to desired object holders.
        cam = Camera.main; 
        blocksInScene = GameObject.FindGameObjectsWithTag("Block");
    }

    public static void ResetBlocks() {
        // loop through the blocks and re-eneable them so the player can keep playing
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
        // disable GameObject, reduce tracking number and tell the GM that we hit a block
        numOfBlocksRemainingOnScreen--;
        // don't like the name of this one but maybe we'll change it
        // pass this object to the GM for handling
        GM.IncreaseScoreAndHandleBallSpeedOnBlockDestruction(this);
        gameObject.SetActive(false);
    }

}
