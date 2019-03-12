using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePill : MonoBehaviour
{
    // used for color changing
    private Renderer Renderer;

    // inject other class
    public UpgradeManager upgradeManager;

    // eases handling of different colors.
    public enum UpgradeType { longPaddle, multiBall, ballCatcher };
    public UpgradeType upgradeType;

    void Start()
    {
        // find component on gameObject itsled
        Renderer = GetComponent<Renderer>();

        // get the Upgrade manager from within the scene
        upgradeManager = FindObjectOfType<UpgradeManager>();

        // randomly select an upgrade type to be.
        upgradeType = (UpgradeType)UnityEngine.Random.Range(0, 3);

        //change color based on random selection above
        switch (upgradeType) {
            case UpgradeType.longPaddle:
                Renderer.material.color = Color.yellow;
                break;
            case UpgradeType.multiBall:
                Renderer.material.color = Color.blue;
                break;
            case UpgradeType.ballCatcher:
                Renderer.material.color = Color.green;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.name);
        // check game state and collision obejct
        if (collision.gameObject.name == "Paddle" && GM.hasStarted) {
            // changes screen test in the injected UpgradeManager
            upgradeManager.CurrentUpgrade = upgradeType.ToString();
            // modify game state to reflect that an upgrade just started
            upgradeManager.NoUpgradeExists = false;
            // start countdown at 4 for upgrade "turn"
            UpgradeManager.TurnCounter = 4;
            Debug.Log(upgradeType.ToString() + " upgrade picked up.");

        } else if (collision.gameObject.name == "Bottom") {
            Debug.Log("Upgrade not gathered");
        }
        // if it hits anything, it will destroy itself do we don't overflow with upgrade pills raining down.
        Destroy(this.gameObject);
    }
}

