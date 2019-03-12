using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePill : MonoBehaviour
{
    private Renderer Renderer;

    public UpgradeManager upgradeManager;

    public enum UpgradeType { longPaddle, multiBall, ballCatcher };

    public UpgradeType upgradeType;

    void Start()
    {
        Renderer = GetComponent<Renderer>();
        upgradeManager = FindObjectOfType<UpgradeManager>();
        upgradeType = (UpgradeType)UnityEngine.Random.Range(0, 3);

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

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Paddle" && GM.hasStarted) {
            upgradeManager.CurrentUpgrade = upgradeType.ToString();
            upgradeManager.NoUpgradeExists = false;
            UpgradeManager.TurnCounter = 4;
            Debug.Log(upgradeType.ToString() + " upgrade picked up.");
        } else if (collision.gameObject.name == "Bottom") {
            Debug.Log("Upgrade not gathered");
        }
        Destroy(this.gameObject);
        Destroy(this.gameObject);
    }
}

