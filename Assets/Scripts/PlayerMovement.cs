using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("FMOD Var")]
    public EventReference inputsound;
    FMOD.Studio.EventInstance landingStepsEvent;
    private int materialValue;
    private RaycastHit materialCheck;
    public AudioManager AM;

    bool playerisMoving;
    private float walkingspeed = 0.6f;
    private float runningspeed = 0.4f;

    [Header("Movement")]
    private float moveSpeed = 7f;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyTojump;
    public bool canmove;

    public Movement moveScript;
    public Text speedMeter;

    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    bool landing;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Vector3 stop;
    Rigidbody rb;

    void Start()
    {
        landingStepsEvent = FMODUnity.RuntimeManager.CreateInstance(inputsound);

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = readyTojump = canmove = true;
        landing = false;
        
        InvokeRepeating("WalkEvent", 0, walkingspeed);
        InvokeRepeating("RunEvent", 0, runningspeed);
    }
    private void FixedUpdate()
    {
        if (canmove)
        {
            MovePlayer();
            SpeedUI();
        }
        
    }

    void Update()
    {
        //check ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.8f, whatIsGround);
        
        MyInput();
        SpeedControl();

        //handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
           
            //check if player moving
            if (rb.velocity.x >= 0.01f || rb.velocity.x <= -0.01f || rb.velocity.y >= 0.01f || rb.velocity.y <= -0.01f || rb.velocity.z >= 0.01f || rb.velocity.z <= -0.01f)
            {
                playerisMoving = true;
            }
            else //if (rb.velocity.normalized == Vector3.zero)
            {
                playerisMoving = false;
            }

        }
        else
        {
            rb.drag = 0;
            playerisMoving = false;
            landing = true;
        }
       
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if (Input.GetKey(jumpkey) && readyTojump && grounded)
        {
            readyTojump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
       
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
       
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

      
    }

    public void Jump()//Player Jump
    {
        if (canmove)
        {
            //reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            landingStepsEvent.setParameterByName("Terrain", materialValue);
            landingStepsEvent.setParameterByName("WalkToRun", 0);
            landingStepsEvent.start();

            //ready to land
            landing = true;
        }

    }

    private void ResetJump()//reset jump if player landing, player can jump again
    {
        readyTojump = true;
    }

    public void PauseMove()// went game pause disabel movement
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canmove)
        {
            moveScript.enabled = false;
            canmove = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !canmove)
        {
            moveScript.enabled = true;
            canmove = true;
        }
    }
    public void SpeedUI()
    {
        int intvalue = (int)rb.velocity.magnitude;
        speedMeter.text = "Speed: " + intvalue.ToString();
    }
    //Audio Movement, Trigger, and Material Detection
    void WalkEvent()
    {
        if (playerisMoving == true && !Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 7f;
            MaterialCheck();

            landingStepsEvent.setParameterByName("Terrain", materialValue);
            landingStepsEvent.setParameterByName("WalkToRun", 0);

            landingStepsEvent.start();
         
        }
      
    }

    void RunEvent()
    {
        if (playerisMoving == true && Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 10f;
            MaterialCheck();

            landingStepsEvent.setParameterByName("Terrain", materialValue);
            landingStepsEvent.setParameterByName("WalkToRun", 1);

            landingStepsEvent.start();
            
        }
    }
    void MaterialCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out materialCheck, playerHeight * 0.5f + 0.8f))
        {
            switch (materialCheck.collider.tag)
            {
                case "Floor_Normal":
                    materialValue = 0;
                    break;
                case "FootSteps_Water_Thin":
                    materialValue = 1;
                    break;
                case "Street":
                    materialValue = 2;
                    break;
                case "Grass":
                    materialValue = 3;
                    break;
                case "Dirt":
                    materialValue = 4;
                    break;
                default:
                    materialValue = 0;
                    break;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)//landing sound trigger
    {
        if (collision.gameObject.tag == "Floor_Normal" && landing)
        {
            materialValue = 0;
            landingStepsEvent.setParameterByName("Terrain", materialValue);
            landingStepsEvent.setParameterByName("WalkToRun", 0);
            landingStepsEvent.start();
           
            landing = false;
        }
        if (collision.gameObject.tag == "FootSteps_Water_Thin" && landing)
        {
            materialValue = 1;
            landingStepsEvent.setParameterByName("Terrain", materialValue);
            landingStepsEvent.setParameterByName("WalkToRun", 0);
            landingStepsEvent.start();
           
            landing = false;
        }
        if (collision.gameObject.tag == "Street" && landing)
        {
            materialValue = 2;
            landingStepsEvent.setParameterByName("Terrain", materialValue);
            landingStepsEvent.setParameterByName("WalkToRun", 0);
            landingStepsEvent.start();

            landing = false;
        }
        if (collision.gameObject.tag == "Grass" && landing)
        {
            materialValue = 3;
            landingStepsEvent.setParameterByName("Terrain", materialValue);
            landingStepsEvent.setParameterByName("WalkToRun", 0);
            landingStepsEvent.start();

            landing = false;
        }
        if (collision.gameObject.tag == "Dirt" && landing)
        {
            materialValue = 4;
            landingStepsEvent.setParameterByName("Terrain", materialValue);
            landingStepsEvent.setParameterByName("WalkToRun", 0);
            landingStepsEvent.start();

            landing = false;
        }
    }
    //end of Audio
  

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "JumpPad")
        {
            rb.AddForce(transform.up * 35, ForceMode.Impulse);
        }

        if (other.gameObject.tag == "Tunnel_Enter")
        {
            AM.reverbTunnelIns.start();
        }
        if (other.gameObject.tag == "Tunnel_Exit")
        {
            AM.reverbTunnelIns.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        if (other.gameObject.tag == "House_Enter")
        {
            AM.reverbHouselIns.start();
        }
        if (other.gameObject.tag == "House_Exit")
        {
            AM.reverbHouselIns.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

    }
   

}
