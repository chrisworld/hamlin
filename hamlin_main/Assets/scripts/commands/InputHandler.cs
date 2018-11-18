using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handle inputs
public class InputHandler : MonoBehaviour
{
  private PlayerController player;
  private FlutePlayer flute;

  void Start()
  {
    if(flute == null){
      player = gameObject.GetComponent<PlayerController>();
      flute = GameObject.Find("Player").GetComponent<FlutePlayer>();
    }
  }

  void Update()
  {
    handleInput();
  }

  public void handleInput()
  {
    // input Readers variables
    Vector2 move_direction = moveInputReader();
    // Move
    if(player.move_activated)
    {
      if(move_direction != Vector2.zero)
      {
        bool run = checkValidRunKey();
        Command command = new MoveCommand(player, move_direction, run);
        command.execute();
      }
    }
    // Jump
    if (checkValidJumpKey())
    {
      Command command = new JumpCommand(player);
      command.execute();
    }
  }

  // move input reader
  public Vector2 moveInputReader()
  {
    return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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
    foreach (KeyCode key in valid_keys){
      if (Input.GetKeyDown(key)) return true;
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
}
