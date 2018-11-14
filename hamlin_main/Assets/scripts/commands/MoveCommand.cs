using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command {
  private  PlayerController player;
  private  Vector2 input;

  public MoveCommand(PlayerController pla, Vector2 inp){
    this.player = pla;
    this.input = inp;
  }

  public override void execute()
  {
    /* Todo
    // control movement
    Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    Vector2 inputDirection = input.normalized;
    //calculates rotation for player as arctan(x / y)
    //player facing forwards = 0 deg rotation; facing right = 90 deg rotation, etc
    if (player.move_activated && inputDirection != Vector2.zero)
    {
      float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
      player.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
    }

    if (move_activated && controller.enabled)
    {
      bool running = false;
      if (checkValidRunKey() && !hold_flute) running = true;
      float targetSpeed = (running ? runSpeed : walkSpeed) * inputDirection.magnitude;
      currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

      player.velocityY += Time.deltaTime * gravity;
      Vector3 velocity = player.transform.forward * currentSpeed + Vector3.up * velocityY;
      player.controller.Move(velocity * Time.deltaTime);
      currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

      if (controller.isGrounded)
      {
        player.velocityY = 0;
      }
      //this is how we tell the animation controller which state we're in
      float animationSpeedPercent = (running ? currentSpeed / runSpeed : currentSpeed / walkSpeed * 0.5f);
      player.anim.SetFloat("speedPercent", animationSpeedPercent, GetModifiedSmoothTime(speedSmoothTime), Time.deltaTime);

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
    */
  }
}
