using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellBuilder : MonoBehaviour {

	public AudioDictionary audioDict; //set in the UI

	// Use this for initialization
	void Start () {
		var elements = new List<Element> () {
			new Element (ElementType.Fire, 10, 100),
			new Element (ElementType.Earth, 5,10),
			new Element (ElementType.Water, 10, 15),
			new Element (ElementType.Wind, 5,10)
		};

		var volcano = new Spell ("volcano", elements, 20, 100,
			audioDict.GetSound("volcanoeruption"), audioDict.GetSound("cloudfailure"));
	}
}
