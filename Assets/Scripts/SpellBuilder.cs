using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellBuilder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var elements = new List<Element> () {
			new Element (ElementType.Fire, 80, 100),
			new Element (ElementType.Earth, 20, 40)
		};
		var volcano = new Spell ("volcano", elements);
		volcano.PrintElements ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
