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
    // Jump
    if (checkValidJumpKey())
    {
      Command command = new JumpCommand(player);
      command.execute();
    }
    // Move
    else if(checkValidJumpKey())//Todo
    {
      Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
      Command command = new MoveCommand(player, input);
      command.execute();
    }
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
}
