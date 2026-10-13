using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] laserPrefabs;

    // Seconds between lasers
    [SerializeField]
    float spawnInterval = 2.5f;

    float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnLaser();
            timer = spawnInterval;
        }
    }

    void SpawnLaser()
    {
        GameObject prefab = laserPrefabs[Random.Range(0, laserPrefabs.Length)];

        Instantiate(prefab, transform.position + prefab.transform.position, prefab.transform.rotation);
    }
}
