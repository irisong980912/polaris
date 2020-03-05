using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravityRadius = 180.0f;
    
    private GameObject _player;
    public float gravityStrength;
    private float _disToPlayer;
    private bool _withinGravityRadius;

    // TODO: See if this collider is needed.
    private void OnTriggerStay(Collider c)
    {
        if (!c.gameObject.tag.Contains("|Player|")) return;
        _player = c.gameObject;
        _disToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        //Debug.Log("disToPlayer is: " + disToPlayer);
        _withinGravityRadius = true;
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag.Contains("|Player|"))
        {
            _withinGravityRadius = false;
        }
        
    }

    private void Start()
    {
        Debug.Log("CHILD GRAVITY enabled!!!!");
    }

    private void Update()
    {
        if (_withinGravityRadius != true) return;
        if (_disToPlayer <= gravityRadius * .65f) // small gravity allow player to get onto the planets easier
        {
            gravityStrength = 5.0f;
        }
        else if (_disToPlayer <= gravityRadius * .75f)
        {
            gravityStrength = 15.0f;
        }

        else if (_disToPlayer <= gravityRadius * .85f)
        {
            gravityStrength = 30.0f;
        }
        else // disToPlayer > gravityRadius * .85f
        {
            // gravity strength is proportional to player speed and distance
            var playerSpeedRatio = 1 / _player.GetComponent<ThirdPersonPlayer>().speed;
            gravityStrength = (_disToPlayer / 20) * (_disToPlayer / 20) * playerSpeedRatio * 8;
        }
        
        _player.GetComponent<Rigidbody>().AddExplosionForce(
            -gravityStrength,
            transform.position,
            gravityRadius,
            0.0f,
            ForceMode.Force
        );

    }


}