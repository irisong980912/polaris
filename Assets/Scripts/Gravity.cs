using UnityEngine;
using UnityEngine.Serialization;

public class Gravity : MonoBehaviour
{
    public float gravityRadius = 180.0f;
    
    private GameObject _player;
    public float gravityStrength;
    public float disToPlayer;
    private bool _withinGravityRadius;

    // TODO: See if this collider is needed.
    private void OnTriggerStay(Collider c)
    {
        if (!c.gameObject.tag.Contains("|Player|")) return;
        _player = c.gameObject;
        disToPlayer = Vector3.Distance(transform.position, _player.transform.position);
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
        if (disToPlayer <= gravityRadius * .65f) // small gravity allow player to get onto the planets easier
        {
            gravityStrength = 5.0f;
        }
        else if (disToPlayer <= gravityRadius * .75f)
        {
            gravityStrength = 20.0f;
        }

        else if (disToPlayer <= gravityRadius * .85f)
        {
            gravityStrength = 40.0f;
        }
        else // disToPlayer > gravityRadius * .85f
        {
            // gravity strength is proportional to player speed and distance
            var playerSpeedRatio = 1 / _player.GetComponent<ThirdPersonPlayer>().speed;
            gravityStrength = (disToPlayer / 20) * (disToPlayer / 20) * playerSpeedRatio * 8;
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