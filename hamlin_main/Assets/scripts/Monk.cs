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
  Transform monk;
  GameObject screen;
  GameObject endScreen;
  Vector3 oldPosition;
  bool storyStopped;
  bool dialogueClicked;
  bool monkInteracted;
  bool userInputYesNo;
  bool userInputLock;
  bool combatDialogueDone;
  int lastEvent;
  Score score;

  //TODO: trigger monk animations

  void StoryInit(){

    //**Introduction**
    story.Enqueue("Monk: Hello there my friend! (Game: Left click anywhere to continue)");
    story.Enqueue("Hamlin: Have we met?");
    story.Enqueue("Monk: We have not, but any music believer is a friend of mine.");
    story.Enqueue("Monk: Tell me, where did you find that flute of yours, and where are you going?");
    story.Enqueue("Monk: It can be very dangerous travelling these parts openly carrying an instrument - they hate it, you know.");
    story.Enqueue("Monk: The creatures can’t stand the sound.");
    story.Enqueue("Hamlin: I got this from the Pied Piper - he asked me to keep it safe for him. But I don't know what it's for!");
    story.Enqueue("Monk: Ah, so you have yet to discover the wonders of music. You have an epic journey ahead of you, my friend, and it will not be");
    story.Enqueue("Monk: without peril. The oracle foretold of this, that a hero with the power to liberate us from the silence would come -");
    story.Enqueue("Monk: 'but he knows not what power he wields'...");
    story.Enqueue("Hamlin: *stunned silence*");
    story.Enqueue("Monk: Pray tell me friend, what is your name?");
    story.Enqueue("Hamlin: I'm Hamlin. But...I don't understand! I'm no hero, I'm just a rat!");
    story.Enqueue("Monk: On the contrary, Hamlin, I think you're exactly who we've been looking for.  But I see I've scared you with all this talk");
    story.Enqueue("Monk: of prophecy and heroics. Come now, let me teach you what that flute of yours can do.");

    //**C Major and Controls**
    //TODO: rewrite this section, dialogue is rubbish here
    story.Enqueue("Monk: I'll teach you some scales, because scales are crucial building blocks in music. Scales are sequences of notes which fit");
    story.Enqueue("Monk: well together, and by learning scales you can start to understand how all the notes in music are connected. Traditionally");
    story.Enqueue("Monk: scales were associated with emotions and moods. For example, some people say C major reminds them of innocence");
    story.Enqueue("Monk: and children playing. A happy scale, in short! A scale we could all do with a bit more of in our lives here in Espero.");
    story.Enqueue("Monk: But you should make up your own mind about how a scale makes you feel. C Major is a very simple scale: on a piano");
    story.Enqueue("Monk: keyboard you just go along the white keys from C. Have a go at playing it and see how you think it makes you feel.");
    story.Enqueue("Monk: Here, take a look at this picture of a keyboard I have in my book.");
    story.Enqueue(0); //show piano keyboard
    story.Enqueue("Game: To take out your flute, press Enter.");
    story.Enqueue("Game: Playing notes on your flute works like playing a piano keyboard.");
    story.Enqueue("Game: The bottom line of letters on your keyboard (y/z, x, c, v, ...) is the lower octave of notes, starting at C.");
    story.Enqueue("Game: These are the white keys on a piano keyboard, and the line of keys above that is the black keys.");
    story.Enqueue("Game: The two rows of keys above that are the notes an octave higher.");
    story.Enqueue(1); //show controls
    story.Enqueue(6); //C major TeachScale

    //**G Major and 1st monster battle**
    story.Enqueue("Monk: Now let's try G major. This is normally associated with gentleness and peacefulness.");
    story.Enqueue(7); //G major TeachScale
    story.Enqueue("Monk: Feel free to take a look around the monastery and practice playing your flute!");
    story.Enqueue("Monk: We are always happy to hear music here. And goodness knows the other monks could do with some cheering up.");
    story.Enqueue("Monk: But keep an eye out for the creatures. Sometimes they seem to just appear out of nowhere!");
    story.Enqueue(12); //Battle 1 - monster takes a while to chase so time for 2 lines of dialogue
    story.Enqueue("Monk: Oh no! Here comes one now! You can fight it by playing the right scale with your flute - music is their weakness.");
    story.Enqueue("Game: Can you remember the notes for the scales you just learnt? Each monster has one scale it can be defeated with.");
    story.Enqueue(16); //signal pre-combat dialogue done
    story.Enqueue("Monk: Hmm, perhaps I should become a fortune teller...I think the coast is clear now!");
    story.Enqueue("Monk: Come back when you're ready to learn some more.");
    story.Enqueue("Game: When you want to continue learning, return to the monk and press x.");
    story.Enqueue(15); //Pause story until player presses x

    //**D MAJOR**
    story.Enqueue("Ah, so you're back for more? How about we try D major this time? D major is exciting - it's the key of triumph and hallelujahs!");
    story.Enqueue(8); //D major TeachScale

    //**HALF AND WHOLE TONES, MAJOR VS MINOR**
    story.Enqueue("Monk: Has this all seemed very complicated to you so far? I remember when I was first learning");
    story.Enqueue("Monk: and I couldn't imagine how anyone could possibly remember all these different scales and notes.");
    story.Enqueue("Monk: But the secret is, it's actually very simple. Each type of scale has a pattern of what we call");
    story.Enqueue("Monk: whole tones and half tones. A half tone is the same as moving by one piano key, so a half tone");
    story.Enqueue("Monk: up is one piano key to the right. But remember - if you are moving from a white key to the next");
    story.Enqueue("Monk: white key, that's a half tone up if there is no black key in between, but it's a whole tone up");
    story.Enqueue("Monk: if there is a black key between! Make sure that doesn't catch you out.");
    story.Enqueue("Monk: See, I'll show you the keyboard from my book again.");
    story.Enqueue(0); //show piano keyboard
    story.Enqueue("Hamlin: Alright, I'll believe you, but I'm still not even sure what a piano is.");
    story.Enqueue("Monk: I fear we may not be able to change that. The last piano I know of in Espero was");
    story.Enqueue("Monk: destroyed around 300 moons ago. Sadly now all we have are pictures of what was once");
    story.Enqueue("Monk: one of the world's greatest instruments. But if you ever see one for goodness' sake");
    story.Enqueue("Monk: play it! Ah, what I would give to play a piano again...");
    story.Enqueue("Game: The monk seems to have entered some kind of piano-induced trance.");
    story.Enqueue("Monk: Hmm, where was I? Yes, whole and half tones. So, each type of scale is just a pattern");
    story.Enqueue("Monk: of half (H) and whole (W) tones. For example, the three scales I've taught you so far have all been major scales.");
    story.Enqueue("Monk: These scales follow the pattern W, W, H, W, W, W, H. So you can choose any note on the keyboard and follow this");
    story.Enqueue("Monk: pattern, and you will have the major scale in the key of that note. Another important pattern is the minor scale");
    story.Enqueue("Monk: pattern. This pattern is W,H,W,W,H,W,W and again you can start from any initial note (the key of the scale).");
    story.Enqueue("Monk: Minor scales typically sound darker and sadder than major scales.");

    //**A MINOR, MONSTER 2**
    story.Enqueue("Monk: How about we start with A minor? It's the easiest minor scale to play as it has no sharps or flats - just like C major.");
    story.Enqueue("Monk: My book describes this scale as having a tender mood, but I must admit I'm not so sure!");
    story.Enqueue(9); //A minor TeachScale
    story.Enqueue(13); //Battle 2
    story.Enqueue("Look out - here comes another creature! It must have heard you playing your flute!");
    story.Enqueue(16); //signal pre-combat dialogue done

    //**E MINOR, B MINOR**
    story.Enqueue("You're getting good at this! The creatures are really quite harmless once you know how to defeat them.");
    story.Enqueue("Monk: Time for the next scale - E minor!");
    story.Enqueue("Someone once described the mood of this scale to me as 'sighs accompanied by a few tears'");
    story.Enqueue("but I think they were just having a very bad day - I quite like this one!");
    story.Enqueue(10); //E minor TeachScale
    story.Enqueue("Monk: By the way, I heard that this isn't called B minor in every part of Espero. In some towns they call it H minor too.");
    story.Enqueue("Monk: Imagine that! The variety of the other tongues we have here in Espero has always fascinated me. The last scale I'll");
    story.Enqueue("Monk: teach you is B minor - the key of patience. Something you need in bucketfuls with these creatures around...");
    story.Enqueue("Monk: If you have trouble learning your scales, just remember patience is all you need! Everything comes with practice.");
    story.Enqueue(11); //B minor TeachScale

    //**Circle of fifths, MONSTER 3**
    story.Enqueue("Monk: Let me tell you another secret. The circle of fifths is your handy cheat-sheet to the world of major and minor scales;");
    story.Enqueue("Monk: it shows you how they're all connected.");
    story.Enqueue(3); //show circle of fifths
    story.Enqueue(14); //Battle 3
    story.Enqueue("Get ready, there's another creature coming!");
    story.Enqueue(16); //signal pre-combat dialogue done

    //**END LEVEL**
    story.Enqueue("Well done! You're learning fast.");
    story.Enqueue("Monk: There are many other types of scales too, not just major and minor scales. They all have their own pattern");
    story.Enqueue("Monk: of half and whole tones. And they have the most fantastical names. Here, let me show you my book again!");
    story.Enqueue(4); //show church scales
    story.Enqueue(5); //show church scales tonics
    story.Enqueue("Monk: These are known as the church scales and so of course we're very familar with them here at the monastery,");
    story.Enqueue("Monk: but of course there's a whole world of other music out there. There are others like us who still know about");
    story.Enqueue("Monk: music; perhaps you will meet some of them on your travels, and they can teach you more. Anyway, I wish you");
    story.Enqueue("Monk: safe travels on your journey. So long, Hamlin, and remember - keep playing your music, don’t let the silence win.");
    //end game screens are in update

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

    //monster spawns away from player and then chases
    Transform baseMonster = monsterManager.monsters[0].transform;
    Vector3 position = new Vector3(baseMonster.position.x + 0.2f, baseMonster.position.y, baseMonster.position.z);
    Monster monster = Instantiate<Monster>(monsterManager.monsters[0], position, baseMonster.rotation);

    if (codeTrigger == 12){  //Battle 1, C major or G major monster
      monster.scale_name = (ScaleNames)1;
      monster.base_key_monster = (rand == 0) ? (NoteBaseKey)48 : (NoteBaseKey)55;
    }
    else if(codeTrigger == 13){ //Battle 2, D major or A minor monster
      monster.scale_name = (rand == 0) ? (ScaleNames)1 : (ScaleNames)2;
      monster.base_key_monster = (rand == 0) ? (NoteBaseKey)50 : (NoteBaseKey)57;
    }
    else {  //codeTrigger == 14. Battle 3, E minor or B minor monster
      monster.scale_name = (ScaleNames)2;
      monster.base_key_monster = (rand == 0) ? (NoteBaseKey)52 : (NoteBaseKey)59;
    }
    monster.gameObject.SetActive(true);
    monsterManager.monsters.Add(monster);
    monsterManager.initMonsters();  
    yield return new WaitUntil(() => combatDialogueDone);     //wait for monk to say "oh no, a monster" or whatever
    storyStopped = true;
    yield return new WaitUntil(() => monster == null);        //wait until player has defeated monster

    //this is to get the stave to disappear - works but slow, would like a better method
    playerController.hold_flute = false;
    playerController.forceActivateCombat = true;

    storyStopped = false;

  }


  IEnumerator TeachScale(int codeTrigger)
  {
    Vector3 position = new Vector3(monk.position.x - 2f, monk.position.y + 0.5f, monk.position.z);
    if (position.x == player.position.x) position.x -= 2f;    //avoid learnScale ending up at same position as monk or player
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
    if(!playerController.play_mode) playerController.forceActivateCombat = true;
    storyStopped = true;
    yield return new WaitUntil(() => scale == null);   //wait until player has won scale to start story again
    
    //this is to get the stave to disappear - works but slow, would like a better method
    playerController.hold_flute = false;
    playerController.forceActivateCombat = true;

    //check if user wants to practice again
    userInputYesNo = true;
    infobox.text = "Monk: Do you want to practice that scale again? (y/n)";
    info_image.SetActive(true);
    userInputLock = false;
    //rest of logic handled in Update if statement
    yield return new WaitUntil(() => userInputYesNo == false);
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

  IEnumerator ShowImage(int imageIndex){
    storyStopped = true;
    screen.GetComponent<Image>().sprite = images[8];                  //blank background
    GameObject screen_image = screen.transform.GetChild(0).gameObject;
    //screen_image.SetActive(true);

    //store old dialogue box values to restore later
    Sprite oldSprite = screen_image.GetComponent<Image>().sprite;
    Color oldColor = screen_image.GetComponent<Image>().color;
    RectTransform canvasTransform = screen_image.GetComponent<RectTransform>();
    Vector2 oldSize = canvasTransform.sizeDelta;

    //set new values to display image
    Sprite newSprite = images[imageIndex];
    screen_image.GetComponent<Image>().sprite = newSprite;
    screen_image.GetComponent<Image>().color = Color.white;
    Vector3 newPosition = new Vector3(oldPosition.x, oldPosition.y + 150, oldPosition.z);
    canvasTransform.sizeDelta = new Vector2(newSprite.texture.width * 0.6f, newSprite.texture.height * 0.6f);  //if this still isn't flexible enough just pass in width and height as params
    canvasTransform.position = newPosition;

    screen_image.transform.GetChild(0).gameObject.SetActive(false);   //hide text
    screen.SetActive(true);

    yield return new WaitUntil(() => (dialogueClicked == true));   //wait for click event (hideDialogueOnClick)

    //hide box and restore to old values
    screen_image.transform.GetChild(0).gameObject.SetActive(true); //show text again
    screen_image.GetComponent<Image>().sprite = oldSprite;
    screen_image.GetComponent<RectTransform>().sizeDelta = oldSize;
    screen_image.GetComponent<Image>().color = oldColor;
    canvasTransform.position = oldPosition;
    newPosition.y -= 150;

    screen_image.transform.GetChild(0).gameObject.SetActive(true); //show text again
    //screen_image.SetActive(false);
    screen.SetActive(false);
    dialogueClicked = false;
    storyStopped = false;
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

    //Used to signal end of pre-monster dialogue
    else if (codeTrigger == 16){
      combatDialogueDone = true;
    }

    else {
      print("codeTrigger undefined, value was " + codeTrigger);
    }

    lastEvent = codeTrigger;

  }

  IEnumerator StoryEnd(){
    dialogueClicked = false;
    Image screen_image = endScreen.GetComponent<Image>();
    screen_image.sprite = images[6]; //congrats
    endScreen.SetActive(true);
    yield return new WaitUntil(() => (dialogueClicked == true));   //wait for click event (hideDialogueOnClick)
    
    endScreen.transform.GetChild(0).gameObject.SetActive(false);      //hide textbox
    dialogueClicked = false;
    endScreen.GetComponent<Image>().sprite = images[7]; //credits
    yield return new WaitUntil(() => (dialogueClicked == true));   //wait for click event (hideDialogueOnClick)
    SceneManager.LoadScene("MainMenu_pablo");
  
   }

  void Start()
  {
    storyStopped = false;
    dialogueClicked = false;
    monkInteracted = true;
    userInputYesNo = false;
    combatDialogueDone = false;
    lastEvent = 0;
    story = new Queue();
    score = GameObject.Find("GameState").GetComponent<Score>();
    monk = (Transform) GameObject.Find("Monk").transform;
    baseScale = (LearnScale)GameObject.FindObjectOfType(typeof(LearnScale));
    playerController = player.GetComponent<PlayerController>();
    screen = GameObject.Find("MonkScreen");
    screen.SetActive(false);
    endScreen = GameObject.Find("MonkScreenEnd");
    endScreen.SetActive(false);
    baseScale.gameObject.SetActive(false); //this must be done here, not before, otherwise it cannot find the object
    StoryInit(); //queue up all the dialogue
    score.SetScoreTotal(9, false);
    Vector3 pos = screen.transform.GetChild(0).gameObject.GetComponent<RectTransform>().position;
    oldPosition = new Vector3(pos.x, pos.y, pos.z);

  }

  void Update()
  {

    //player has walked off a cliff, as players do
    if(player.position.y < -50){
      infobox.text = "You fell to your doom.";
      info_image.SetActive(true);
      
      if(player.position.y < -150){
        SceneManager.LoadScene("MainMenu_pablo");
      }
    }

    //Only play next story event when player is close to monk. We use info_image's active status as a lock for showing messages so we don't unqueue them too fast
    if (!storyStopped && info_image.activeSelf == false && Vector3.Distance(transform.position, player.transform.position) <= 4f)
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

    if (!storyStopped && story.Count < 1)                                                     //story finished
    {
      storyStopped = true;
      StartCoroutine(StoryEnd());                                                            //show end game screen and go back to main menu
    }

    if(Input.GetMouseButtonDown(0)){                                         //player left clicks to go to next line of dialogue/image
      dialogueClicked = true;
    }

    //player presses X to interact with the monk and resume story
    if (!monkInteracted && Input.GetKeyDown(KeyCode.X) && Vector3.Distance(transform.position, player.transform.position) <= 4f)
    {                                         
      monkInteracted = true;
    }

    //player presses Y to repeat learning scale or N to skip
    if(userInputYesNo && !playerController.play_mode && !userInputLock){

      if (Input.GetKeyDown(KeyCode.Y)){
        userInputLock = true;
        info_image.SetActive(false);
        StoryEvent(lastEvent);
      }
      else if(Input.GetKeyDown(KeyCode.N)){     //continue story
        info_image.SetActive(false);
        userInputYesNo = false;
      }

    }

  }

}
