using UnityEngine;

public class Gravity : MonoBehaviour
{

    private GameObject _player;
    public float gravityStrength;
    public float gravityRadius = 180.0f;

    public float disToPlayer;

    private bool withinGravityRadius = false;


    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag.Contains("|Player|"))
        {
            _player = c.gameObject;
            disToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            //Debug.Log("disToPlayer is: " + disToPlayer);
            withinGravityRadius = true;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag.Contains("|Player|"))
        {
            Debug.Log("exit gravity distance");

            withinGravityRadius = false;
        }
    }



    private void Start()
    {

        Debug.Log("CHILD GRAVITY enabled!!!!");

        //try
        //{
        //    foreach (var o in GameObject.FindObjectsOfType(typeof(GameObject)))
        //    {
        //        var go = (GameObject)o;
        //        if (!go.tag.Contains("|Player|")) continue;
        //        _player = go;
        //        break;
        //    }
        //}
        //catch (UnityException)
        //{
        //    print("No |Player|");
        //}
    }

    private void Update()
    {
        if (withinGravityRadius == true)
        {
            // enforce gravity on player only when player is within the gravitational field
            // TODO:figure out the number for gravitational field
            Debug.Log("Gravity strength: " + gravityStrength * disToPlayer / 10 * disToPlayer / 10);

            _player.GetComponent<Rigidbody>().AddExplosionForce(
                -(gravityStrength * disToPlayer / 10 * disToPlayer / 10),
                transform.position,
                gravityRadius,
                0.0f,
                ForceMode.Force
            );
            

        }
        


    }


}