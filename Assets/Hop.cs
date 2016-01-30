using UnityEngine;
using System.Collections;

public class Hop : MonoBehaviour {
	// Use this for initialization

	private Rigidbody rb;

	private AudioSource audio;

	private GameObject metroObject;
	void Start () {
		metroObject = GameObject.Find("Metronome");
		rb = GameObject.Find("Sphere").AddComponent<Rigidbody>();
		metroObject.GetComponent<Metronome>().OnTick += Talk;
		metroObject.GetComponent<Metronome> ().OnNewMeasure += White;

		audio = GetComponent<AudioSource>();

	}

	// Update is called once per frame
	void Update () {
		
	}

	void Talk(Metronome metro) {
		print ("wee");
		Vector3 movement = new Vector3 (0.0f, 0.8f, 0.0f);
		rb.AddForce (movement);
		audio.Play();
	}

	void Color (Metronome metro) {
		GameObject.Find ("Sphere").GetComponent<Renderer> ().material.color = UnityEngine.Color.red;

	}

	void White (Metronome metro) {
		GameObject.Find ("Sphere").GetComponent<Renderer> ().material.color = UnityEngine.Color.white;
		print ("measure");
	}
}
