using UnityEngine;
using System.Collections;

public class RemoveSpellBox : MonoBehaviour {

	GameObject theSpellBox;

	// Use this for initialization
	void Start () {
		theSpellBox = this.gameObject;
	}

	public void DestroyBox(){
		Destroy (theSpellBox);
	}
}