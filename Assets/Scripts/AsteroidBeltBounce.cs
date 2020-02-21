using UnityEngine;

/// <summary>
/// When a player collides with an asteroid belt, imparts a force on the player opposite the vector of their velocity.
/// </summary>
public class AsteroidBeltBounce : MonoBehaviour
{
    // TODO: play around with explosionStrength and find the best value 
    public float explosionStrength = 10.0f;

    private void OnCollisionEnter(Collision c)
    {
        Debug.Log(c.rigidbody.velocity.normalized);
        var forceVec = c.rigidbody.velocity.normalized * explosionStrength;
        c.rigidbody.AddForce(forceVec, ForceMode.Impulse);
    }
}
