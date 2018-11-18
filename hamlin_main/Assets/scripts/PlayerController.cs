using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
  // public
  public Transform player;
  public Transform hud;
  public Text ScaleText;
  public Text BaseKeyText;
  public SoundPlayer sound_player;
  public InputHandler input_handler;

  public float walkSpeed = 1;
  public float runSpeed = 3;
  public float turnSmoothTime = 0.2f;
  public float speedSmoothTime = 0.1f;
  public float gravity = -12;
  public float jumpHeight = 0.5f;
  public float inAirControl = 1;          //controls how much the player can turn while in mid air

  [HideInInspector]
  public bool hold_flute = false;
  [HideInInspector]
  public bool inDistance = false;
  [HideInInspector]
  public bool play_mode = false;
  [HideInInspector]
  public bool forceActivateCombat = false;    //used by Monk.cs,  **do not remove**

  public bool move_activated = true;
  // private
  private bool switch_model = false;
  private bool walk = false;
  private bool run = false;
  // anim hashes
  //private int idle_hash = Animator.StringToHash("Base Layer.idle");
  //private int cont_play_hash = Animator.StringToHash("Base Layer.hamlin_cont_play");

  float turnSmoothVelocity;
  float speedSmoothVelocity;
  float currentSpeed;
  float velocityY;

  Animator anim;
  Transform cameraT;
  CharacterController controller;

  // start
  void Start()
  {
    anim = GetComponentInChildren<Animator>();
    cameraT = Camera.main.transform;
    controller = GetComponent<CharacterController>();
    move_activated = true;
    if (player == null)
    {
      player = GameObject.Find("Player").GetComponent<Transform>();
    }
    if (input_handler == null)
    {
      input_handler = GameObject.Find("Player").GetComponent<InputHandler>();
    }
    if (hud == null)
    {
      hud = GameObject.Find("HUDCanvas").GetComponent<Transform>();
    }
    if (ScaleText == null)
    {
      ScaleText = GameObject.Find("HUDCanvas/Note/ScaleText").GetComponent<Text>();
    }
    if (BaseKeyText == null)
    {
      BaseKeyText = GameObject.Find("HUDCanvas/Note/BaseKey").GetComponent<Text>();
    }
    hud.transform.GetChild(1).gameObject.SetActive(false);
  }

  // update
  void Update()
  {

    // switch the model
    if (switch_model) switchModel();

    // take the flute and put it back
    else if (checkValidTakeFluteKey() || forceActivateCombat)
    {
      sound_player.hamlin_idle.Stop();
      if(hold_flute) sound_player.hamlin_flute_in.Play();
      forceActivateCombat = false;
      anim.SetTrigger("takeFlute");
      switch_model = true;
    }

    // sound should stop in play mode
    if (play_mode)
    {
      // stop sound of walking
      if (run || walk){
        sound_player.hamlin_walk.Stop();
        sound_player.hamlin_run.Stop();
        run = false;
        walk = false;
      }
    }
  }

  // change text
  public void changeScaleText(string text)
  {
    ScaleText.text = text;
  }
  public void changeBaseKeyText(string text)
  {
    BaseKeyText.text = text;
  }

  // play the flute
  public void playNote()
  {
    anim.SetTrigger("notePlay");
  }

  // play the flute
  public void getAttacked()
  {
    anim.SetTrigger("getAttacked");
    sound_player.hamlin_hurt.Play();
  }

  // enter play mode
  public void enterPlayMode()
  {
    if (!play_mode)
    {
      anim.SetTrigger("startPlay");
      play_mode = true;
      sound_player.inPlay = true;
      move_activated = false;
      inDistance = false;
      sound_player.hamlin_idle.Stop();
    }
  }

  // exit play mode
  public void exitPlayMode()
  {
    if (play_mode)
    {
      anim.SetTrigger("stopPlay");
      play_mode = false;
      sound_player.inPlay = false;
      move_activated = true;
    }
  }

  // Put back flute
  public void putAwayFlute()
  {
    anim.SetTrigger("takeFlute");
    switch_model = true;
  }


  // switch the model hamlin -> hamlin_flute vice versa
  private void switchModel()
  {
    int wFlute_hash = Animator.StringToHash("Base Layer.hamlin_wFlute");
    int woFlute_hash = Animator.StringToHash("Base Layer.hamlin_woFlute");
    // switch the model
    AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
    if (stateInfo.fullPathHash == wFlute_hash)
    {
      player.transform.GetChild(1).gameObject.SetActive(false);
      player.transform.GetChild(2).gameObject.SetActive(true);
      hud.transform.GetChild(1).gameObject.SetActive(true);
      anim = GetComponentInChildren<Animator>();
      hold_flute = true;
      switch_model = false;
      sound_player.hamlin_flute_out.Play();
      enterPlayMode();
    }
    else if (stateInfo.fullPathHash == woFlute_hash)
    {
      player.transform.GetChild(1).gameObject.SetActive(true);
      player.transform.GetChild(2).gameObject.SetActive(false);
      hud.transform.GetChild(1).gameObject.SetActive(false);
      anim = GetComponentInChildren<Animator>();
      hold_flute = false;
      switch_model = false;
      exitPlayMode();
    }
  }

  // ready to play (cont. playing state)
  public bool hamlinReadyToPlay()
  {
    if (play_mode)
    {
      int cont_hash = Animator.StringToHash("Base Layer.hamlin_cont_play");
      int play_hash = Animator.StringToHash("Base Layer.hamlin_play_note");
      // switch the model
      AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
      if (stateInfo.fullPathHash == cont_hash || stateInfo.fullPathHash == play_hash)
      {
        return true;
      }
    }
    return false;
  }

  // Move hamlin
  public void move(Vector2 input, bool run)
  {
    // control movement
    Vector2 inputDirection = input.normalized;
    //calculates rotation for player as arctan(x / y)
    //player facing forwards = 0 deg rotation; facing right = 90 deg rotation, etc
    float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));

    if (controller.enabled)
    {
      float targetSpeed = (run ? runSpeed : walkSpeed) * inputDirection.magnitude;
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
      float animationSpeedPercent = (run ? currentSpeed / runSpeed : currentSpeed / walkSpeed * 0.5f);
      anim.SetFloat("speedPercent", animationSpeedPercent, GetModifiedSmoothTime(speedSmoothTime), Time.deltaTime);

      // walk and run sound
      if (animationSpeedPercent > 0.1 && animationSpeedPercent < 0.6 && !walk){
        walk = true;
        run = false;
        sound_player.hamlin_walk.Play();
        sound_player.hamlin_idle.Stop();
      }
      else if (animationSpeedPercent > 0.6 && !run){
        run = true;
        walk = false;
        sound_player.hamlin_run.Play();
        sound_player.hamlin_idle.Stop();
      }
      else if (animationSpeedPercent < 0.1 && (walk || run)){
        walk = false;
        run = false;
        sound_player.hamlin_walk.Stop();
        sound_player.hamlin_run.Stop();
        sound_player.hamlin_idle.Play();
      }
    }
  }

  //should work even when movement disabled
  public void Jump()
  {
    if (controller.isGrounded)
    {
      float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
      velocityY = jumpVelocity;
      anim.SetTrigger("jump");
      if (hold_flute && play_mode) exitPlayMode();
      sound_player.hamlin_jump.Play();
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
  public void setMoveActivate(bool state)
  {
    move_activated = state;
  }

  // get move activate
  public bool getMoveActivate()
  {
    return move_activated;
  }

  // check valid jump keys
  public bool checkValidJumpKey()
  {
    KeyCode[] valid_keys = {
      KeyCode.Space,
      KeyCode.Keypad0,
      KeyCode.RightControl
    };
    // check valid key
    foreach (KeyCode key in valid_keys)
    {
      if (Input.GetKeyDown(key))
      {
        return true;
      }
    }
    return false;
  }

  // check valid run keys
  public bool checkValidRunKey()
  {
    KeyCode[] valid_keys = {
      KeyCode.LeftShift,
      KeyCode.Keypad1,
      KeyCode.RightShift
    };
    // check valid key
    foreach (KeyCode key in valid_keys)
    {
      if (Input.GetKey(key))
      {
        return true;
      }
    }
    return false;
  }

  // check valid keys to take the flute
  public bool checkValidTakeFluteKey()
  {
    KeyCode[] valid_keys = {
        KeyCode.KeypadEnter,
        KeyCode.Return
    };
    // check valid key
    foreach (KeyCode key in valid_keys)
    {
      if (Input.GetKeyDown(key))
      {
        return true;
      }
    }
    return false;
  }

  // check valid keys play flute key
  public bool checkValidPlayFluteKey()
  {
    KeyCode[] valid_keys = {
      KeyCode.Y,
      KeyCode.S,
      KeyCode.X,
      KeyCode.D,
      KeyCode.C,
      KeyCode.V,
      KeyCode.G,
      KeyCode.B,
      KeyCode.H,
      KeyCode.N,
      KeyCode.J,
      KeyCode.M,
      KeyCode.Comma,
      KeyCode.Q,
      KeyCode.Alpha2,
      KeyCode.W,
      KeyCode.Alpha3,
      KeyCode.E,
      KeyCode.R,
      KeyCode.Alpha5,
      KeyCode.T,
      KeyCode.Alpha6,
      KeyCode.Z,
      KeyCode.Alpha7,
      KeyCode.U,
      KeyCode.I
    };
    // check valid key
    foreach (KeyCode key in valid_keys)
    {
      if (Input.GetKeyDown(key))
      {
        return true;
      }
    }
    return false;
  }

}
