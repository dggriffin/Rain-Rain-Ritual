using UnityEngine;
using System.Collections;

public class StopDancing : MonoBehaviour {

	public GameObject[] dancers;
	AudioSource thisSource;
	Animator[] dancerMotion;

	// Use this for initialization
	void Start () {
		System.Array.Resize (ref dancerMotion, dancers.GetLength(0));
		thisSource = gameObject.GetComponent<AudioSource> ();
		for (int k = 0; k <= 3; k++) {
			dancerMotion[k] = dancers[k].GetComponent<Animator>();
		}
	}

	//TEST CODE ONLY
	void Update(){
		if (Input.GetKeyDown("q")){
			StopMusic ();
		}
	}

	void StopMusic(){
		thisSource.Stop ();
		for (int k = 0; k <= 3; k++) {
			dancerMotion [k].enabled = false;
		}
	}
}