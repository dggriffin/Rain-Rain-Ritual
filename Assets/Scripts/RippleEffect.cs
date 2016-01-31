using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RippleEffect : MonoBehaviour {

	// Use this for initialization
	Dictionary<string, Transform> dict = new Dictionary<string, Transform>();
	void Start(){
		foreach(Transform t in transform)
		{
			dict.Add(t.name, t);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public IEnumerator Ripple() {
		dict ["Particle System"].GetComponent<ParticleSystem>().Play ();
		Transform lastChild = null;
		int count = 0;
		Component lastRipple = null;
		foreach (Transform t in transform) {
			if (lastChild != null) {
				//yield return new WaitForSeconds (0.000001f);
				if (lastChild.GetComponent<MeshRenderer> ()) {
					lastChild.GetComponent<MeshRenderer> ().enabled = false;
				}
			}
			if (t.GetComponent<MeshRenderer> ()) {
				t.GetComponent<MeshRenderer> ().enabled = true;
			}
			lastChild = t;
			yield return new WaitForSeconds (0.0000001f); 
			count++;
			if (count == transform.childCount) {
				if (t.GetComponent<MeshRenderer> ()) {
					t.GetComponent<MeshRenderer> ().enabled = false;
				}
			}
		}
		dict ["Particle System"].GetComponent<ParticleSystem>().Stop ();
	}
}
