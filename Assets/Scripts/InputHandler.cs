using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	private System.DateTime lastBeat;

	public double inputThreshold = .35;

	public Metronome metronome;

	public delegate void InputEvent(ElementType element);

	public event InputEvent ElementEvent;

	public GameObject firePrefab;
	public GameObject waterPrefab;
	public GameObject windPrefab;
	public GameObject earthPrefab;

	void Start () {
		metronome = GameObject.Find ("Metronome").GetComponent<Metronome> ();
		metronome.OnTick += Store;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire") && VerifyBeat ()) {
			ElementEvent (ElementType.Fire);
			Instantiate (firePrefab, gameObject.transform.position, gameObject.transform.rotation);
		} else if (Input.GetKeyDown ("Water") && VerifyBeat ()) {
			ElementEvent (ElementType.Water);
			Instantiate (waterPrefab, gameObject.transform.position, gameObject.transform.rotation);
		} else if (Input.GetKeyDown("Wind") && VerifyBeat ()) {
			ElementEvent (ElementType.Wind);
			Instantiate (windPrefab, gameObject.transform.position, gameObject.transform.rotation);
		} else if (Input.GetKeyDown ("Earth") && VerifyBeat ()) {
			ElementEvent (ElementType.Earth);
			Instantiate (earthPrefab, gameObject.transform.position, gameObject.transform.rotation);
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