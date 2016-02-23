using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HideThisBox : MonoBehaviour {

	GameObject go;

	// Use this for initialization
	void Start () {
		go = this.gameObject;
	}
	
	public void HideBox(){
		//Don't use setActive because we can't access the object again later
		//go.SetActive (false);

		var image = go.GetComponent<Image> ();
		image.canvas.enabled = false;
	}
}