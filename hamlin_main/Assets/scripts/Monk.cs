using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Monk : MonoBehaviour {

  public GameObject info_image;
  public Text infobox;
  public Transform player;
  public Queue<string> script;
  bool playerNear;
  bool scriptFinished;

  //TODO: monk needs to move - either idle 

  //This is the *only* place you need to edit text to change the dialogue between the player and monk
  void initScript() {

    script = new Queue<string>();
    script.Enqueue("Monk: Hello there my friend!");
    script.Enqueue("Hamlin: Have we met?");
    script.Enqueue("Monk: We have not, but any music believer is a friend of mine. Tell me, where did you find that flute of yours, and where are you going? " +
    "It can be very dangerous travelling these parts openly carrying an instrument - they hate it, you know. Can’t stand the sound.");
    script.Enqueue("Hamlin: A flute? What’s a flute? I found this thing by the side of the river in Hortondale - I guess it must have washed up there from somewhere upstream. What it’s for?");
    script.Enqueue("Monk: Ah, so you have yet to discover the wonders of music. You have an epic journey ahead of you, my friend, and it will not be without peril.");
    script.Enqueue("The oracle foretold of this, that a hero with the power to liberate us from the silence would come - 'but he knows not what power he wields'.");
    script.Enqueue("Hamlin: *stunned silence*");
    script.Enqueue("Monk: Pray tell me friend, what is your name?");
    script.Enqueue("I'm Hamlin. But...I don't understand! I'm no hero, I'm just a rat!");
    script.Enqueue("Monk: On the contrary, Hamlin, I think you're exactly who we've been looking for.");
    script.Enqueue("Monk: But I see I've scared you with all this talk of prophecy and heroics. Come now, let me teach you what that flute of yours can do.");
    //NOTE: at this point 



    // [TODO: MORE DIALOGUE]

    script.Enqueue("Monk: The other day I found an ancient book hidden away behind a bookshelf in the monastery’s library. It was so old its leather cover started to tear" +
    " as I picked it up, but I could still just about read the title: ‘Ideen zu einer Aesthetik der Tonkunst’.");
    script.Enqueue("Hamlin: You read German?!");
    script.Enqueue("Monk: Why of course!We speak many tongues hear at the monastery.We must be very careful of course, for the");
  
   }

  void Start () {
    playerNear = false;
    scriptFinished = false;
    initScript();

    //setup scene:
    //hide all monsters (or instantiate them when needed? probably better)
    //player should spawn close to monk

  }
	
	void Update () {

    //start dialogue when player is close to monk
    if (Vector3.Distance(transform.position, player.transform.position) <= 1)
    {
      playerNear = true;
    }

    //show next line of dialogue
    if (playerNear && info_image.activeSelf == false && !scriptFinished){       //we use info_image's active status as a lock for showing messages, otherwise previous messages get skipped

      monkTalk();
      if (script.Count < 1){
        scriptFinished = true;
      }

    }

  }


  //TODO: message should show until user input instead of time delay?
  //note: uses next message from stack
  void monkTalk(){
    StartCoroutine(ShowMessage(script.Dequeue(), 5f, false));
  }    

  //do not call directly, call with StartCoroutine(ShowMessage(...))
  IEnumerator ShowMessage(string message, float delay, bool endGame)
  {
    infobox.text = message;
    info_image.SetActive(true);
    yield return new WaitForSeconds(delay);
    info_image.SetActive(false);
    if (endGame)
    {
      SceneManager.LoadScene("MainMenu_cat");
    }
  }


}
