using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakerSpawner : MonoBehaviour
{
    public GameObject shakerPrefab;
    public Transform spawnPoint;
    public float checkInterval = 1f;

    private void Start()
    {
        InvokeRepeating(nameof(CheckAndSpawn), 0f, checkInterval);
    }

    void CheckAndSpawn()
    {
        if (GameObject.Find("Shaker") == null)
        {
            GameObject shaker = Instantiate(shakerPrefab, spawnPoint.position, spawnPoint.rotation);
            shaker.name = "Shaker";
            Debug.Log("🔁 Shaker spawned.");
        }
    }
}
