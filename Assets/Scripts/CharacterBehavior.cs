using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

	// Use this for initialization

	private Animator animator;
	int idleHash = Animator.StringToHash("Idle");

	void Start () {
		animator = GameObject.Find ("Character").GetComponentInChildren<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
