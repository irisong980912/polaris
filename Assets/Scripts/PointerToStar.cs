using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

/// <summary>
/// attach to an arrow icon image object under movableCanvas
/// 
/// 1. determine which stars to point to
/// 2. have each star owning the canvas and the pointer image
/// 3. make the image follow the star locations (lookat)
/// 4. make those image the children of the canvas
/// 5. make the cursor stay within the field of view
///
/// 6. when selecting the cursor, show a live image of the star
///  within what diameter of distance?
/// need to build a tree and then breath first search the closest stars ????
/// simply, the 2 most adjacent stars
/// </summary>
public class PointerToStar : MonoBehaviour
{
    public GameObject StarToPoint; 
    private bool _OnIsometricStarView;
    public Camera cam;
    public float imageWidth; 

    public float disToClipPanel = 0f;

    public float rotateSpeed = 2.0f;

    private void Start()
    {

        // TODO: need to listen to the event of isometric cam. 
        IsometricStarView.OnIsometricStarView += OnIsometricStarView;
        GetComponent<Image>().enabled = false;
        imageWidth = (float) (GetComponent<RectTransform>().rect.width * 0.5);
        //GetComponent<Image>().enabled = true;

    }
    
    
    private void OnIsometricStarView(bool OnIsometricStarView)
    {
        _OnIsometricStarView = OnIsometricStarView;
        GetComponent<Image>().enabled = true;
        
        print("star to point: " + StarToPoint.name);


    }

    private void testPos()
    {
        var p = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        transform.position = p;
        transform.LookAt(cam.transform.position, -Vector3.up);
        // transform.LookAt(cam.transform);
    }
 
    

    private void Update()
    {

        // find the relative position of the camera to the star
        
        if (!StarToPoint) return;
        if (!_OnIsometricStarView) return;
        // handle pointing position 
        var starPos = StarToPoint.transform.position;
        var camPos = cam.transform.position;
        var xDis = starPos.x - camPos.x;
        var zDis = starPos.z - camPos.z;
        
        // print("---------------mmmmmmm");
        // print(StarToPoint.name);
        // print(new Vector3(xDis, 0, zDis).normalized);

        xDis = Math.Abs(xDis);
        zDis = Math.Abs(zDis);
        
        var dis3D = new Vector3(xDis, 0, zDis).normalized;
        

        if (dis3D.x < dis3D.z)
        {
            if ((1 - dis3D.z) < dis3D.x) // z is closer to edge
            {
                dis3D.z = 1 - imageWidth;
            } else if ((1 - dis3D.z) >= dis3D.x)
            {
                dis3D.x = 0 + imageWidth;
            }
        }
        else if (dis3D.x >= dis3D.z)
        {
            if ((1 - dis3D.x) < dis3D.z) // z is closer to edge
            {
                dis3D.x = 1 - imageWidth;
            } else if ((1 - dis3D.x) >= dis3D.z)
            {
                dis3D.z = 0 + imageWidth;
            }
        }
        
        // print("---------------xs");
        // print(StarToPoint.name);
        // print(dis3D);
            
        //due to the rotation of the camera, world space z is camera x, world space x is camera y
        var viewportPos = cam.ViewportToWorldPoint(new Vector3(dis3D.z, dis3D.x, 
                                                        cam.nearClipPlane + disToClipPanel));
        transform.position = viewportPos;
        // transform.LookAt(cam.transform);
        transform.rotation = cam.transform.rotation;
        transform.LookAt(cam.transform.position, -Vector3.up);
        
        
        // handle pointing rotation
        var starPosViewport = cam.ScreenToWorldPoint(starPos);
        
        // TODO： refine the rotation direction of the pointer
        
        // var delta = starPos - transform.position;
        // delta.y = 0;
        // var rotation = Quaternion.LookRotation(delta);
        // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
        //
        
        // transform.Rotate(starPosViewport.x, 0,starPosViewport.z,Space.World);
        // TODO： ---- refine rotation Method 2 ----
        // Get the current euler angle in absolute terms
        // Vector3 curRotateAngle = transform.localEulerAngles;
        //
        // // now modify that euler angle, creating a new absolute euler angle
        // curRotateAngle.x += delta.x;
        // curRotateAngle.y = 0;
        // curRotateAngle.z += delta.z;
        // transform.localEulerAngles = curRotateAngle;
        //
        // transform.LookAt(StarToPoint.transform.position, -Vector3.left);
        
        // TODO： ---- refine rotation method 1 ----
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, starPosViewport - transform.position);

    }
    
    // TODO: make the player use stick to choose which star to go to. 
    // TODO: upon selection, show the live frontal images. 
    // TODO: allow user to be shot to the direction of the star. 
}