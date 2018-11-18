using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCommand : Command {
  private  PlayerController player;
  private  Vector2 input;
  private  bool run;

  public MoveCommand(PlayerController player, Vector2 input, bool run){
    this.player = player;
    this.input = input;
    this.run = run;
  }

  public override void execute()
  {
    player.move(input, run);
  }
}
