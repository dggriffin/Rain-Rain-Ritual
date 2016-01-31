using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellBuilder : MonoBehaviour {

	public AudioDictionary audioDict; //set in the UI

	// Use this for initialization
	void Start () {
		var elements = new List<Element> () {
			new Element (ElementType.Fire, 10, 100),
			new Element (ElementType.Earth, 0,0)
		};
			
		var volcano = new Spell ("volcano", elements, 5, 5,
			audioDict.GetSound("volcanoeruption"), audioDict.GetSound("volcanorumble"));
	}
}
