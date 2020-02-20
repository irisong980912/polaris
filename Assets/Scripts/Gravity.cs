using UnityEngine;

public class Gravity : MonoBehaviour
{

    private GameObject _player;
    public float gravityStrength;
    public float gravityRadius = 100.0f;

    private void Start()
    {

        try
        {
            foreach (var o in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                var go = (GameObject)o;
                if (!go.tag.Contains("|Player|")) continue;
                _player = go;
                break;
            }
        }
        catch (UnityException)
        {
            print("No |Player|");
        }
    }

    private void Update()
    {
       
        // enforce gravity on player only when player is within the gravitational field
        // TODO:figure out the number for gravitational field
        float disToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        if (disToPlayer <= gravityRadius)
        {
            _player.GetComponent<Rigidbody>().AddExplosionForce(
                -(gravityStrength * disToPlayer),
                transform.position,
                gravityRadius,
                0.0f,
                ForceMode.Force
            );
        }


    }


}