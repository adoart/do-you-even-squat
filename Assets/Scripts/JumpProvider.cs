using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class JumpProvider : MonoBehaviour
{
    // public variables
    public float jumpForce = 30.0f;
    public float forwardForce = 3.0f;
    public float gravity = 9.81f;
    public Transform forwardSource;

    public bool grounded;
    private Rigidbody myBody;
    private Vector3 originalPosition;
    public List<LocomotionProvider> locomotionProviders;

    private List<InputDevice> inputDevices = new List<InputDevice>();
    public bool isPressed = false;

    // Start is called before the first frame update
    void Start() {
        InputDevices.GetDevices(inputDevices);

        myBody = gameObject.GetComponent<Rigidbody>();
        originalPosition = new Vector3(0.0f, 3.0f, 0.0f);
        grounded = false;
    }

    // Update is called once per frame
    void Update() {
        // Determine how much should move in the z-direction
        Vector3 movementZ = transform.forward * forwardForce;
        Vector3 movementY = transform.up * jumpForce;
        Vector3 forceVector = movementY + movementZ;

        Jump(forceVector);

        //Debug.DrawLine(forwardSource.localPosition, Vector3.up, Color.yellow);
        Debug.DrawLine(transform.position, forceVector, Color.red);
        //Debug.DrawLine(forwardSource.localPosition, movementZ, Color.blue);

    }

    private void Jump(Vector3 forceVector) {

        foreach (InputDevice inputDevice in inputDevices) {
            if (inputDevice.isValid) {

                InputHelpers.IsPressed(inputDevice, InputHelpers.Button.PrimaryButton, out isPressed);
                if (grounded && isPressed) {
                    Debug.Log("JUMP");
                    myBody.isKinematic = false;
                    foreach (LocomotionProvider locomotionProvider in locomotionProviders) {
                        locomotionProvider.enabled = false;
                    }

                    myBody.AddForce(forceVector, ForceMode.Impulse);
                    grounded = false;
                }

                //if vertical velocity is 0 we're grounded and we can jump again
                if (myBody.IsSleeping()) {
                    grounded = true;
                    myBody.isKinematic = true;
                    foreach (LocomotionProvider locomotionProvider in locomotionProviders) {
                        locomotionProvider.enabled = true;
                    }
                }

                //check if below the map, reset position
                if (transform.position.y < -5.0f) {
                    transform.position = originalPosition;
                    grounded = true;
                }
            }
        }
    }
}
