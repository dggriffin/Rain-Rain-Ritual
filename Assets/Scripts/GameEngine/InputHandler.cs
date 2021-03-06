﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour {

	private System.DateTime lastMeasure;
	private System.DateTime lastInput;

	//Distance a user's input can be from falling on a noteOfSignificance, in SECONDS
	public double inputThreshold = .11;

	//Smallest length note we care to track, (1/8 -> eighth note, 1/16 -> sixteenth, etc.)
	//We throw away any note below this length
	public double noteOfSignificance = 1/16;

	public Metronome metronome;

	public delegate void InputEvent(ElementType element, bool isOffbeat = false);

	public event InputEvent ElementEvent;

	public GameObject firePrefab;
	public GameObject waterPrefab;
	public GameObject windPrefab;
	public GameObject earthPrefab;

	Button theCloseButton;

	int offbeats = 1;

	void Start () {
		metronome = GameObject.Find ("Metronome").GetComponent<Metronome> ();
		metronome.OnNewMeasure += Store;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire")) {
			Instantiate (firePrefab, gameObject.transform.position, gameObject.transform.rotation);

			if (VerifyBeat (ElementType.Fire)) {
				ElementEvent (ElementType.Fire);
				StartCoroutine (GameObject.Find ("FireRipple").GetComponent<RippleEffect> ().Ripple ());
			}
		} else if (Input.GetButtonDown ("Water")) { 
			Instantiate (waterPrefab, gameObject.transform.position, gameObject.transform.rotation);

			if (VerifyBeat (ElementType.Water)) {
				ElementEvent (ElementType.Water);
				StartCoroutine (GameObject.Find ("WaterRipple").GetComponent<RippleEffect> ().Ripple ());
			}
		} else if (Input.GetButtonDown ("Wind")) {
			Instantiate (windPrefab, gameObject.transform.position, gameObject.transform.rotation);
			
			if (VerifyBeat (ElementType.Wind)) {
				ElementEvent (ElementType.Wind);
				StartCoroutine (GameObject.Find ("WindRipple").GetComponent<RippleEffect> ().Ripple ());
			}
		} else if (Input.GetButtonDown ("Earth")) {
			Instantiate (earthPrefab, gameObject.transform.position, gameObject.transform.rotation);

			if (VerifyBeat (ElementType.Earth)) {
				ElementEvent (ElementType.Earth);
				StartCoroutine (GameObject.Find ("EarthRipple").GetComponent<RippleEffect> ().Ripple ());
			}
		} else if (Input.GetKeyDown("space")) { //cwkTODO figure out how to just listen on the canvas
			var instructionCanvas = GameObject.Find("InstructionCanvas").GetComponent<Canvas> ();
			if (instructionCanvas.enabled) {
				var instructionBox = GameObject.Find ("RainInstructionBox"); //cwkTODO hard coded to rain :(
				instructionBox.GetComponent<Button> ().onClick.Invoke ();
			}
		}
	}

	void Store () {
		lastMeasure = System.DateTime.Now;
	}

	bool VerifyBeat(ElementType type) {
		System.DateTime inputTime = System.DateTime.Now;
 
		float noteOfSignificanceLengthInSeconds = (float) (60 / metronome.BPM) * (float) (this.noteOfSignificance * 4);
		float secondsSinceLastMeasure = (float)(inputTime - this.lastMeasure).TotalSeconds;

		// ANTI-SPAM: If the user is spamming notes quicker than our "note of measure", return offbeat
		if ((inputTime - this.lastInput).TotalSeconds < noteOfSignificanceLengthInSeconds) {
			this.lastInput = inputTime;
			//print("offBeat" + offbeats);
			offbeats += 1;
			ElementEvent (type, true);
			return false;
		}

		this.lastInput = inputTime;

		//How far the user's input was from falling on a noteOfSignificance, in seconds
		float distanceOfUserInputFromNote = (Mathf.Abs ((float)(secondsSinceLastMeasure % 1) - noteOfSignificanceLengthInSeconds)) % noteOfSignificanceLengthInSeconds;

		if (distanceOfUserInputFromNote <= inputThreshold ) {
			return true;
		} else {
			print("offBeat" + offbeats);
			offbeats += 1;
			ElementEvent (type, true);
			return false;
		}
	}

}