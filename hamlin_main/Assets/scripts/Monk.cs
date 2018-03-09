using System.Collections;
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
  public MonsterManager monsterManager;
  [HideInInspector]

  LearnScale baseScale;
  PlayerController playerController;
  Queue story;
  bool storyStopped;
  bool dialogueClicked;
  bool monkInteracted;
  Score score;

  //TODO: trigger monk animations

  void StoryInit(){

    //**Introduction**
    story.Enqueue("Monk: Hello there my friend! (Game: Left click anywhere to continue)");
    story.Enqueue("Hamlin: Have we met?");
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
    story.Enqueue(0); //show piano keyboard
    story.Enqueue("Game: To take out your flute, press Enter.");
    story.Enqueue("Game: Playing notes on your flute works like playing a piano keyboard.");
    story.Enqueue("Game: The bottom line of letters on your keyboard (y/z, x, c, v, ...) is the lower octave of notes, starting at C.");
    story.Enqueue("Game: These are the white keys on a piano keyboard, and the line of keys above that is the black keys.");
    story.Enqueue("Game: The two rows of keys above that are the notes an octave higher.");
    story.Enqueue(1); //show controls
    story.Enqueue(6); //C major TeachScale
    story.Enqueue(16); //try scale again loop option

    //**G Major and 1st monster battle**
    story.Enqueue("Now let's try G major. ...");
    story.Enqueue(7); //G major TeachScale
    story.Enqueue(16); //try scale again loop option
    story.Enqueue("Monk: Feel free to take a look around the monastery and practice playing your flute!");
    story.Enqueue("We are always happy to hear music here. And goodness knows the other monks could do with some cheering up.");
    story.Enqueue("But keep an eye out for the creatures. Sometimes they seem to just appear out of nowhere!");
    story.Enqueue(12); //Battle 1
    story.Enqueue("Hmm, perhaps I should become a fortune teller...I think the coast is clear now!");
    story.Enqueue("Come back when you're ready to learn some more.");
    story.Enqueue("Game: When you want to continue learning, return to the monk and press x");
    story.Enqueue(15); //Pause story until player presses x

    //**D MAJOR**
    story.Enqueue("Ah, so you're back for more? How about we try D major this time?");
    story.Enqueue("");
    story.Enqueue(8); //D major TeachScale
    story.Enqueue(16); //try scale again loop option

    //**HALF AND WHOLE TONES, MAJOR VS MINOR**
    story.Enqueue("Monk: Has this all seemed very complicated to you so far? I remember when I was first learning");
    story.Enqueue("and I couldn't imagine how anyone could possibly remember all these different scales and notes.");
    story.Enqueue("But the secret is, it's actually very simple. Each type of scale has a pattern of what we call");
    story.Enqueue("whole tones and half tones. A half tone is the same as moving by one piano key, so a half tone");
    story.Enqueue("up is one piano key to the right. But remember - if you are moving from a white key to the next");
    story.Enqueue("white key, that's a half tone up if there is no black key in between, but it's a whole tone up");
    story.Enqueue("if there is a black key between! Make sure that doesn't catch you out.");
    story.Enqueue("See, I'll show you the keyboard from my book again.");
    story.Enqueue(0); //show piano keyboard
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

    //**A MINOR, MONSTER 2**
    //TODO introduce A minor
    story.Enqueue(9); //A minor TeachScale
    story.Enqueue(16); //try scale again loop option
    story.Enqueue(13); //Battle 2

    //**E MINOR, B MINOR**
    story.Enqueue(10); //E minor TeachScale
    story.Enqueue(16); //try scale again loop option
    story.Enqueue("By the way, I heard that this isn't called B minor in every part of Espero. In some towns they call it H minor too.");
    story.Enqueue("Imagine that! The variety of the other tongues we have here in Espero has always fascinated me.");
    story.Enqueue("But anyway, I am digressing into my books again...");
    story.Enqueue(11); //B minor TeachScale
    story.Enqueue(16); //try scale again loop option

    //**Circle of fifths, MONSTER 3**
    story.Enqueue("Let me tell you another secret. The circle of fifths is your handy cheat-sheet to the world of major and minor scales;");
    story.Enqueue("it shows you how they're all connected.");
    story.Enqueue(3); //show circle of fifths
    story.Enqueue(14); //Battle 3

    //**END LEVEL**
    story.Enqueue("Monk: There are many other types of scales too, not just major and minor scales. They all have their own pattern");
    story.Enqueue("of half and whole tones. And they have the most fantastical names. Here, let me show you my book again!");
    story.Enqueue(4); //show church scales
    story.Enqueue(5); //show church scales tonics
    story.Enqueue("These are known as the church scales and so of course we're very familar with them here at the monastery,");
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

  //used to pause the story until player comes back to monk and presses X
  IEnumerator WaitForMonkInteraction(){
    monkInteracted = false;
    storyStopped = true;
    yield return new WaitUntil(() => (monkInteracted == true));
    storyStopped = false;
  }

  IEnumerator MonsterBattle(int codeTrigger, int rand){
    
    //TODO - should monster run from afar? unrealistic to just spawn next to you
    Vector3 position = new Vector3(player.position.x, player.position.y, player.position.z - 0.2f);
    Monster monster = Instantiate<Monster>(monsterManager.monsters[0], position, Quaternion.identity);

    if (codeTrigger == 12){  //Battle 1, C major or G major monster
      monster.scale_name = (ScaleNames)1;
      //monster.base_key = (rand == 0) ? (NoteBaseKey)48 : (NoteBaseKey)55;
      monster.base_key = (NoteBaseKey)55;
    }
    else if(codeTrigger == 13){ //Battle 2, D major or A minor monster
      monster.scale_name = (rand == 0) ? (ScaleNames)1 : (ScaleNames)2;
      monster.base_key = (rand == 0) ? (NoteBaseKey)50 : (NoteBaseKey)57;
    }
    else {  //codeTrigger == 14. Battle 3, E minor or B minor monster
      monster.scale_name = (ScaleNames)2;
      monster.base_key = (rand == 0) ? (NoteBaseKey)52 : (NoteBaseKey)59;
    }
    monster.gameObject.SetActive(true);
    monsterManager.monsters.Add(monster);
    monsterManager.initMonsters();
    playerController.forceActivateCombat = true;
    storyStopped = true;
    yield return new WaitUntil(() => monster == null);

    //this is to get the stave to disappear - works but slow, would like a better method
    playerController.hold_flute = false;
    playerController.forceActivateCombat = true;

    storyStopped = false;

  }


  IEnumerator TeachScale(int codeTrigger)
  {
    Vector3 position = new Vector3(player.position.x, player.position.y, player.position.z - 0.2f);
    LearnScale scale = Instantiate<LearnScale>(baseScale, position, Quaternion.identity);
    scale.unlimitedWrongNotes = true;
    if (codeTrigger == 6){  //C Major
      scale.scale_name = (ScaleNames)1;
      scale.base_key = (NoteBaseKey)48;
    }
    else if(codeTrigger == 7){  //G Major
      scale.scale_name = (ScaleNames)1;
      scale.base_key = (NoteBaseKey)55;
    }
    else if(codeTrigger == 8){  //D Major
      scale.scale_name = (ScaleNames)1;
      scale.base_key = (NoteBaseKey)50;
    }
    else if(codeTrigger == 9){  //A Minor
      scale.scale_name = (ScaleNames)2;
      scale.base_key = (NoteBaseKey)57;
    }
    else if(codeTrigger == 10){ //E Minor
      scale.scale_name = (ScaleNames)2;
      scale.base_key = (NoteBaseKey)52;
    }
    else {  //codeTrigger == 11. B Minor
      scale.scale_name = (ScaleNames)2;
      scale.base_key = (NoteBaseKey)59;
    }
    score.UpdateNumScales((int)scale.scale_name);
    scale.gameObject.SetActive(true);
    playerController.forceActivateCombat = true;
    storyStopped = true;
    yield return new WaitUntil(() => scale == null);   //wait until player has won scale to start story again
    
    //this is to get the stave to disappear - works but slow, would like a better method
    playerController.hold_flute = false;
    playerController.forceActivateCombat = true;
    
    storyStopped = false;
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

    if(codeTrigger < 0){
      print("codeTrigger error in story, value < 0");
    }

    //Show image 
    //uses mapping from images[] public var in editor. should be mapped 0 = keyboard, 1 = controls, 2 = major minor, 3 = circle of fifths, 4 = church scales, 5 = church scales tonics
    else if(codeTrigger <= 5){
      StartCoroutine(ShowImage(codeTrigger));
    }

    //Teach a scale
    else if(codeTrigger <= 11){
      StartCoroutine(TeachScale(codeTrigger));
    }

    //Monster battles
    else if(codeTrigger <= 14){
      int rand = Random.Range(0, 2);
      StartCoroutine(MonsterBattle(codeTrigger, rand));
    }

    //Pause story waiting for monk interaction
    else if(codeTrigger == 15){
      StartCoroutine(WaitForMonkInteraction());
    }

    else if(codeTrigger == 16){
      //TODO
      //StartCoroutine(Talk("Monk: Do you want to practice that scale again? (y/n)"));
    }

    else {
      print("codeTrigger undefined, value was " + codeTrigger);
    }

  }

  void Start()
  {
    storyStopped = false;
    dialogueClicked = false;
    monkInteracted = true;
    story = new Queue();
    score = GameObject.Find("GameState").GetComponent<Score>();
    baseScale = (LearnScale)GameObject.FindObjectOfType(typeof(LearnScale));
    playerController = player.GetComponent<PlayerController>();
    baseScale.gameObject.SetActive(false); //this must be done here, not before, otherwise it cannot find the object
    StoryInit(); //queue up all the dialogue
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

    if(Input.GetMouseButtonDown(0)){                                         //player left clicks to go to next line of dialogue/image
      dialogueClicked = true;
    }

    //player presses X to interact with the monk and resume story
    if (!monkInteracted && Input.GetKeyDown(KeyCode.X) && Vector3.Distance(transform.position, player.transform.position) <= 0.5f)
    {                                         
      monkInteracted = true;
    }

  }

}
