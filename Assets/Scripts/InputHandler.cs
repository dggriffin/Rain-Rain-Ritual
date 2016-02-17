using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour {

	private System.DateTime lastBeat;

	public double inputThreshold = .35;

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
		metronome.OnTick += Store;
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
		} else if (Input.GetButton ("HideBox")) {
			theCloseButton = GameObject.FindObjectOfType<Button> (); //assumes only 1 close button will be active at a time
			if (theCloseButton != null) {
				theCloseButton.onClick.Invoke ();
			}
		}
	}

	void Store () {
		lastBeat = System.DateTime.Now;
		//print (lastBeat);
	}

	bool VerifyBeat(ElementType type) {
		System.DateTime inputTime = System.DateTime.Now;
		double millisecondSpan = (inputTime - lastBeat).TotalMilliseconds;
		if (System.Math.Round(millisecondSpan - inputThreshold) % 2 == 0 || System.Math.Round(millisecondSpan + inputThreshold) % 2 == 0) {
			//print ("onBeat" + System.Math.Round((inputTime - lastBeat).TotalMilliseconds));
			return true;
		} else {
			//print("offBeat" + offbeats);
			offbeats += 1;
			ElementEvent (type, true);

			return false;
		}
	}

}