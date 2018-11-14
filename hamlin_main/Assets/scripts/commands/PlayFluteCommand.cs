
// play a flute note
public class PlayFluteCommand : Command 
{
  private int _note;
  private  FlutePlayer _flute;

  public PlayFluteCommand(FlutePlayer flute, int note){
    this._flute = flute;
    this._note = note;
  }

	public override void execute()
  {
    _flute.PlayFluteNote(_note);
  }
}