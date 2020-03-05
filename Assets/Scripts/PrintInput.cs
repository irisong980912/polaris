using UnityEngine;

public class PrintInput : MonoBehaviour
{
    float xDpadAxisInput;
    float yDpadAxisInput;

    float xAxisInput;
    float yAxisInput;

    void Start()
    {
        // Prints a joystick name if movement is detected.
        xDpadAxisInput = Input.GetAxisRaw("DpadHorizontal");
        yDpadAxisInput = Input.GetAxisRaw("DpadVertical");

        xAxisInput = Input.GetAxisRaw("Horizontal");
        yAxisInput = Input.GetAxisRaw("Vertical");
    }
    
    void Update()
    {
        // Prints a joystick name if movement is detected.
        xDpadAxisInput = Input.GetAxisRaw("DpadHorizontal");
        yDpadAxisInput = Input.GetAxisRaw("DpadVertical");

        xAxisInput = Input.GetAxisRaw("Horizontal");
        yAxisInput = Input.GetAxisRaw("Vertical");

        Debug.Log("Dpad: " + xDpadAxisInput + ", " + yDpadAxisInput);
        Debug.Log("Joystick: " + xAxisInput + ", " + yAxisInput);
    }
}