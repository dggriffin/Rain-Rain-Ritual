using UnityEngine;
using System.Collections;

public class Hop : MonoBehaviour {
	// Use this for initialization

	private Rigidbody rb;

	private AudioSource audioPlay;

	private GameObject metroObject;

	private GameObject inputHandler;

	void Start () {
		metroObject = GameObject.Find("Metronome");

		rb = GameObject.Find("Sphere").AddComponent<Rigidbody>();
		metroObject.GetComponent<Metronome>().OnTick += Talk;
		metroObject.GetComponent<Metronome> ().OnNewMeasure += White;

		audioPlay = GetComponent<AudioSource>();


//		//INPUTHANDLER EXAMPLE
//		inputHandler = GameObject.Find ("InputHandler");
//
//		inputHandler.GetComponent<InputHandler> ().FireEvent += Fire;
//		inputHandler.GetComponent<InputHandler> ().WaterEvent += Water;
//		inputHandler.GetComponent<InputHandler> ().AirEvent += Air;
//		inputHandler.GetComponent<InputHandler> ().EarthEvent += Earth;
//		inputHandler.GetComponent<InputHandler> ().OffBeatEvent += OffBeat;
	}

	// Update is called once per frame
	void Update () {
		
	}

	void Fire(InputHandler handler) {
		print ("fireee");
	}
	void Water(InputHandler handler) {
		print ("Wateeer");
	}
	void Air(InputHandler handler) {
		print ("Aiiirr");
	}
	void Earth(InputHandler handler) {
		print ("Eaaarth");
	}
	void OffBeat(InputHandler handler) {
		print ("OFFBEAT");
	}

	void Talk() {
		//print ("wee");
		Vector3 movement = new Vector3 (0.0f, 0.8f, 0.0f);

		rb.AddForce (movement);
		audioPlay.Play();
	}

	void Color () {
		GameObject.Find ("Sphere").GetComponent<Renderer> ().material.color = UnityEngine.Color.red;

	}

	void White () {
		GameObject.Find ("Sphere").GetComponent<Renderer> ().material.color = UnityEngine.Color.white;
		//print ("measure");
	}
}
