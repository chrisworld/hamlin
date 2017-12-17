using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float turnSmoothTime = 0.2f;
    public float speedSmoothTime = 0.1f;

    float turnSmoothVelocity;
    float speedSmoothVelocity;
    float currentSpeed;
    Animator animator;
    Transform cameraT;

	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
        cameraT = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {

        //TODO: why are we using raw?
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDirection = input.normalized;

        //calculates rotation for player as arctan(x / y)
        //player facing forwards = 0 deg rotation; facing right = 90 deg rotation, etc 
        if (inputDirection != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        //running activated by pressing left shift; change this key code to use a different key
        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = (running ? runSpeed : walkSpeed) * inputDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        //this is how we tell the animation controller which state we're in
        float animationSpeedPercent = (running ? 1f : 0.5f) * inputDirection.magnitude;
        //Debug.Log("setting animator var to: " + animationSpeedPercent);    //don't think this is working properly
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

    }
}
