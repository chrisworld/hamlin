
public class JumpCommand : Command {
  private  PlayerController _player;

  public JumpCommand(PlayerController player){
    this._player = player;
  }

  public override void execute()
  {
    _player.Jump();
  }
}
