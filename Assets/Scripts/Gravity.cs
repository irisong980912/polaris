using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
@@ -11,7 +8,7 @@ public class Gravity : MonoBehaviour
    private GameObject[] _gravityObjects;
    public float gravityStrength;
    public float gravityRadius;
    

    private void Start()
    {
        try
@@ -24,6 +21,7 @@ private void Start()
                {
                    goList.Add(go);
                }

                _gravityObjects = goList.ToArray();
            }
        }
@@ -37,14 +35,30 @@ private void Update()
    {
        foreach (var gravityObject in _gravityObjects)
        {
            gravityObject.GetComponent<Rigidbody>().AddExplosionForce(
                -gravityStrength, 
                transform.position, 
                gravityRadius,
                0.0f,
                ForceMode.Force
                );
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
