using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	// Use this for initialization
	private System.DateTime lastBeat;

	public double inputThreshold = .40;

	public Metronome metronome;

	void Start () {
		metronome = GameObject.Find ("Metronome").GetComponent<Metronome> ();
		metronome.OnTick += Store;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
		{
			System.DateTime inputTime = System.DateTime.Now;
			double millisecondSpan = (inputTime - lastBeat).TotalMilliseconds;
			if (System.Math.Round(millisecondSpan - inputThreshold) % 2 == 0 || System.Math.Round(millisecondSpan + inputThreshold) % 2 == 0) {
				print ("onBeat" + System.Math.Round((inputTime - lastBeat).TotalMilliseconds));
			} else {
				print ("offBeat");
			}
			//print ("fire");
		}
	}

	void Store (Metronome metronome) {
		lastBeat = System.DateTime.Now;
		//print (lastBeat);
	}
}
