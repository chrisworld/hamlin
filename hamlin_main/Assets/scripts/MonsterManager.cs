using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MonsterManager : MonoBehaviour
{
  // GameObjects
  public Transform player;
  public PlayerController player_controller;
  public Health health;
  public ContainerManager container;
  public SoundPlayer sound_player;
  public GameObject infowindow;
  public Text infobox;
  public Object nextScene;

  [HideInInspector]
  public Monster[] monsters;  //do NOT make this private

  // settings
  public float viewDistance = 2f;
  public float attackDistance = 1f;
  public float viewAngle = 60f;
  public ScaleNames scale_name;
  public NoteBaseKey base_key;

  private bool activated;
  private bool chasing;
  private int currentMonsterId;
  private bool initialisedMonsters;

  private int num_c = 12;   //was 11
  private int num_n = 15;   //was 15
  private int c_pos;
  private int error_counter;
  private NoteState[][] note_state = new NoteState[12][];
  private SignState[][] sign_state = new SignState[12][];
  private KeyCode[] valid_keys = {
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

  public int[][] allScales =   // Scales Definition
  {
    new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11},
    new int[] {0, 2, 4, 5, 7, 9, 11, 12},
    new int[] {0, 2, 3, 5, 7, 8, 10, 12},
    new int[] {0, 2, 4, 5, 7, 9, 11, 12},
    new int[] {0, 2, 3, 5, 7, 8, 10, 12},
    new int[] {0, 2, 3, 5, 7, 8, 11, 12},
    new int[] {0, 2, 3, 5, 7, 8, 9, 10, 11, 12}, // mix of ascend and descend
		new int[] {0, 2, 3, 5, 7, 8, 10, 12},
    new int[] {0, 2, 3, 5, 7, 8, 10, 12},
    new int[] {0, 2, 3, 5, 7, 8, 10, 12},
    new int[] {0, 1, 3, 5, 7, 8, 10, 12},
    new int[] {0, 1, 3, 5, 6, 8, 10, 12},
    new int[] {0, 2, 3, 5, 7, 9, 10, 12},
    new int[] {0, 2, 4, 6, 7, 9, 11, 12},
    new int[] {0, 2, 4, 5, 7, 9, 10, 12},
    new int[] {0, 2, 4, 7, 9, 12},
    new int[] {0, 2, 3, 4, 5, 7, 9, 10, 11, 12},
    new int[] {0, 1, 3, 5, 7, 10, 11, 12},
    new int[] {0, 1, 1, 4, 5, 8, 10 ,12},
  };

  void Start()
  {
    infowindow.SetActive(false);
    monsters = (Monster[]) GameObject.FindObjectsOfType(typeof(Monster));
    chasing = false;
    activated = false;
  }

  void Update()
  {
    bool monsterActivatedThisTurn = false;
    int result = 0;

    if (!initialisedMonsters && monsters != null){
      for (int i = 0; i < monsters.Length; i++)
      {
        resetNoteState();
        resetSignState();
        // container position
        c_pos = 0;
        error_counter = 0;
        monsters[i].box_scale = allScales[(int)monsters[i].scale_name];
        monsters[i].box_midi = scaleToMidi(monsters[i].box_scale);
        container.updateNoteContainer(note_state);
        container.updateSignContainer(sign_state);
      }
      initialisedMonsters = true;
    }
    else {
      if (activated)
      {           //update just the active monster, activated = false when combat has finished
        result = UpdateMonster(currentMonsterId);
        monsterActivatedThisTurn = true;
      }
      else
      {                  //cycle through all monsters and activate first one which is within combat distance, if any
        for (int i = 0; i < monsters.Length; i++)
        {

          if (!monsterActivatedThisTurn && Vector3.Distance(player.position, monsters[i].transform.position) < viewDistance && Vector3.Angle(player.position - monsters[i].transform.position, monsters[i].transform.forward) < viewAngle)
          {
            monsterActivatedThisTurn = true;
            currentMonsterId = i;
            result = UpdateMonster(i);
          }
          else
          {
            //deactivate everything
            monsters[i].anim.SetBool("isRunning", false);
            monsters[i].anim.SetBool("isWalking", false);
            monsters[i].anim.SetBool("isAttacking", false);
            monsters[i].anim.SetBool("isIdle", true);
            if (i == (monsters.Length - 1) && !monsterActivatedThisTurn)
            {
              //this fixes a bug where monsters were stopping then instantly reactivating due to player proximity
              activated = false;
            }
          }
        }
      }
      if (result == 1 || health.GetHealthAmount() == 0)
      {
        resetNoteState();
        resetSignState();
        player_controller.setMoveActivate(true);
        StartCoroutine(ShowMessage("You lose :'(", 3f, true));
      }

      //load next level if player has defeated all the monsters
      bool allMonstersDefeated = true;
      foreach (Monster monster in monsters){
        if(monster.defeated == false){
          allMonstersDefeated = false;
          break;
        }
      }
      if(allMonstersDefeated){
        SceneManager.LoadScene(nextScene.name);
      }

    }


  //TODO: add key listener for buttons that activate scales
  // set soundPlayer.inLearning = true;
  //should let player play notes without activating combat 
  }

  //return values: 0 no action, 1 player lost
  int UpdateMonster(int id) {

    Vector3 direction = player.position - monsters[id].transform.position;
    // start the scale
    if (!activated && Input.anyKey)
    {
      activated = true;
      // put scale
      setNoteStateToScale(monsters[id].box_scale);
      setSignStateToScale(monsters[id].box_scale);
      c_pos = 0;
      error_counter = 0;
      sound_player.inCombat = true;
     }
     // stop the scale
     else if (activated && player_controller.checkValidJumpKey())
     {
        activated = false;
        player_controller.setMoveActivate(true);
        c_pos = 0;
        error_counter = 0;
        resetNoteState();
        resetSignState();
        sound_player.inCombat = false;
        monsters[id].nav.isStopped = false;
        return 0;
     }
      else if (activated && (direction.magnitude > attackDistance))     //too far away to attack, chase player
      {
        monsters[id].anim.SetBool("isRunning", false);
        monsters[id].anim.SetBool("isWalking", true);
        monsters[id].anim.SetBool("isAttacking", false);
        monsters[id].anim.SetBool("isIdle", false);
        monsters[id].nav.destination = player.position;
        chasing = true; //TODO: stop monster chasing player for ever and never letting other monster attack
      }
      // play the scales
      else if (activated)
      {
        monsters[id].anim.SetBool("isRunning", false);
        monsters[id].anim.SetBool("isWalking", false);
        monsters[id].anim.SetBool("isAttacking", false);
        monsters[id].anim.SetBool("isIdle", true);
      if (!monsters[id].nav.isStopped)
        {
          monsters[id].nav.ResetPath();
          monsters[id].nav.isStopped = true;
        }
        player_controller.setMoveActivate(false);
        int key = 0;
        bool[] key_mask = getKeyMask();
        if (c_pos >= monsters[id].box_scale.Length)          //player has beaten monster
        {
          activated = false;
          StartCoroutine(ShowMessage("You win!", 3f, false));
          resetNoteState();
          resetSignState();
          player_controller.setMoveActivate(true);
          return 0;
        }
        else if (health.GetHealthAmount() == 0)         //game ends if they run out of health
        {
          print("player out of health");
          activated = false;
          return 1;
        }
        else
        {
          Camera.main.fieldOfView = 40f;                          //zoom in camera to go into 'combat mode'
          monsters[id].transform.LookAt(player);   //TODO: we need this but need to change the player transform somehow as it's the wrong angle

          // check each key
          foreach (bool mask in key_mask)
          {
            if (mask)
            {
              cleanWrongNoteState(monsters[id].box_scale);
              int note_midi = keyToMidiMapping(key);
              int note_pos = midiToContainerMapping(note_midi);
              if (note_midi == monsters[id].box_midi[c_pos])
              {
                note_state[c_pos][note_pos] = NoteState.RIGHT;
                sign_state[c_pos][note_pos] = midiToSignState(note_midi);
                c_pos++;
              }
              else
              {
                note_state[c_pos][note_pos] = NoteState.WRONG;
                sign_state[c_pos][note_pos] = midiToSignState(note_midi);
                error_counter++;
                monsters[id].anim.SetBool("isAttacking", true);
                monsters[id].anim.SetBool("isIdle", false);
              }
            }
            key++;
          }
        }

        container.updateNoteContainer(note_state);
        container.updateSignContainer(sign_state);
      }
    return 0;

  }


  //do not call directly, call with StartCoroutine(ShowMessage(...))
  IEnumerator ShowMessage(string message, float delay, bool endGame)
  {
    infobox.text = message;
    infowindow.SetActive (true);
    yield return new WaitForSeconds(delay);
    infowindow.SetActive (false);
    if(endGame){
      SceneManager.LoadScene("MainMenu");
    }
  }



  // ***************************************************************************
  // Functions for note/scale management



  // remove wrong notes played before
  void cleanWrongNoteState(int[] right_scale)
  {
    for (int c = 0; c < num_c; c++)
    {
      for (int n = 0; n < num_n; n++)
      {
        if (note_state[c][n] == NoteState.WRONG)
        {
          if (scaleToContainerMapping(right_scale[c]) == n)
          {
            note_state[c][n] = NoteState.NORMAL;
            sign_state[c][n] = scaleToSignStateMapping(right_scale[c]);
          }
          else
          {
            note_state[c][n] = NoteState.DISABLED;
            sign_state[c][n].act = false;
          }
        }
      }
    }
    container.updateNoteContainer(note_state);
    container.updateSignContainer(sign_state);
  }

  // set the NoteState to all disabled
  void resetNoteState()
  {
    for (int c = 0; c < num_c; c++)
    {
      note_state[c] = new NoteState[num_n];
      for (int n = 0; n < num_n; n++)
        {
          note_state[c][n] = NoteState.DISABLED;
        }
    }
    container.updateNoteContainer(note_state);
  }

  // set the SignState to all disabled
  void resetSignState()
  {
    for (int c = 0; c < num_c; c++)
    {
      sign_state[c] = new SignState[num_n];
      for (int n = 0; n < num_n; n++)
      {
        sign_state[c][n].act = false;
      }
    }
    container.updateSignContainer(sign_state);
  }

  // set the note_state to a scale
  void setNoteStateToScale(int[] update_scale)
  {
    int ci = 0;
    int ni = 0;
    foreach (int note in update_scale)
    {
      ni = scaleToContainerMapping(note);
      //Debug.Log("debug:" + note + " " + ci + " " + ni);
      note_state[ci][ni] = NoteState.NORMAL;
      ci++;
    }
    container.updateNoteContainer(note_state);
  }

  // set the sign_state to a scale
  void setSignStateToScale(int[] update_scale)
  {
    int ci = 0;
    SignState st;
    foreach (int note in update_scale)
    {
      st = scaleToSignStateMapping(note);
      sign_state[ci][st.pos].act = st.act;
      ci++;
    }
    container.updateSignContainer(sign_state);
  }

  // check if valid music key is pressed
  public bool checkValidMusicKey()
  {
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

  // get mask of pressed keys
  public bool[] getKeyMask()
  {
    int k = 0;
    bool[] key_mask = new bool[valid_keys.Length];
    // set to zero
    for (int c = 0; c < valid_keys.Length; c++)
    {
      key_mask[c] = false;
    }
    // get mask
    foreach (KeyCode key in valid_keys)
    {
      if (Input.GetKeyDown(key))
      {
        key_mask[k] = true;
      }
      k++;
    }
    return key_mask;
  }

  // map the keys to midi
  public int keyToMidiMapping(int key)
  {
    if (key > 12)
    {
      key--;
    }
    return key + 48;
  }

  // puts a scale to a midi array
  public int[] scaleToMidi(int[] scale)
  {
    int[] midi = new int[scale.Length];
    for (int i = 0; i < scale.Length; i++)
    {
      midi[i] = scale[i] + (int)base_key;
    }
    return midi;
  }

  public int scaleToContainerMapping(int scale_note)
  {
    return midiToContainerMapping((int)base_key + scale_note);
  }

  public SignState scaleToSignStateMapping(int scale_note)
  {
    return midiToSignState((int)base_key + scale_note);
  }

  public int midiToContainerMapping(int midi)
  {
    switch (midi)
    {
      case 48:
      case 49: return 14; // c, cis
      case 50:
      case 51: return 13; // d, dis
      case 52: return 12; // e
      case 53:
      case 54: return 11; // f, fis
      case 55:
      case 56: return 10;
      case 57:
      case 58: return 9;
      case 59: return 8;
      case 60:
      case 61: return 7;
      case 62:
      case 63: return 6;
      case 64: return 5;
      case 65:
      case 66: return 4;
      case 67:
      case 68: return 3;
      case 69:
      case 70: return 2;
      case 71: return 1;
      case 72: return 0;
      default: break;
    }
    return 0;
  }

  public SignState midiToSignState(int midi)
  {
    SignState st;
    st.act = false;
    st.pos = 0;
    switch (midi)
    {
      case 48: st.pos = 14; st.act = false; return st;  // c
      case 49: st.pos = 14; st.act = true; return st; //cis
      case 50: st.pos = 13; st.act = false; return st;  // d
      case 51: st.pos = 13; st.act = true; return st; // dis
      case 52: st.pos = 12; st.act = false; return st;  // e
      case 53: st.pos = 11; st.act = false; return st;  //f
      case 54: st.pos = 11; st.act = true; return st;
      case 55: st.pos = 10; st.act = false; return st;
      case 56: st.pos = 10; st.act = true; return st;
      case 57: st.pos = 9; st.act = false; return st;
      case 58: st.pos = 9; st.act = true; return st;
      case 59: st.pos = 8; st.act = false; return st;
      case 60: st.pos = 7; st.act = false; return st;
      case 61: st.pos = 7; st.act = true; return st;
      case 62: st.pos = 6; st.act = false; return st;
      case 63: st.pos = 6; st.act = true; return st;
      case 64: st.pos = 5; st.act = false; return st;
      case 65: st.pos = 4; st.act = false; return st;
      case 66: st.pos = 4; st.act = true; return st;
      case 67: st.pos = 3; st.act = false; return st;
      case 68: st.pos = 3; st.act = true; return st;
      case 69: st.pos = 2; st.act = false; return st;
      case 70: st.pos = 2; st.act = true; return st;
      case 71: st.pos = 1; st.act = false; return st;
      case 72: st.pos = 0; st.act = false; return st;
      default: break;
    }
    return st;
  }
}
