using UnityEngine;

public sealed class MapCamera : MonoBehaviour
{
    public Transform mapTarget;
    public float mapHeight = 30f;
    public float mapDistance = 30f;
    public float mapAngle = 45f;

    // Start is called before the first frame update
    private void Start()
    {
        HandleCamera();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleCamera();
    }

    private void HandleCamera()
    {
        //Exit if no target
        if(!mapTarget)
        {
            return;
        }

        //Build world position vector
        var worldPosition = (Vector3.forward * -mapDistance) + (Vector3.up * mapHeight);

        //Build rotated vector
        var rotatedVector = Quaternion.AngleAxis(mapAngle, Vector3.up) * worldPosition;

        //Move the position
        var flatTargetPosition = mapTarget.position;
        flatTargetPosition.y = 0f;
        var finalPosition = flatTargetPosition + rotatedVector;

        transform.position = finalPosition;

        //Let camera look at target
        transform.LookAt(flatTargetPosition);
    }
}
