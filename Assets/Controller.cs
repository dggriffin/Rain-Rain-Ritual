using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public Metronome beatMetronome;

	private static Controller instance;

	public static Controller Instance
	{	
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType (typeof(Controller)) as Controller;
			}
			return instance;
		}
			
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
