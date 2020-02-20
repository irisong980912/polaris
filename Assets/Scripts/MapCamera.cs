using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Transform m_Target;
    public float m_Height = 30f;
    public float m_Distance = 30f;
    public float m_Angle = 45f;

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
        if(!m_Target)
        {
            return;
        }

        //Build world position vector
        Vector3 worldPosition = (Vector3.forward * -m_Distance) + (Vector3.up * m_Height);

        //Build rotated vector
        Vector3 rotatedVector = Quaternion.AngleAxis(m_Angle, Vector3.up) * worldPosition;

        //Move the position
        Vector3 flatTargetPosition = m_Target.position;
        flatTargetPosition.y = 0f;
        Vector3 finalPosition = flatTargetPosition + rotatedVector;

        transform.position = finalPosition;

        //Let camera look at target
        transform.LookAt(flatTargetPosition);
    }
}
