using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Monk : MonoBehaviour {

  public GameObject info_image;
  public Text infobox;
  public Transform player;
  
  Queue script;
  LearnScale baseScale;
  PlayerController playerController;
  bool playerNear;
  bool scriptStopped;

  //TODO: trigger monk animations

  //Edit text here to change the dialogue between the player and monk. Numbers are triggers for code in monkEvent().
  void initScript()
  {

    //**INTRODUCTION**
    script = new Queue();
    script.Enqueue("Monk: Hello there my friend!");
    script.Enqueue("Hamlin: Have we met?");
    script.Enqueue("Monk: We have not, but any music believer is a friend of mine.");
    script.Enqueue("Tell me, where did you find that flute of yours, and where are you going?");
    script.Enqueue("It can be very dangerous travelling these parts openly carrying an instrument - they hate it, you know.");
    script.Enqueue("The creatures can’t stand the sound.");
    script.Enqueue("Hamlin: A flute? What’s a flute? I found this thing by the side of the river in Hortondale.");
    script.Enqueue("I guess it must have washed up there from somewhere upstream. What it’s for?");
    script.Enqueue("Monk: Ah, so you have yet to discover the wonders of music. You have an epic journey ahead of you, my friend");
    script.Enqueue("and it will not be without peril. The oracle foretold of this, that a hero with the power to liberate us");
    script.Enqueue("from the silence would come - 'but he knows not what power he wields'.");
    script.Enqueue("Hamlin: *stunned silence*");
    script.Enqueue("Monk: Pray tell me friend, what is your name?");
    script.Enqueue("I'm Hamlin. But...I don't understand! I'm no hero, I'm just a rat!");
    script.Enqueue("Monk: On the contrary, Hamlin, I think you're exactly who we've been looking for.");
    script.Enqueue("Monk: But I see I've scared you with all this talk of prophecy and heroics. Come now, let me teach you what that flute of yours can do.");

    //**CONTROLS AND C MAJOR**
    script.Enqueue("I'll teach you some scales, because scales are crucial building blocks in music.");
    script.Enqueue("Scales are sequences of notes which fit well together, and by learning scales you can");
    script.Enqueue("start to understand how all the notes in music are connected. Traditionally scales were");
    script.Enqueue("associated with emotions and moods. For example, C major ")
    script.Enqueue("Game: To take out your flute, press Enter.");
    script.Enqueue("Game: Playing notes on your flute works like playing a piano keyboard.");
    script.Enqueue(11); //show piano keyboard
    script.Enqueue("Game: The bottom line of letters on your keyboard (y/z, x, c, v, ...) is the lower octave of notes, starting at C.");
    script.Enqueue("Game: These are the white keys on a piano keyboard, and the line of keys above that is the black keys.");
    script.Enqueue("Game: The two rows of keys above that are the notes an octave higher.");
    script.Enqueue(0); //C major learn scale

    //**G MAJOR**
    script.Enqueue(1);

    //**MONSTER 1**
    script.Enqueue("Feel free to take a look around the monastery and practice playing your flute!");
    script.Enqueue("We are always happy to hear music here. And goodness knows the other monks could do with some cheering up.");
    script.Enqueue("But keep an eye out for the creatures. Sometimes they seem to just appear out of nowhere!");
    //joke: monster appears out of nowhere. C major or G major
    script.Enqueue(2);
    script.Enqueue("Hmm, perhaps I should become a fortune teller...I think the coast is clear now!");
    script.Enqueue("Come back when you're ready to learn some more.");
    //PAUSE DIALOGUE

    //**D MAJOR**
    script.Enqueue(3);

    //**HALF AND FULL TONES, MAJOR VS MINOR**
    script.Enqueue("Monk: Has this all seemed very complicated to you so far? I remember when I was first learning");
    script.Enqueue("Monk: and I couldn't imagine how anyone could possibly remember all these different scales and notes.");
    script.Enqueue("Monk: But the secret is, it's actually very simple. Each type of scale has a pattern of what we call");
    script.Enqueue("full tones and half tones. A half tone is the same as moving by one piano key, so a half tone");
    script.Enqueue("up is one piano key to the right. But remember - if you are moving from a white key to the next");
    script.Enqueue("white key, that's a half tone up if there is no black key in between, but it's a full tone up");
    script.Enqueue("if there is a black key between! That can catch you out at first.");
    script.Enqueue("See, I'll show you the keyboard from my book again.");
    script.Enqueue(11); //show piano keyboard
    script.Enqueue("Hamlin: Alright, I'll believe you, but I'm still not even sure what a piano is.");
    script.Enqueue("Monk: I fear we may not be able to change that. The last piano I know of in Espero was");
    script.Enqueue("Monk: destroyed around 300 moons ago. Sadly now all we have are pictures of what was once");
    script.Enqueue("Monk: one of the world's greatest instruments. But if you ever see one for goodness' sake");
    script.Enqueue("Monk: play it! Ah, what I would give to play a piano again...");
    script.Enqueue("Game: the monk seems to have entered some kind of piano-induced trance.");
    script.Enqueue("...");

    //**A MINOR**
    //TODO
    script.Enqueue(4);

    //**MONSTER 2**
    script.Enqueue(5);

    //**E MINOR**
    script.Enqueue(6);

    //**B MINOR**
    script.Enqueue("By the way, I heard that this isn't called B minor in every part of Espero. In some towns they call it H minor too.");
    script.Enqueue("Imagine that! The variety of the other tongues we have here in Espero has always fascinated me.");
    script.Enqueue("But anyway, I am digressing into my books again...");
    script.Enqueue(7);
    
    //**Circle of fifths**


    // [TODO: MORE DIALOGUE]
    // todo: need to show two pictures of scales as well

    //**END LEVEL**
    script.Enqueue("Well, I've taught you all I can. Here at the monastery we're only familiar with the church scales");
    script.Enqueue("but there's a whole world of other music out there. There are others like us who still know about music,");
    script.Enqueue("you just have to look carefully for them. I wish you safe travels on your journey");
    script.Enqueue("So long, Hamlin. It was a privilege to meet you.");
    //TODO: show game over screen - full screen image by Andrea and text below
    //"Congratulations, you completed Musical Theory Foundations and helped make Espero a happier place!
    // Stay tuned for new musical adventures soon in the next update. In the meantime, you can carry on 
    // making music in adventure mode."
    //DO NOT FORGET TO PUT 'a wild C dorian monster appears' in adventure mode

      //script.Enqueue("Monk: The other day I found an ancient book hidden away behind a bookshelf in the monastery’s library. It was so old its leather cover started to tear" +
      //" as I picked it up, but I could still just about read the title: ‘Ideen zu einer Aesthetik der Tonkunst’.");
      //script.Enqueue("Hamlin: You read German?!");
      //script.Enqueue("Monk: Why of course!We speak many tongues hear at the monastery. We must be very careful of course, for the");

      /* *****THEORY TO TURN INTO DIALOGUE******
      To play scales you only need to know the intervals between the notes and where to start with.
      We have full tones(1 or F) and half tones(1 / 2 or H).
      So for a major scale the sequence of intervals is F, F, H, F, F, F, H, so as if you start with a C and go along all white keys
      you will notice that between E and F there is no black key! This is a half tone! For minor scale it is similar: The sequence is 
      F,H,F,F,H,F,F so as if you start with A and follow all white keys upwards.This rules last for every base key, but there will 
      not only white keys.The circle of fifths shows you for which base - key there are which # or b which mean a half note higher or lower that ne written note.


      WHOLE TONE not full tone hence WT
      the scales in the picture are the CHURCH SCALES which is why we are teaching them at the monastery, mention this

     full tone jumps over a key. eg two white keys next to each other is half tone unless black key inbetween then you skipped a key so it's full tone = two half tones.
     major scale is full tone full tone half tone ...
     so you can work out any scale from the start key just by remembering the sequence of full and half tones you need
     black keys = sharps and flats, check?
     explain controls - row above is black keys on keyboard:
     bottom row y/z x c etc is white keys starting at lower c
     then row above that is black keys for that octave
     then row above that is white keys for upper ocatve
     row above that (numbers) is black keys for upper octave
     other types of scales are different patterns of full and half tones, you can work out in any base key if you know this pattern e.g. c dorian just dorian pattern starting at c key.

      */


  }

  void teachScale(int scaleName, int baseKey){
    //TODO position - should be v. close to player so it auto activates "combat"
    Vector2 position = new Vector2(player.position.x - 0.5f, player.position.y);
    LearnScale scale = Instantiate<LearnScale>(baseScale, position, Quaternion.identity);
    scale.scale_name = (ScaleNames) scaleName;
    scale.base_key = (NoteBaseKey) baseKey;
    scale.gameObject.SetActive(true);
    playerController.play_mode = true;
    //TODO
    //dialogue needs to pause here until they actually play the scale...ugh
  }



  void monkEvent(){

    //event is dialogue, display in infobox
    if(script.Peek().GetType() == typeof(string)){
      StartCoroutine(ShowMessage((string) script.Dequeue(), 5f, false));
    }
    else {  //event is a code trigger

      int trigger = (int) script.Dequeue();
      scriptStopped = true; //pause dialogue while we run event
      switch(trigger){

        //LearnScale C major
        case 0:
          teachScale(1, 48);
          break;

        //LearnScale G major
        case 1:
          teachScale(1, 55);
          break;

        //Battle 1 - C major or G major monster
        case 2:
          //TODO
          break;

        //LearnScale D major
        case 3:
          teachScale(1, 50);
          break;

        //LearnScale A minor
        case 4:
          teachScale(2, 57);
          break;

        //Battle 2 - D major or A minor monster
        case 5:
          //TODO
          break;

        //LearnScale E minor
        case 6:
          teachScale(2, 52);
          break;
        
        //LearnScale B (H) minor
        case 7:
          teachScale(2, 59);
          break;

        //Battle 3 - E minor or B (H) minor monster
        case 8:
          //TODO
          break;

        //Circle of fifths - does this need any code?
        case 9:
          //TODO
          break;

        //End of level - congratulations screen. Ideally have boss monster if time.
        case 10:
          //TODO
          break;

        //Show piano keyboard image (ideally with notes annotated) and pause for a bit
        case 11:
          //TODO
          break;

      }
      scriptStopped = false;
    }
  
  }

  void Start()
  {
    playerNear = false;
    scriptStopped = false;
    baseScale = (LearnScale) GameObject.FindObjectOfType(typeof(LearnScale));
    playerController = player.GetComponent<PlayerController>();
    if(playerController == null){     //debug, delete
      print("ALERT PLAYER CONTROLLER NULL");
    }
    baseScale.gameObject.SetActive(false); //this must be done here, not before, otherwise it cannot find the object
    initScript();

    //setup scene:
    //hide all monsters (or instantiate them when needed? probably better)
    //player should spawn close to monk

  }

  void Update()
  {

    //start dialogue when player is close to monk
    if (Vector3.Distance(transform.position, player.transform.position) <= 1)
    {
      playerNear = true;
    }

    //show next line of dialogue
    if (playerNear && info_image.activeSelf == false && !scriptStopped)
    {       //we use info_image's active status as a lock for showing messages, otherwise previous messages get skipped

      monkEvent();
      if (script.Count < 1)
      {
        scriptStopped = true;
      }

    }

  }

  //do not call directly, call with StartCoroutine(ShowMessage(...))
  IEnumerator ShowMessage(string message, float delay, bool endGame)
  {


    //DO NOT NEED TO CHANGE SIZE BACK BECAUSE I WILL NOT NEED THE BOX FOR ANYTHING ELSE IN THIS LEVEL

    //extra code here is to resize textbox (as we have long messages here), then resize back to normal after
    infobox.text = message;
    int oldFontSize = infobox.fontSize;
    RectTransform canvasTransform = info_image.GetComponent<RectTransform>();
    Vector2 oldSize = canvasTransform.sizeDelta;
    infobox.fontSize = 30;
    canvasTransform.sizeDelta = new Vector2(400, oldSize.y);
    info_image.SetActive(true);
    yield return new WaitForSeconds(delay);
    info_image.SetActive(false);
    canvasTransform.sizeDelta = oldSize;
    infobox.fontSize = oldFontSize;
    if (endGame)
    {
      SceneManager.LoadScene("MainMenu_cat");
    }
  }

}
