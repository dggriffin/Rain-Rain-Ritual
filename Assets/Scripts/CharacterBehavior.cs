using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

	// Use this for initialization
	private Animator animator;
	private InputHandler inputHandler;

	void Start () {
		animator = GameObject.Find ("Character").GetComponentInChildren<Animator>();
		inputHandler = GameObject.Find ("InputHandler").GetComponent<InputHandler> ();
		ListenToEvents ();
	}

	void ListenToEvents () {
		inputHandler.ElementEvent += ToeCurl;
	}
 	
	// Update is called once per frame
	void Update () {
		
	}

	void ToeCurl (ElementType element) {
		animator.Play ("ToeCurl");
	}
}
