using UnityEngine;
using System.Collections;

public class ElementRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (4,2,50*Time.deltaTime);
	}
}
