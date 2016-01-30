using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	private System.DateTime lastBeat;

	public double inputThreshold = .35;

	public Metronome metronome;

	public delegate void InputEvent(InputHandler input);

	public event InputEvent FireEvent;
	public event InputEvent WaterEvent;
	public event InputEvent EarthEvent;
	public event InputEvent AirEvent;
	public event InputEvent OffBeatEvent;

	void Start () {
		metronome = GameObject.Find ("Metronome").GetComponent<Metronome> ();
		metronome.OnTick += Store;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("w") && VerifyBeat ()) {
			if (FireEvent != null) {
				FireEvent (this);
			}
		} else if (Input.GetKeyDown ("s") && VerifyBeat ()) {
			if (WaterEvent != null) {
				WaterEvent (this);
			}
		} else if (Input.GetKeyDown("a") && VerifyBeat ()) {
			if (AirEvent != null) {
				AirEvent (this);
			}
		} else if (Input.GetKeyDown ("d") && VerifyBeat ()) {
			if (EarthEvent != null) {
				EarthEvent (this);
			}
		}
	}

	void Store (Metronome metronome) {
		lastBeat = System.DateTime.Now;
		//print (lastBeat);
	}

	bool VerifyBeat() {
		System.DateTime inputTime = System.DateTime.Now;
		double millisecondSpan = (inputTime - lastBeat).TotalMilliseconds;
		if (System.Math.Round(millisecondSpan - inputThreshold) % 2 == 0 || System.Math.Round(millisecondSpan + inputThreshold) % 2 == 0) {
			//print ("onBeat" + System.Math.Round((inputTime - lastBeat).TotalMilliseconds));
			return true;
		} else {
			//print ("offBeat");
			if (OffBeatEvent != null) {
				OffBeatEvent (this);
			}
			return false;
		}
	}

}