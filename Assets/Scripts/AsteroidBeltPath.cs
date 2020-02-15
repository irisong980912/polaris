using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bends the path of the asteroid belt when gravity fields form by rotating parents in the asteroid belt.
/// When an element of the asteroid belt rotates, all of its children will rotate in that direction,
/// effectively moving the entire asteroid belt.
/// </summary>
/// <remarks>
/// The head of the asteroid belt cannot move.
/// </remarks>
public class AsteroidBeltPath : MonoBehaviour
{
    // TODO: Test values to find a good value for this, then code it in and set to private.
    // Determines how much the path of the asteroid belt is affected by the gravity fields of stars.
    // Do not set to 0!
    public float resistanceToGravity = 1;
    // Keeps track of which star's gravity fields have already been accounted for.
    private readonly Dictionary<GameObject, bool> _stars = new Dictionary<GameObject, bool>();
    private Transform _parent;

    private void Start()
    {
        if (transform.parent.parent is null)
        {
            gameObject.GetComponent<AsteroidBeltPath>().enabled = false;
        }
        else
        {
            _parent = transform.parent;
            foreach (var go in FindObjectsOfType<GameObject>())
            {
                if (!go.tag.Contains("|Star|")) continue;
                try
                {
                    _stars.Add(go, false);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Stars being added to asteroid belt list multiple times!");
                }
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (var star in _stars.Keys)
        {
            if (Vector3.Distance(star.transform.position, transform.position) < star.GetComponent<Gravity>().gravityRadius 
                && !_stars[star])
            {
                _stars[star] = true;
                var vectorToStar = star.transform.position - _parent.position;
                RotateTowardsObject(
                    _parent.forward,
                    vectorToStar,
                    star.GetComponent<Gravity>().gravityStrength / resistanceToGravity);
            }
            else if (_stars[star] && 
                     Vector3.Distance(star.transform.position, transform.position) >= star.GetComponent<Gravity>().gravityRadius)
            {
                _stars[star] = false;
                RotateTowardsObject(
                    _parent.forward,
                    _parent.parent.forward,
                    star.GetComponent<Gravity>().gravityStrength / resistanceToGravity);
            }
            
        }
    }

    private void RotateTowardsObject(Vector3 from, Vector3 to, float max)
    {
        var newRotation = Vector3.RotateTowards(
            from,
            to,
            max,
            0.0f
        );
        _parent.rotation = Quaternion.LookRotation(newRotation);
    }
}
