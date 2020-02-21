using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Transform mapTarget;
    public float mapHeight = 30f;
    public float mapDistance = 30f;
    public float mapAngle = 45f;

    // Start is called before the first frame update
    void Start()
    {
        HandleCamera();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCamera();
    }

    protected virtual void HandleCamera()
    {
        //Exit if no target
        if(!mapTarget)
        {
            return;
        }

        //Build world position vector
        Vector3 worldPosition = (Vector3.forward * -mapDistance) + (Vector3.up * mapHeight);

        //Build rotated vector
        Vector3 rotatedVector = Quaternion.AngleAxis(mapAngle, Vector3.up) * worldPosition;

        //Move the position
        Vector3 flatTargetPosition = mapTarget.position;
        flatTargetPosition.y = 0f;
        Vector3 finalPosition = flatTargetPosition + rotatedVector;

        transform.position = finalPosition;

        //Let camera look at target
        transform.LookAt(flatTargetPosition);
    }
}
