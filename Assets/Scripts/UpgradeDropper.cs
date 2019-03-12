using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDropper : MonoBehaviour
{
    // drag and drop the prefab pill for use when spawning things
    public UpgradePill UpgradePillPrefab;

    // used for random timing of upgrade spawns
    public float maxUpgradeGenerationTime = 2;
    public float minUpgradeGenerationTime = 5;
    private float time;
    private float spawnTime;

    // on/off switch for the spawner so it doesn't keep going when an upgrade is active
    public static bool Activated = false;

    // objects to hold the camera and view field
    private Camera mainCamera;
    private Rect cameraRect;
    
    void Start() {
        // find camera in scene
        mainCamera = FindObjectOfType<Camera>();
        // establish points using the camera view field
        var bottomLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
        var topRight = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight));
        // generate the Rect from the view field
        cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);
        // set timing for spawning
        GenerateRandomTime();
        // baseline the time for spawning
        time = minUpgradeGenerationTime;
    }

    // Update is called once per frame
    void Update()
    {
        // track time
        time += Time.deltaTime;
        // if the time has passed enough, spawn an upgrade
        if (time >= spawnTime && Activated) {
            SpawnUpgradePill();
            GenerateRandomTime();
        }
    }

    private void GenerateRandomTime() {
        // random time for spawning
        spawnTime = UnityEngine.Random.Range(minUpgradeGenerationTime, maxUpgradeGenerationTime);
    }

    private void SpawnUpgradePill() {
        // reset time baseline
        time = minUpgradeGenerationTime;
        // establish where the upgradePill will spawn
        var spawnPosition = new Vector3(
                        UnityEngine.Random.Range(cameraRect.xMin + .5f, cameraRect.xMax - .5f),
                        cameraRect.yMax, 
                        1f);
        // finally spawn the object onto the scene
        Instantiate(UpgradePillPrefab, spawnPosition, new Quaternion());
    }

    public void DropPills(bool toDropOrNotToDrop) {
        // toggle method
        Activated = toDropOrNotToDrop;
    }
}
