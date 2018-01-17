using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float walkSpeed = 1;
    public float runSpeed = 3;
    public float turnSmoothTime = 0.2f;
    public float speedSmoothTime = 0.1f;
    public float gravity = -12;
    public float jumpHeight = 0.5f;
    public float inAirControl = 1;          //controls how much the player can turn while in mid air

    private bool move_activated = true;

    float turnSmoothVelocity;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    Animator animator;
    Transform cameraT;
    CharacterController controller;

	void Start () {
        animator = GetComponentInChildren<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        move_activated = true;
    }
	
	void Update () {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDirection = input.normalized;

        if (checkValidJumpKey())
        {
          Jump();
        }


        if (move_activated){
         
          //calculates rotation for player as arctan(x / y)
          //player facing forwards = 0 deg rotation; facing right = 90 deg rotation, etc 
          if (inputDirection != Vector2.zero)
          {
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
          }

          if (move_activated)
          {
            bool running = checkValidRunKey();
            float targetSpeed = (running ? runSpeed : walkSpeed) * inputDirection.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

            velocityY += Time.deltaTime * gravity;
            Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

            controller.Move(velocity * Time.deltaTime);
            currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

            if (controller.isGrounded)
            {
              velocityY = 0;
            }

            //this is how we tell the animation controller which state we're in
            float animationSpeedPercent = (running ? currentSpeed / runSpeed : currentSpeed / walkSpeed * 0.5f);
            animator.SetFloat("speedPercent", animationSpeedPercent, GetModifiedSmoothTime(speedSmoothTime), Time.deltaTime);
          }
        
        }

        
    }
    
    //should work even when movement disabled
    void Jump()
    {
        if (controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    //not convinced this is really that useful - may delete later
    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded) return smoothTime;
        else if (inAirControl == 0) return float.MaxValue;
        else return (smoothTime / inAirControl);

    }

    // set move activate
    public void setMoveActivate(bool state){
        move_activated = state;
    }

    // get move activate
    public bool getMoveActivate(){
        return move_activated;
    }

    // check valid jump keys
    public bool checkValidJumpKey(){
        KeyCode[] valid_keys = {
            KeyCode.Space,
            KeyCode.Keypad0,
            KeyCode.RightControl
        };
        // check valid key
        foreach (KeyCode key in valid_keys){
            if(Input.GetKeyDown(key)){ 
                return true;
            }
        }
        return false;
    }

    // check valid run keys
    public bool checkValidRunKey(){
        KeyCode[] valid_keys = {
            KeyCode.LeftShift,
            KeyCode.Keypad1,
            KeyCode.RightShift
        };
        // check valid key
        foreach (KeyCode key in valid_keys){
            if(Input.GetKey(key)){ 
                return true;
            }
        }
        return false;
    }

}
