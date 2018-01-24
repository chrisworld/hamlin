using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class clef_controller : MonoBehaviour {

	public Transform player;
	static Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

	}


	private IEnumerator Die()
	{		
		yield return new WaitForSeconds(1.333f);
		Destroy(gameObject);
	}


	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Alpha1)){
			
			anim.Play("listening");
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){

			anim.Play("rightNote");
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)){

			anim.Play("disappear");
		
			Die ();

		}

		Vector3 direction = player.position - this.transform.position;

		if (Vector3.Distance (player.position, this.transform.position) < 1) {


			direction.y = 0;

			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.05f);

			anim.SetBool ("isWaiting", false);
			anim.SetBool ("isListening", true);
		}

		else 
		{
			anim.SetBool ("isWaiting", true);
			anim.SetBool ("isListening", false);
		}
	}
}