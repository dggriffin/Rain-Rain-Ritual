using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellBuilder : MonoBehaviour {

	public AudioDictionary audioDict; //set in the UI

	// Use this for initialization
	void Start () {
		StartSpell ();
	}

	public void StartSpell(){
		var elements = new List<Element> () {
			new Element (ElementType.Fire, 0, 5),
			new Element (ElementType.Earth, 0, 5),
			new Element (ElementType.Water, 0, 20),
			new Element (ElementType.Wind, 0, 10)
		};

		var rain = new Spell ("rain", elements, 20, 100,
			audioDict.GetSound("thunderclap"), audioDict.GetSound("cloudfailure"));
	}
}
