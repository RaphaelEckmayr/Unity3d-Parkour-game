using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float airSpeed = 0.2f;

    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform wallCheck;
    public float wallDistasnce = 1f;
    public LayerMask wallMask;
    public float walljumpVelocity = 150f;

    private bool isGrounded;
    private bool isWallrunning;
    private Vector3 lastmove;
    private Vector3 move;

    private bool isSliding;

    // Start is called before the first frame update
    void Start()
    {

    }

    // FPS Like Movement (Counter Strike Like)

    //void Update()
    //{
    //    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

    //    if (isGrounded && velocity.y < 0)
    //    {
    //        velocity.y = -1f;
    //    }

    //    float x = Input.GetAxis("Horizontal");
    //    float z = Input.GetAxis("Vertical");

    //    move = transform.right * x + transform.forward * z;

    //    controller.Move(move * speed * Time.deltaTime);

    //    if(Input.GetButtonDown("Jump") && isGrounded)
    //    {
    //        velocity.y = Mathf.Sqrt(jumpHeight * -1 * gravity);
    //    }

    //    velocity.y += gravity * Time.deltaTime;

    //    controller.Move(velocity * Time.deltaTime);
    //}

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isWallrunning = Physics.CheckSphere(wallCheck.position, wallDistasnce, wallMask);

        isSliding = Input.GetKeyDown(KeyCode.LeftShift);


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        if (isGrounded)
        {
            velocity.x = 0;
            velocity.z = 0;
            move = transform.right * x + transform.forward * z;
        }
        else if (isWallrunning)
        {
            move = transform.forward * z;
        }
        else if(isSliding)
        {
            move = lastmove;
        }
        else
        {
            move = lastmove + (transform.right * x * airSpeed) + (transform.forward * z * airSpeed);
        }

        controller.Move(move * speed * Time.deltaTime);

        

        if (isWallrunning)
        {
            if (velocity.y < 0)
            {
                velocity.y = -1f;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -1 * gravity);
            }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        lastmove = move;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isWallrunning && hit.normal.y < 0.1f)
        {
            if (Input.GetButton("Jump"))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);

                velocity.y = Mathf.Sqrt(jumpHeight * -1 * gravity);

                //Pfusch around
                velocity.x = hit.normal.x * walljumpVelocity;
                velocity.z = hit.normal.y * walljumpVelocity;

            }
        }
    }
}
