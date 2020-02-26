using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBeam : MonoBehaviour
{
    private LineRenderer beam;
    private float counter;
    private float dist;

    public Transform originStar;
    public Transform destinStar;

    public float beamDrawSpeed;

    public bool starOriginActivated;
    public bool starDestinActivated;
    public bool triggerBeam;

    // Start is called before the first frame update
    void Start()
    {
        beam = gameObject.GetComponent<LineRenderer>();
        //beam.GetComponent<LineRenderer>();
        beam.SetPosition(0, originStar.position);
        beam.SetPosition(1, originStar.position);

        dist = Vector3.Distance(originStar.position, destinStar.position);

    }

    // Update is called once per frame
    void Update()
    {
        checkStarsActivation();
        DrawConstellation();
    }

    void checkStarsActivation()
    {
        if(starOriginActivated && starDestinActivated)
        {
            triggerBeam = true;
        } else
        {
            triggerBeam = false;
        }
    }

    void DrawConstellation()
    {
        if (counter < dist && triggerBeam)
        {
            counter += 0.1f / beamDrawSpeed;

            float x = Mathf.Lerp(0, dist, counter);

            Vector3 pointA = originStar.position;
            Vector3 pointB = destinStar.position;

            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
            beam.SetPosition(1, pointAlongLine);
        }
        else if (counter != 0 && !triggerBeam)
        {
            counter -= 0.1f / beamDrawSpeed;

            float x = Mathf.Lerp(0, dist, counter);

            Vector3 pointA = originStar.position;
            Vector3 pointB = destinStar.position;

            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
            beam.SetPosition(1, pointAlongLine);
        }
    }
}
