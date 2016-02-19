using UnityEngine;
using System.Collections;

public class DanceAnimOffset : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Animator> ().Play ("Idle", 0, (float)15.0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
