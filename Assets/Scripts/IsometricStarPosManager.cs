using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


/// <summary>
/// on enter the gravity core, immediately turn the player to the isometric starview
///
/// 
/// attach to the star gravity core
/// 1. on trigger enter find all the planet inside the star (start)
/// when player enter the orbit of a planet, notify this
/// 1. update->find the equation perpendicular to the plane/line, place the star pos to that pos
/// </summary>
public class IsometricStarPosManager : MonoBehaviour
{
    
    // TODO: change the naming to enter planet orbit
    // star view pos to planet view pos
    public Transform isoStarViewPos;
    public Transform player;
    public Transform camera;
    
    public float yDisToStar = 500.0f;
    public float xDisToStar = 90.0f;
    public float zDisToStar = 90.0f;

    private readonly float _dirMultiplierCam = 500.0f;
    private readonly float _dirMultiplierPlayer = 50.0f;
    
    private int _planetNum;
    public List<GameObject> planetList = new List<GameObject>();

    private Vector3 _starPos;
    private bool _isIsometricStarView;
    private bool _isExitingField;

    public static event Action<bool, Transform> OnIsometricStarView;

    private void Start()
    {
        if (!transform.parent.CompareTag("|Star|")) return;
        _starPos = transform.parent.position;
        print("Iso beginning parent -- "+ transform.parent.name);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (_isExitingField) return;
        
        // find all the planet inside the star
        if (other.gameObject.tag.Contains("|Planet|"))
        {
            _planetNum++;
            planetList.Add(other.gameObject);
            print(other.gameObject.name);
        } else if (other.gameObject.tag.Contains("|Player|"))
        {
            // notify camera to turn to isoview
            _isIsometricStarView = true;

            if (_planetNum == 0)
            {
                handleZeroPlanetIsoView();
            }
            
            OnIsometricStarView?.Invoke(_isIsometricStarView, transform.parent);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        _isExitingField = true;
        
        if (!other.gameObject.tag.Contains("|Player|")) return;
        print("player EXIT gravity field -- "+ transform.parent.name);
        _isIsometricStarView = false;
        OnIsometricStarView?.Invoke(_isIsometricStarView, transform.parent);
        
        Invoke(nameof(HandleGravityEdge), 2f);
    }

    /// <summary>
    /// prevent camera from panning repeatedly when going near the gravity field edge 
    /// 
    /// </summary>
    private void HandleGravityEdge()
    {
        _isExitingField = false;
    }
    
    private void handleZeroPlanetIsoView()
    {
        if (_planetNum != 0) return;
        print("planet is 0");
        var dir = new Vector3(xDisToStar, yDisToStar, zDisToStar);
        var isoIdealPos = _starPos + dir;
        isoStarViewPos.position = isoIdealPos;
        camera.GetComponent<ThirdPersonCamera>().isometricStarViewPos.position = isoIdealPos;
        
        print("iso position  " + isoIdealPos);
        // move player a little bit closer to the star when first enter iso view
        var dirA = (player.position - _starPos).normalized;
        player.GetComponent<ThirdPersonPlayer>().playerIsoStartPos = player.position - dirA * 50.0f;
    }

    private void Update()
    {
        if (!_isIsometricStarView) return;
        if (_planetNum == 0) return;
        print("planet is not 0");
        var planetOnePos = planetList[0].transform.position;
        var planetTwoPos = _planetNum == 1 ? new Vector3(
                planetOnePos.x + 50.0f, planetOnePos.y, planetOnePos.z + 50.0f) 
            : planetList[1].transform.position;
        calculatePerpendicularDir(planetOnePos, planetTwoPos);

    }

    private void calculatePerpendicularDir(Vector3 planetOnePos, Vector3 planetTwoPos)
    {
        var dirA = planetOnePos - _starPos;
        var dirB = planetTwoPos - _starPos;

        var normalDir = Vector3.Cross(dirA, dirB).normalized;

        var isoIdealPos = _starPos + normalDir * _dirMultiplierCam;
        isoStarViewPos.position = isoIdealPos;
        camera.GetComponent<ThirdPersonCamera>().isometricStarViewPos.position = isoIdealPos;
        
        // // TODO: figure out player orbit angle or change the planet positions
        // put the player on the plane
        var planeDir1  = Vector3.Cross(normalDir, dirB).normalized;
        var playerIsoStartPos = _starPos + planeDir1 * _dirMultiplierPlayer;
        player.GetComponent<ThirdPersonPlayer>().playerIsoStartPos = playerIsoStartPos;
        
    }
    
}

















