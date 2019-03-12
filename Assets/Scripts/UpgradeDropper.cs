using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDropper : MonoBehaviour
{
    public UpgradePill UpgradePillPrefab;

    public float maxUpgradeGenerationTime = 2;
    public float minUpgradeGenerationTime = 5;

    private float time;
    private float spawnTime;

    public static bool Activated = false;

    private Camera mainCamera;
    private Rect cameraRect;
    
    void Start() {
        mainCamera = FindObjectOfType<Camera>();
        var bottomLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
        var topRight = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight));
        cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);
        GenerateRandomTime();
        time = minUpgradeGenerationTime;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= spawnTime && Activated) {
            SpawnUpgradePill();
            GenerateRandomTime();
        }
    }

    private void GenerateRandomTime() {
        spawnTime = UnityEngine.Random.Range(minUpgradeGenerationTime, maxUpgradeGenerationTime);
    }

    private void SpawnUpgradePill() {
        time = minUpgradeGenerationTime;
        var spawnPosition = new Vector3(
                        UnityEngine.Random.Range(cameraRect.xMin + .5f, cameraRect.xMax - .5f),
                        cameraRect.yMax, 
                        1f);
        Instantiate(UpgradePillPrefab, spawnPosition, new Quaternion());
    }

    public void DropPills(bool toDropOrNotToDrop) {
        Activated = toDropOrNotToDrop;
    }
}
