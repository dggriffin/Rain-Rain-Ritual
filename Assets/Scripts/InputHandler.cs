using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour {

	private System.DateTime lastBeat;

	public double inputThreshold = .11;

	public Metronome metronome;

	public delegate void InputEvent(ElementType element);

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

			if (VerifyBeat ()) {
				ElementEvent (ElementType.Fire);
				StartCoroutine (GameObject.Find ("FireRipple").GetComponent<RippleEffect> ().Ripple ());
			}
		} else if (Input.GetButtonDown ("Water")) { 
			Instantiate (waterPrefab, gameObject.transform.position, gameObject.transform.rotation);

			if (VerifyBeat ()) {
				ElementEvent (ElementType.Water);
				StartCoroutine (GameObject.Find ("WaterRipple").GetComponent<RippleEffect> ().Ripple ());
			}
		} else if (Input.GetButtonDown ("Wind")) {
			Instantiate (windPrefab, gameObject.transform.position, gameObject.transform.rotation);
			
			if (VerifyBeat ()) {
				ElementEvent (ElementType.Wind);
				StartCoroutine (GameObject.Find ("WindRipple").GetComponent<RippleEffect> ().Ripple ());
			}
		} else if (Input.GetButtonDown ("Earth")) {
			Instantiate (earthPrefab, gameObject.transform.position, gameObject.transform.rotation);

			if (VerifyBeat ()) {
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

	bool VerifyBeat() {
		System.DateTime inputTime = System.DateTime.Now;
		float sixteenthNote = (60 / metronome.BPM) / 4;
		float secondSpan = (float)(inputTime - lastBeat).TotalSeconds;
		if ((Mathf.Abs((float) (secondSpan % 1) - sixteenthNote)) % sixteenthNote <= inputThreshold ) {
			return true;
		} else {
			print("offBeat" + offbeats);
			offbeats += 1;
			ElementEvent (ElementType.OffBeat);

			return false;
		}
	}

}