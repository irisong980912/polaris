using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    // Update is called once per frame

    private GameObject[] _gravityObjects;
    public float gravityStrength;
    public float gravityRadius;

    private void Start()
    {

        try
        {
            var goList = new List<GameObject>();
            foreach (var o in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                var go = (GameObject) o;
                if (go.tag.Contains("|GravityObject|"))
                {
                    goList.Add(go);
                }

                _gravityObjects = goList.ToArray();
            }
        }
        catch (UnityException)
        {
            print("No such tag");
        }
    }

    private void Update()
    {
        
        foreach (var gravityObject in _gravityObjects)
        {
            if (gameObject.tag.Contains("|Star|"))
            {
                if (Vector3.Distance(transform.position, gravityObject.transform.position) > 0.8 * gravityRadius)
                {
                    ApplyGravity(gravityObject);
                }
            }
            else
            {
                ApplyGravity(gravityObject);
            }
            
        }
    }

    private void ApplyGravity(GameObject gravityObject)
    {


        gravityObject.GetComponent<Rigidbody>().AddExplosionForce(
            -gravityStrength,
            transform.position,
            gravityRadius,
            0.0f,
            ForceMode.Force
        );
    }
}
    
