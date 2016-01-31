using UnityEngine;
using System.Collections;

public class DrumSuicide : MonoBehaviour {

	AudioSource mySource;

	// Use this for initialization
	void Start () {
		mySource = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (mySource.isPlaying == false) {
			Destroy (gameObject);
		}
	}
}
