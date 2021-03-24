using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    public Transform player;
    public Transform respawnPoint;

    public CharacterController controller;
    public AudioSource jumpSound;
    public AudioSource jumpLand;
    public AudioSource jumpLandOutOfBounds;

    public float walkSpeed = 12f;
    public float sprintSpeedMultiplier = 1.5f;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask OutOfBoundsMask;

    Vector3 velocity;
    bool isGrounded;
    bool isOutOfBounds;
    bool waitForTouchDown = true;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isOutOfBounds = Physics.CheckSphere(groundCheck.position, groundDistance, OutOfBoundsMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move = move.normalized;

        //Movement Speed
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed = walkSpeed * sprintSpeedMultiplier;
        }
        else
        {
            speed = walkSpeed;
        }
        controller.Move(move * speed * Time.deltaTime);


        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpSound.Play();
        }
        //Wait for landing 
        else if (!isGrounded && !isOutOfBounds)
        {
            waitForTouchDown = true;
        }
        //Landing on ground
        if (waitForTouchDown && isGrounded)
        {
            waitForTouchDown = false;
            jumpLand.Play();
        }
        //Out of bounds
        if (isOutOfBounds)
        {
            Debug.Log("OutOfBounds!");
            //Landing out of bounds
            if (waitForTouchDown)
            {
                waitForTouchDown = false;
                jumpLandOutOfBounds.Play();
            }
            this.transform.position = respawnPoint.position;
            Debug.Log("Player" + this.transform.position);
            Debug.Log("Respawn" + respawnPoint.position);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
