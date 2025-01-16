using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float turnSpeed = 45.0f;
    [SerializeField] float rpm;
    [SerializeField] float horsePower = 0;
    private float horizontalInput;
    private float verticalInput;
    public Camera mainCamera;
    public Camera hoodCamera;
    public KeyCode switchKey;
    public string inputID;
    private Rigidbody playerRb;
    [SerializeField] TextMeshProUGUI speedometerText;
    [SerializeField] TextMeshProUGUI rpmText;
    [SerializeField] List<WheelCollider> allWheels;
    [SerializeField] int wheelsOnGround;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            mainCamera.enabled = !mainCamera.enabled;
            hoodCamera.enabled = !hoodCamera.enabled;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal" + inputID);
        verticalInput = Input.GetAxis("Vertical" + inputID);


        if (isOnGround())
        {
            //Move the vehicle forward
            playerRb.AddRelativeForce(Vector3.forward * verticalInput * horsePower);
            //Rotates the car based on horizontal input
            transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

            //Looking back at this project after 2 years, I realize how bad these calculations are. They are based off the speed of the car and stop updating when the car is not upright
            //Would need to take the speed at which the tires would be rotating to calculate speed and RPMS, but these models do not have tires that would function properly with such rotations
            speed = Mathf.Round(playerRb.velocity.magnitude * 2.237f);
            speedometerText.text = "Speed: " + speed + "mph";

            rpm = Mathf.RoundToInt((speed % 30) * 40);
            rpmText.text = "RPM: " + rpm;
        }
    }

    bool isOnGround()
    {
        wheelsOnGround = 0;
        foreach(WheelCollider wheel in allWheels)
        {
            if (wheel.isGrounded)
            {
                wheelsOnGround++;
            }
        }

        if (wheelsOnGround == 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
