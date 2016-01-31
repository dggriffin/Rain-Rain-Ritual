using UnityEngine;
using System.Collections;

public class HideThisBox : MonoBehaviour {

	GameObject go;

	// Use this for initialization
	void Start () {
		go = this.gameObject;
	}
	
	public void HideBox(){
		go.SetActive (false);
	}
}