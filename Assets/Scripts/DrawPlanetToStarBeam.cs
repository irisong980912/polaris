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
    private bool _endDraw;

    // Start is called before the first frame update
    private void Start()
    {
        StarIconManager.OnHoverStart += OnHoverStart;
        StarIconManager.OnHoverStop += OnHoverStop;
        StarIconManager.OnSelectStar += OnSelectStar;
        _connectBeam = false;
        player = transform.parent;

    }

    private void OnSelectStar(Transform obj)
    {
        _draw = false;
        _endDraw = true;
    }
    
    private void OnHoverStart(Transform targetStar)
    {
        _endDraw = false;
        print("on hover star -- draw planet to star beam");
        print(targetStar.name);

        destinStar = targetStar;
        // planet core parent

        player = transform.parent;
        if (!player.parent) return;
        
        print("on hover star --" + player.parent.parent.name);
        
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
        DrawBeam();
    
    }

    private void DrawBeam()
    {
        var pointA = curPlanet.position;
        var pointB = destinStar.position;
        
        
        beam.SetPosition(0, curPlanet.position);
        beam.SetPosition(1, curPlanet.position);
    
        dist = Vector3.Distance(curPlanet.position, destinStar.position);
        
        if (counter < dist && _draw)
        {
            counter += 0.1f / beamDrawSpeed;
    
            var x = Mathf.Lerp(0, dist, counter);
    
            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
            beam.SetPosition(1, pointAlongLine);
        }
        else if ((counter != 0 && !_draw) || (counter != 0 &&_endDraw))
        {
    
            counter -= 0.1f / beamDrawSpeed;
    
            var x = Mathf.Lerp(0, dist, counter);
    
            var pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
            beam.SetPosition(1, pointAlongLine);
        }
    }

    private void OnDisable()
    {
        //Prevent event from looking for prescribed object that is removed on Reload of scene, by unsubscribing.
        StarIconManager.OnHoverStart -= OnHoverStart;
        StarIconManager.OnHoverStop -= OnHoverStop;
        StarIconManager.OnSelectStar -= OnSelectStar;
    }
}
