using UnityEngine;

public class PopulateAsteroidBelt : MonoBehaviour
{
    private Collider _volume;
    public GameObject[] asteroidPrefabs;
    public int numberSpawned;

    private int _randomIndex;
    private float _randScale;

    private void Start()
    {
        _volume = gameObject.GetComponent<Collider>();

        for (var i = 0; i < numberSpawned; i++)
        {
            _randomIndex = Random.Range(0, asteroidPrefabs.Length);
            _randScale = Random.Range(1, 2);

            var bounds = _volume.bounds;
            var spawnLoc = new Vector3(Random.Range(bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x), Random.Range(bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y), Random.Range(bounds.center.z - bounds.extents.z, bounds.center.z + bounds.extents.z));
            var asteroid = Instantiate(asteroidPrefabs[_randomIndex], spawnLoc, Random.rotation);

            asteroid.transform.parent = _volume.transform;
            asteroid.transform.localScale *= _randScale;

        }
    }
}