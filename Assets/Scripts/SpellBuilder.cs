using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellBuilder : MonoBehaviour {

	public AudioDictionary audioDict; //set in the UI

	// Use this for initialization
	void Start () {
		var elements = new List<Element> () {
			new Element (ElementType.Fire, 8, 10),
			new Element (ElementType.Earth, 2, 4)
		};

		var volcano = new Spell ("volcano", elements, 10, 10000,
			audioDict.GetSound("volcanoeruption"), audioDict.GetSound("volcanorumble"));
	}
}
