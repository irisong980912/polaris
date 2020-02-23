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

        Debug.Log("start gravity");

        try
        {
            var goList = new List<GameObject>();
            foreach (var o in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                var go = (GameObject) o;
                if (go.tag.Contains("|Player|"))
                {
                    goList.Add(go);
                    Debug.Log(go.name);
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
                GameObject core = GameObject.Find("Core");
                // keep the gravity force within a distance
                //Debug.Log(Vector3.Distance(core.transform.position, gravityObject.transform.position));
                // before: if (Vector3.Distance(transform.position, gravityObject.transform.position) > 0.8 * gravityRadius)
                if (Vector3.Distance(transform.position, gravityObject.transform.position) <= gravityRadius)
                {
                    //Debug.Log("Within Distance");
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
    
