using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	private System.DateTime lastBeat;

	public double inputThreshold = .35;

	public Metronome metronome;

	public delegate void InputEvent(ElementType element);

	public event InputEvent ElementEvent;

	void Start () {
		metronome = GameObject.Find ("Metronome").GetComponent<Metronome> ();
		metronome.OnTick += Store;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("w") && VerifyBeat ()) {
			ElementEvent (ElementType.Fire);
		} else if (Input.GetKeyDown ("s") && VerifyBeat ()) {
			ElementEvent (ElementType.Water);
		} else if (Input.GetKeyDown("a") && VerifyBeat ()) {
			ElementEvent (ElementType.Wind);
		} else if (Input.GetKeyDown ("d") && VerifyBeat ()) {
			ElementEvent (ElementType.Earth);
		}
	}

	void Store () {
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
			print("offBeat");
			ElementEvent (ElementType.OffBeat);

			return false;
		}
	}

}