using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

	// Use this for initialization
	private Animator animator;
	private InputHandler inputHandler;

	void Start () {
		animator = GameObject.Find ("Character").GetComponentInChildren<Animator>();
		inputHandler = GameObject.Find ("InputHandler").GetComponent<InputHandler> ();
		inputHandler.ElementEvent += ToeCurl;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ToeCurl (ElementType element, bool isOffbeat) {
		animator.Play ("ToeCurl");
	}
}
