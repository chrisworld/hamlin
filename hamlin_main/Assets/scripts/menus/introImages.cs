 using UnityEngine;
 using UnityEngine.UI;
 using System.Collections;
using UnityEngine.SceneManagement;

 public class introImages : MonoBehaviour {
 
	private int i;
	public Transform[] images;
	public Button skipButton;
	public Button playButton;
	public Transform MainMenu;
	private AudioSource bgMusic;

    void Start () {

		Time.timeScale = 1;
		i = 0;
		MainMenu = GameObject.Find("MainMenu").GetComponent<Transform>();
		bgMusic = MainMenu.gameObject.GetComponent<AudioSource>();
		bgMusic.Stop();

		Transform test1 = GameObject.Find("MainMenu").GetComponent<Transform>();
		Transform img1 = test1.transform.GetChild (8);
		Transform primera = img1.transform.GetChild (0);
		primera.gameObject.SetActive(true);

		Transform test2 = GameObject.Find("MainMenu").GetComponent<Transform>();
		Transform img2 = test2.transform.GetChild (8);
		Transform segunda = img2.transform.GetChild (1);
		segunda.gameObject.SetActive(false);

		Transform test3 = GameObject.Find("MainMenu").GetComponent<Transform>();
		Transform img3 = test3.transform.GetChild (8);
		Transform tercera = img3.transform.GetChild (2);
		tercera.gameObject.SetActive(false);

		Transform test4 = GameObject.Find("MainMenu").GetComponent<Transform>();
		Transform img4 = test4.transform.GetChild (8);
		Transform cuarta = img4.transform.GetChild (3);
		cuarta.gameObject.SetActive(false);

		Transform test5 = GameObject.Find("MainMenu").GetComponent<Transform>();
		Transform img5 = test5.transform.GetChild (8);
		Transform quinta = img5.transform.GetChild (4);
		quinta.gameObject.SetActive(false);

		images = new Transform[5];
		images[0] = primera;
		images[1] = segunda;
		images[2] = tercera;
		images[3] = cuarta;
		images[4] = quinta;

		StartCoroutine(Carousel());

    }


	IEnumerator Carousel(){

		yield return new WaitForSeconds(27);
		images[i].gameObject.SetActive(false);
		i++;
		images[i].gameObject.SetActive(true);

		yield return new WaitForSeconds(18);
		images[i].gameObject.SetActive(false);
		i++;

		images[i].gameObject.SetActive(true);

		yield return new WaitForSeconds(6);
		images[i].gameObject.SetActive(false);
		i++;
		images[i].gameObject.SetActive(true);

		yield return new WaitForSeconds(15);
		images[i].gameObject.SetActive(false);
		i++;
		images[i].gameObject.SetActive(true);

		buttonSetActive();

	}

	private void buttonSetActive(){

		Transform skipButton = GameObject.Find("MainMenu").GetComponent<Transform>();
		Transform sb = skipButton.transform.GetChild (8);
		Transform button1 = sb.transform.GetChild (6);
		button1.gameObject.SetActive(false);

		Transform playButton = GameObject.Find("MainMenu").GetComponent<Transform>();
		Transform pb = playButton.transform.GetChild (8);
		Transform button2 = pb.transform.GetChild (7);
		button2.gameObject.SetActive(true);
		
	}

	public void restart(){

		images[0].gameObject.SetActive(true);
		images[1].gameObject.SetActive(false);
		images[2].gameObject.SetActive(false);
		images[3].gameObject.SetActive(false);
		images[4].gameObject.SetActive(false);
	}
}