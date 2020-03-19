using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// when hover, 
public class DrawPlanetToStarBeam : MonoBehaviour
{
    private LineRenderer beam;
    private float counter;
    private float dist;

    public Transform player;

    public Transform curPlanet;
    public Transform destinStar;

    public float beamDrawSpeed;
    public float normalBeamWidth = 30.0f;
    //public float mapClearBeamWidth = 20.0f;

    private bool _levelClear;
    private bool _connectBeam;
    private bool _draw;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent;
        StarIconManager.OnHoverStart += OnHoverStart;
        StarIconManager.OnHoverStop += OnHoverStop;
        StarIconManager.OnSelectStar += OnSelectStar;
        _connectBeam = false;

    }

    private void OnSelectStar(Transform obj)
    {
        _draw = false;
    }
    
    private void OnHoverStart(Transform targetStar)
    {
        print("on hover star -- draw planet to star beam");
        print(targetStar.name);

        destinStar = targetStar;
        // planet core parent
        if (!player.parent.parent.CompareTag("|Planet|")) return;
        print("on hover star -- draw planet to star beam -- find planet");
        curPlanet = player.parent.parent;
        
        beam = gameObject.GetComponent<LineRenderer>();
        beam.startWidth = normalBeamWidth;
        beam.endWidth = normalBeamWidth;

        _connectBeam = true;

        _draw = true;

    }
    
    private void OnHoverStop(Transform targetStar)
    {
        print("beam -- OnHoverStop");
        _draw = false;
      

    }
    
    
    // Update is called once per frame
    private void Update()
    {

        if (!_connectBeam) return;
        print("animate beam for planet and star");
        DrawBeam();
    
    }
    
    void DrawBeam()
    {
        Vector3 pointA = curPlanet.position;
        Vector3 pointB = destinStar.position;
        
        
        beam.SetPosition(0, curPlanet.position);
        beam.SetPosition(1, curPlanet.position);
    
        dist = Vector3.Distance(curPlanet.position, destinStar.position);

    
        print("draw conse");
        if (counter < dist && _draw)
        {
            counter += 0.1f / beamDrawSpeed;
    
            float x = Mathf.Lerp(0, dist, counter);
    
            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
            beam.SetPosition(1, pointAlongLine);
        }
        else if (counter != 0 && !_draw)
        {
    
            counter -= 0.1f / beamDrawSpeed;
    
            float x = Mathf.Lerp(0, dist, counter);
    
            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
            beam.SetPosition(1, pointAlongLine);
        }
    }
}
