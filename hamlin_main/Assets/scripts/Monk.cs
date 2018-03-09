﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Monk : MonoBehaviour
{

  public GameObject info_image;
  public Text infobox;
  public Transform player;
  public Sprite[] images;

  LearnScale baseScale;
  PlayerController playerController;
  Queue story;
  bool playerNear;
  bool storyStopped;
  bool dialogueClicked;

  //TODO: trigger monk animations

  void StoryInit(){

    //**Introduction**
    story.Enqueue("Monk: Hello there my friend! (Game: Left click anywhere to continue)");
    story.Enqueue("Hamlin: Have we met?");
    story.Enqueue(0);
    story.Enqueue("Monk: We have not, but any music believer is a friend of mine.");
    story.Enqueue("Tell me, where did you find that flute of yours, and where are you going?");
    story.Enqueue("It can be very dangerous travelling these parts openly carrying an instrument - they hate it, you know.");
    story.Enqueue("The creatures can’t stand the sound.");
    story.Enqueue("Hamlin: A flute? What’s a flute? I found this thing by the side of the river in Hortondale.");
    story.Enqueue("I guess it must have washed up there from somewhere upstream. What it’s for?");
    story.Enqueue("Monk: Ah, so you have yet to discover the wonders of music. You have an epic journey ahead of you, my friend");
    story.Enqueue("and it will not be without peril. The oracle foretold of this, that a hero with the power to liberate us");
    story.Enqueue("from the silence would come - 'but he knows not what power he wields'.");
    story.Enqueue("Hamlin: *stunned silence*");
    story.Enqueue("Monk: Pray tell me friend, what is your name?");
    story.Enqueue("Hamlin: I'm Hamlin. But...I don't understand! I'm no hero, I'm just a rat!");
    story.Enqueue("Monk: On the contrary, Hamlin, I think you're exactly who we've been looking for.");
    story.Enqueue("But I see I've scared you with all this talk of prophecy and heroics. Come now, let me teach you what that flute of yours can do.");

    //**C Major and Controls**
    story.Enqueue("I'll teach you some scales, because scales are crucial building blocks in music.");
    story.Enqueue("Scales are sequences of notes which fit well together, and by learning scales you can");
    story.Enqueue("start to understand how all the notes in music are connected. Traditionally scales were");
    story.Enqueue("associated with emotions and moods. But of course different people had different ideas");
    story.Enqueue("about what each scale meant! For example, some people say C major reminds them of innocence, naivety");
    story.Enqueue("and children's talk. A happy scale, in short! A scale we could all do with a bit more of in our lives");
    story.Enqueue("here in Espero. But others say it reminds them of decisiveness and powerful religious feelings.");
    story.Enqueue("Still others associate different colours with each scale. You could drive yourself mad trying to think");
    story.Enqueue("about all the different associations for too long! I reckon it's best to just make up your own mind");
    story.Enqueue("about how a scale makes you feel. Have a go at playing it and see what you think.");
    story.Enqueue("C Major is a very simple scale: on a piano keyboard you just go along the white keys from C.");
    story.Enqueue("Here, take a look at this picture of a keyboard I have in my book.");

    //TODO SHOW PIANO KEYBOARD

    story.Enqueue("Game: To take out your flute, press Enter.");
    story.Enqueue("Game: Playing notes on your flute works like playing a piano keyboard.");
    story.Enqueue("Game: The bottom line of letters on your keyboard (y/z, x, c, v, ...) is the lower octave of notes, starting at C.");
    story.Enqueue("Game: These are the white keys on a piano keyboard, and the line of keys above that is the black keys.");
    story.Enqueue("Game: The two rows of keys above that are the notes an octave higher.");

    //TODO:     TeachScale(1, 48); //C major learn scale

    //**G Major and 1st monster battle**
    story.Enqueue("Now let's try G major. ...");
    //TODO: TeachScale(1, 55);
    story.Enqueue("Monk: Feel free to take a look around the monastery and practice playing your flute!");
    story.Enqueue("We are always happy to hear music here. And goodness knows the other monks could do with some cheering up.");
    story.Enqueue("But keep an eye out for the creatures. Sometimes they seem to just appear out of nowhere!");
    //TODO: Battle 1 - C major or G major monster
    story.Enqueue("Hmm, perhaps I should become a fortune teller...I think the coast is clear now!");
    story.Enqueue("Come back when you're ready to learn some more.");
    //PAUSE DIALOGUE wait for player to come back - some kind of trigger? "press x to talk to monk again"
    story.Enqueue("Game: When you want to continue learning, return to the monk and press x");

    //**D MAJOR**
    story.Enqueue("Ah, so you're back for more? How about we try D major this time?");
    story.Enqueue("");
    //TODO TeachScale(1, 50);

    //**HALF AND WHOLE TONES, MAJOR VS MINOR**
    story.Enqueue("Monk: Has this all seemed very complicated to you so far? I remember when I was first learning");
    story.Enqueue("and I couldn't imagine how anyone could possibly remember all these different scales and notes.");
    story.Enqueue("But the secret is, it's actually very simple. Each type of scale has a pattern of what we call");
    story.Enqueue("whole tones and half tones. A half tone is the same as moving by one piano key, so a half tone");
    story.Enqueue("up is one piano key to the right. But remember - if you are moving from a white key to the next");
    story.Enqueue("white key, that's a half tone up if there is no black key in between, but it's a whole tone up");
    story.Enqueue("if there is a black key between! Make sure that doesn't catch you out.");
    story.Enqueue("See, I'll show you the keyboard from my book again.");
    //TODO SHOW PIANO KEYBOARD
    story.Enqueue("Hamlin: Alright, I'll believe you, but I'm still not even sure what a piano is.");
    story.Enqueue("Monk: I fear we may not be able to change that. The last piano I know of in Espero was");
    story.Enqueue("destroyed around 300 moons ago. Sadly now all we have are pictures of what was once");
    story.Enqueue("one of the world's greatest instruments. But if you ever see one for goodness' sake");
    story.Enqueue("play it! Ah, what I would give to play a piano again...");
    story.Enqueue("Game: the monk seems to have entered some kind of piano-induced trance.");
    story.Enqueue("Monk: Hmm, where was I? Yes, whole and half tones. So, each type of scale is just a pattern");
    story.Enqueue("of half (H) and whole (W) tones. For example, the three scales I've taught you so far have all been major scales.");
    story.Enqueue("These scales follow the pattern W, W, H, W, W, W, H. So you can choose any note on the keyboard and follow this");
    story.Enqueue("pattern, and you will have the major scale in the key of that note. Another important pattern is the minor scale");
    story.Enqueue("pattern. This pattern is W,H,W,W,H,W,W and again you can start from any initial note (the key of the scale).");
    story.Enqueue("Minor scales typically sound darker and sadder than major scales. But certainly no less important!");

    //**A MINOR**
    //TODO introduce A minor
    //TODO TeachScale(2, 57);

    //**MONSTER 2**
    //todo Battle 2 - D major or A minor monster

    //**E MINOR**
    //TODO TeachScale(2, 52);

    //**B MINOR**
    story.Enqueue("By the way, I heard that this isn't called B minor in every part of Espero. In some towns they call it H minor too.");
    story.Enqueue("Imagine that! The variety of the other tongues we have here in Espero has always fascinated me.");
    story.Enqueue("But anyway, I am digressing into my books again...");
    //TODO TeachScale(2, 59);

    //**Circle of fifths**
    story.Enqueue("Let me tell you another secret. The circle of fifths is your handy cheat-sheet to the world of major and minor scales;");
    story.Enqueue("it shows you how they're all connected.");
    //SHOW CIRCLE OF FIFTHS IMAGE until click through

    //**MONSTER 3**
    //todo Battle 3 - E minor or B (H) minor monster

    //**END LEVEL**
    story.Enqueue("Monk: There are many other types of scales too, not just major and minor scales. They all have their own pattern");
    story.Enqueue("of half and whole tones. And they have the most fantastical names. Here, let me show you my book again!");
    //SHOW 1ST CHURCH SCALES IMAGE. NEED TO SHOW THIS FOR A LONG TIME. ideally until click through.
    story.Enqueue("These are known as the church scales and so of course we're very familar with them here at the monastery.");
    story.Enqueue("but of course there's a whole world of other music out there. There are others like us who still know about");
    story.Enqueue("music; perhaps you will meet some of them on your travels, and they can teach you more. Anyway, I wish you");
    story.Enqueue("safe travels on your journey. So long, Hamlin, and remember - keep playing your music, don’t let the silence win.");
    //TODO: show game over screen - full screen image by Andrea and text below
    //"Congratulations, you completed Musical Theory Foundations and helped make Espero a happier place!
    // Stay tuned for new musical adventures soon in the next update. In the meantime, you can carry on 
    // making music in adventure mode."
    //DO NOT FORGET TO PUT 'a wild C dorian monster appears' in adventure mode

  }

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


  void TeachScale(int scaleName, int baseKey)
  {
    //TODO position - should be v. close to player so it auto activates "combat"
    Vector2 position = new Vector2(player.position.x - 0.5f, player.position.y);
    LearnScale scale = Instantiate<LearnScale>(baseScale, position, Quaternion.identity);
    scale.scale_name = (ScaleNames)scaleName;
    scale.base_key = (NoteBaseKey)baseKey;
    scale.gameObject.SetActive(true);
    playerController.play_mode = true;
    //TODO
    //dialogue needs to pause here until they actually play the scale...ugh
  }

  //manages dialogue
  IEnumerator Talk(string message){
    infobox.text = message;
    info_image.SetActive(true);
    yield return new WaitUntil( () => (dialogueClicked == true) );   //wait for click event (hideDialogueOnClick)
    info_image.SetActive(false);
    dialogueClicked = false;
  }

  //manages image display (for keyboard, circle of fifths images etc)
  IEnumerator ShowImage(int imageIndex)
  {
    //store old dialogue box values to restore later
    Sprite oldSprite = info_image.GetComponent<Image>().sprite;
    Color oldColor = info_image.GetComponent<Image>().color;
    RectTransform canvasTransform = info_image.GetComponent<RectTransform>();
    Vector2 oldSize = canvasTransform.sizeDelta;
    Vector3 oldPosition = canvasTransform.position;
    infobox.gameObject.SetActive(false);

    //set new values to display image
    Sprite newSprite = images[imageIndex];
    info_image.GetComponent<Image>().sprite = newSprite;
    info_image.GetComponent<Image>().color = Color.white;
    Vector3 newPosition = new Vector3(oldPosition.x, oldPosition.y + 150, oldPosition.z);
    canvasTransform.sizeDelta = new Vector2(newSprite.texture.width * 0.6f, newSprite.texture.height * 0.6f);  //if this still isn't flexible enough just pass in width and height as params
    canvasTransform.position = newPosition;
    info_image.SetActive(true);
    yield return new WaitUntil(() => (dialogueClicked == true));   //wait for click event (hideDialogueOnClick)

    //hide box and restore to old values
    infobox.gameObject.SetActive(true);
    info_image.SetActive(false);
    info_image.GetComponent<Image>().sprite = oldSprite;
    canvasTransform.sizeDelta = oldSize;
    info_image.GetComponent<Image>().color = oldColor;
    canvasTransform.position = oldPosition;
    dialogueClicked = false;
  }

  void StoryEvent(int codeTrigger){

    switch(codeTrigger){

        //**Displaying images**

        //show piano keyboard

        //show churchScales
        case 0:
          StartCoroutine(ShowImage(0));
          break;

        //show churchScales tonics

        //show circle of fifths

        //show major and minor scales?



        //TODO: a billion more cases
        //add TeachScale cases

        default:
          break;

    }

  }

  void Start()
  {
    playerNear = false;
    storyStopped = false;
    dialogueClicked = false;
    story = new Queue();
    baseScale = (LearnScale)GameObject.FindObjectOfType(typeof(LearnScale));
    playerController = player.GetComponent<PlayerController>();
    baseScale.gameObject.SetActive(false); //this must be done here, not before, otherwise it cannot find the object
    StoryInit(); //queue up all the dialogue

    //setup scene:
    //hide all monsters (or instantiate them when needed? probably better)
    //player should spawn close to monk

  }

  void Update()
  {

    //Only play next story event when player is close to monk. We use info_image's active status as a lock for showing messages so we don't unqueue them too fast
    if (!storyStopped && info_image.activeSelf == false && Vector3.Distance(transform.position, player.transform.position) <= 0.5f)
    {
        if (story.Peek().GetType() == typeof(string))                         //event is dialogue, display in infobox
        {
          StartCoroutine(Talk((string)story.Dequeue()));
        }
        else                                                                  //event is an int code trigger
        {
          StoryEvent((int)story.Dequeue());
        }

    }

    if (story.Count < 1)                                                     //story finished
    {
      storyStopped = true;
    }

    if(Input.GetMouseButtonDown(0)){
      dialogueClicked = true;
    }

  }

}
