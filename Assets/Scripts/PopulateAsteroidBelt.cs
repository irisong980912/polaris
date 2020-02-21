using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateAsteroidBelt : MonoBehaviour
{

    Collider volume;
    public GameObject[] asteroidPrefabs;
    public int numberSpawned = 0;

    public float radius;
    int randomIndex;
    float randScale;

    void Start()
    {
        volume = gameObject.GetComponent<Collider>();
        radius = volume.bounds.extents.x;

        for (int i = 0; i < numberSpawned; i++)
        {
            randomIndex = Random.Range(0, asteroidPrefabs.Length);
            randScale = Random.Range(1, 2);

            Vector3 spawnLoc = new Vector3(Random.Range(volume.bounds.center.x - volume.bounds.extents.x, volume.bounds.center.x + volume.bounds.extents.x), Random.Range(volume.bounds.center.y - volume.bounds.extents.y, volume.bounds.center.y + volume.bounds.extents.y), Random.Range(volume.bounds.center.z - volume.bounds.extents.z, volume.bounds.center.z + volume.bounds.extents.z));
            GameObject asteroid = Instantiate(asteroidPrefabs[randomIndex], spawnLoc, Random.rotation);

            asteroid.transform.parent = volume.transform;
            asteroid.transform.localScale *= randScale;

        }
    }

    void Update()
    {

        
    }


}
