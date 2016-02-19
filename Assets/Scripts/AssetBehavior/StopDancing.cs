using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StopDancing : MonoBehaviour {

	public GameObject[] dancers;
	//Supply a list of all the background dancers in the Unity editor
	AudioSource thisSource;
	List<Animator> dancerMotion = new List<Animator>();

	// Use this for initialization
	//This script is meant to be attached to the object that is playing the theme music.
	void Start () {
		Animator[] thisMotion;
		thisSource = gameObject.GetComponent<AudioSource> ();
		foreach (GameObject go in dancers){
			thisMotion = go.GetComponents<Animator> ();
			dancerMotion.AddRange (thisMotion);
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
		foreach (Animator a in dancerMotion) {
			a.enabled = false;
		}
	}
}