using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

// MonoBehaviour.OnTriggerEnter example.
//
// A cube is based in the center of the world. A small white sphere is added
// to the world and it can move anywhere in the Scene. The cube changes
// color each time the sphere enters the cube. As the sphere leaves the cube,
// the cube reverts back to white.

public class TransparentCollider : MonoBehaviour
{
    private float moveSpeed = 3.0f;
    private GameObject sphere;

    void Awake()
    {
        // Create the ground.
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Quad);
        ground.transform.rotation = Quaternion.Euler(90, 0, 0);
        ground.transform.localScale = new Vector3(6, 6, 6);
        ground.transform.position = new Vector3(0, -0.5f, 0);

        // Make sure the BoxCollider is a trigger.
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        // Create a sphere to interact with this cube.
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.gameObject.transform.position = new Vector3(1f, 0, 0);
        sphere.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        // Add a Rigidbody to the sphere.
        // The sphere does not touch the ground.
        sphere.gameObject.AddComponent<Rigidbody>();
        sphere.gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    void Update()
    {
        // Move the sphere based on a/d and s/w.
        Transform sphereTransform = sphere.gameObject.transform;
        sphereTransform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * moveSpeed);
        sphereTransform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * moveSpeed);

        LimitSphere(sphereTransform);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Change the cube color to green.
        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        meshRend.material.color = Color.green;
        Debug.Log(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        // Revert the cube color to white.
        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        meshRend.material.color = Color.white;
    }

    // Keep sphere inside a xz square of 2 units.
    private void LimitSphere(Transform sphereTransform)
    {
        if (sphereTransform.position.x < -1.0f)
        {
            sphereTransform.position = new Vector3(-1.0f, 0.0f, sphereTransform.position.z);
        }

        if (sphereTransform.position.x > 1.0f)
        {
            sphereTransform.position = new Vector3(1.0f, 0.0f, sphereTransform.position.z);
        }

        if (sphereTransform.position.z < -1.0f)
        {
            sphereTransform.position = new Vector3(sphereTransform.position.x, 0.0f, -1.0f);
        }

        if (sphereTransform.position.z > 1.0f)
        {
            sphereTransform.position = new Vector3(sphereTransform.position.x, 0.0f, 1.0f);
        }
    }
}