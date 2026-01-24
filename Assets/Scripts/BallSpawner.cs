using UnityEngine;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;

    [Header("Ball Limit")]
    public int maxBalls = 10;

    private List<GameObject> spawnedBalls = new List<GameObject>();

    public void SpawnBall()
    {
        if (ballPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("BallSpawner missing prefab or spawnPoint.");
            return;
        }

        // If we already have max balls, remove the oldest one
        if (spawnedBalls.Count >= maxBalls)
        {
            GameObject oldestBall = spawnedBalls[0];
            spawnedBalls.RemoveAt(0);

            if (oldestBall != null)
                Destroy(oldestBall);
        }

        // Spawn new ball
        GameObject newBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);

        // Track it
        spawnedBalls.Add(newBall);
    }
}


