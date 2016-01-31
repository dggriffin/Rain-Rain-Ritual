using UnityEngine;
using UnityEngine.UI;
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

	int offbeats = 1;

	Button theCloseButton;

	void Start () {
		metronome = GameObject.Find ("Metronome").GetComponent<Metronome> ();
		metronome.OnTick += Store;
		theCloseButton = GameObject.FindObjectOfType<Button> (); //Assumes the close spell box button is the only one in the scene, safe assumption for now.
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire")) {
			Instantiate (firePrefab, gameObject.transform.position, gameObject.transform.rotation);

			if (VerifyBeat ()) {
				ElementEvent (ElementType.Fire);
				StartCoroutine (GameObject.Find ("FireRipple").GetComponent<RippleEffect> ().Ripple ());
			}
		} else if (Input.GetButtonDown ("Water")){ 
			Instantiate (waterPrefab, gameObject.transform.position, gameObject.transform.rotation);

			if (VerifyBeat()){
				ElementEvent(ElementType.Water);
				StartCoroutine (GameObject.Find ("WaterRipple").GetComponent<RippleEffect> ().Ripple ());
			}
		} else if (Input.GetButtonDown("Wind")) {
			Instantiate (windPrefab, gameObject.transform.position, gameObject.transform.rotation);
			
			if (VerifyBeat()){
				ElementEvent (ElementType.Wind);
				StartCoroutine (GameObject.Find ("WindRipple").GetComponent<RippleEffect> ().Ripple ());
			}
		} else if (Input.GetButtonDown ("Earth")) {
			Instantiate (earthPrefab, gameObject.transform.position, gameObject.transform.rotation);

			if (VerifyBeat ()) {
				ElementEvent (ElementType.Earth);
				StartCoroutine (GameObject.Find ("EarthRipple").GetComponent<RippleEffect> ().Ripple ());
			}
		}

		if (Input.GetButton ("CloseSpellBox")) {
			theCloseButton.onClick.Invoke ();
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
			print("offBeat" + offbeats);
			offbeats += 1;
			ElementEvent (ElementType.OffBeat);

			return false;
		}
	}

}