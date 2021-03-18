using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Transform cameraTransform;
    public Rigidbody rb;
    //Variables

    //Camera
    //Axis
    private float x;
    private float y;
    //Sensitivity
    public float verticalSensitivity = 1f;
    public float horizontalSensitivity = 1f;

    //Rotations
    //Camera
    private Vector3 rotateValueCamera;
    //Body
    private Vector3 rotateValueBody;

    //Movement
    //Speed
    public float playerspeedForward = 10f;
    public float playerspeedSides = 10f;
    //Axis
    private float verticalMovement;
    private float horizontalMovement;
    //Jump
    private float jump;
    private float jumpHeight = 4000f;
    private float DeploymentHeight = 1.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Camera player

        //Axis
        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");

        //Rotate Values
        rotateValueCamera = new Vector3(x * horizontalSensitivity, 0, 0);
        rotateValueBody = new Vector3(0, -y * verticalSensitivity, 0);

        //Angles
        cameraTransform.eulerAngles = cameraTransform.eulerAngles - rotateValueCamera;
        transform.eulerAngles = transform.eulerAngles - rotateValueBody;



        //Movement player

        //Vertical
        verticalMovement = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.AddForce(transform.forward * Time.deltaTime * verticalMovement * playerspeedForward * 3);
        }
        else
        {
            rb.AddForce(transform.forward * Time.deltaTime * verticalMovement * playerspeedForward);
        }

        //Horizontal
        horizontalMovement = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.AddForce(transform.right * Time.deltaTime * horizontalMovement * playerspeedSides * 3);
        }
        else
        {
            rb.AddForce(transform.right * Time.deltaTime * horizontalMovement * playerspeedSides);
        }
        //Slow down
        if (CheckGroundStatus())
        {
            if (horizontalMovement == 0 && verticalMovement == 0)
            {
                rb.velocity = new Vector3
                (
                    rb.velocity.x * Time.deltaTime * 0.99f,
                    rb.velocity.y * Time.deltaTime,
                    rb.velocity.z * Time.deltaTime * 0.99f
                );
            }
        }
        

        //Jump
        jump = Input.GetAxis("Jump");
        if (CheckGroundStatus())
        {
            rb.AddForce(transform.up * jump * jumpHeight * Time.deltaTime);
        }

        

        

    }
    bool CheckGroundStatus()
    {
        RaycastHit hit;
        Ray landingRay = new Ray(transform.position, Vector3.down);
        bool canJump = false;
        if (Physics.Raycast(landingRay, out hit, DeploymentHeight))
        {
            if (hit.collider == null)
            {
                canJump = false;
            }
            else
            {
                canJump = true;
            }

        }

            return canJump;
    }
}