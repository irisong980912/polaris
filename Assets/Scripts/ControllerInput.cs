using UnityEngine;

public class ControllerInput : MonoBehaviour
{

    public string[] controllers;

    float xDpadAxisInput;
    float yDpadAxisInput;

    float xLAxisInput;
    float yLAxisInput;

    float xRAxisInput;
    float yRAxisInput;

    void Start()
    {

    }
    
    void Update()
    {
        controllers = Input.GetJoystickNames();
  
       //Prints Joystick values

        if (controllers[0] == "")
        {

        }

        xDpadAxisInput = Input.GetAxisRaw("XBOXDpadHorizontal");
        yDpadAxisInput = Input.GetAxisRaw("XBOXDpadVertical");

        xLAxisInput = Input.GetAxisRaw("Horizontal");
        yLAxisInput = Input.GetAxisRaw("Vertical");

        xRAxisInput = Input.GetAxisRaw("rHorizontal");
        yRAxisInput = Input.GetAxisRaw("rVertical");

        Debug.Log("Dpad: " + xDpadAxisInput + ", " + yDpadAxisInput);
        Debug.Log("JoystickL: " + xLAxisInput + ", " + yLAxisInput);
        Debug.Log("JoystickR: " + xRAxisInput + ", " + yRAxisInput);
    }
}