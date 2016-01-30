using UnityEngine;
using System.Collections;

public class LightBehaviorFire : MonoBehaviour {

	private Light thisLight;
	private Color originalColor;
	private float timePassed;
	private float changedValue;
	// Use this for initialization
	void Start () {

		thisLight = this.GetComponent<Light> ();

		if (thisLight != null) {
			originalColor = thisLight.color;
		} else {
			enabled = false;
			return;
		}

		changedValue = 0;
		timePassed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timePassed = Time.time;
		timePassed = timePassed - Mathf.Floor (timePassed);

		thisLight.color = originalColor * CalculateChange ();
	}

	private float CalculateChange(){
		changedValue = -Mathf.Sin (timePassed * 4 * Mathf.PI) * 0.08f + 0.95f;
		return changedValue;
	}
}
