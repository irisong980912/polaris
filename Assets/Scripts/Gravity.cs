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

    }

    private void Update()
    {
        if (withinGravityRadius == true)
        {

            if (disToPlayer <= 100.0f) // small gravity allow player to get onto the planets easier
            {

                gravityStrength = 10.0f;

                _player.GetComponent<Rigidbody>().AddExplosionForce(
                -gravityStrength,
                transform.position,
                gravityRadius,
                0.0f,
                ForceMode.Force
                );
            }
            else if (disToPlayer <= 130.0f)
            {
                gravityStrength = 20.0f;

                _player.GetComponent<Rigidbody>().AddExplosionForce(
                -gravityStrength,
                transform.position,
                gravityRadius,
                0.0f,
                ForceMode.Force
                );
            }

            else if (disToPlayer <= 160.0f)
            {
                gravityStrength = 30.0f;

                _player.GetComponent<Rigidbody>().AddExplosionForce(
                -gravityStrength,
                transform.position,
                gravityRadius,
                0.0f,
                ForceMode.Force
                );
            }
            else // disToPlayer > 160.0f,
            // should also be porpotional to the player speed (for speed up implementation)
            {
                float playerSpeedRatio = 1 / _player.GetComponent<ThirdPersonPlayer>().speed;
                gravityStrength = (disToPlayer / 20) * (disToPlayer / 20) * playerSpeedRatio;

                _player.GetComponent<Rigidbody>().AddExplosionForce(
                -gravityStrength,
                transform.position,
                gravityRadius,
                0.0f,
                ForceMode.Force
                );
            }

        }
        


    }


}