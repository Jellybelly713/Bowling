using UnityEngine;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public int maxBalls = 10;

    private List<GameObject> spawnedBalls = new List<GameObject>();

    public void SpawnBall()
    {
        if (ballPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("BallSpawner missing prefab");
            return;
        }

        // removes the oldest ball
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


