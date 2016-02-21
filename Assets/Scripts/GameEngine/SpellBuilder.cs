using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellBuilder : MonoBehaviour {

	public AudioDictionary audioDict; //set in the UI

	// Use this for initialization
	void Start () {
		//StartSpell ();
	}

	public void StartSpell(){
		var elements = new List<Element> () {
			//new Element (ElementType.Fire, 0, 5, 5),
			//new Element (ElementType.Earth, 0, 5, 5),
			new Element (ElementType.Water, 25, 32, 1.0f),
			new Element (ElementType.Wind, 25, 32, 0.5f)
		};

		var rain = new Spell ("rain", elements, 20, 120,
			audioDict.GetSound("thunderclap"), audioDict.GetSound("cloudfailure"));

		//cwkTODO notes for SpellList / Level / SpellBook
		//initialize a list of spells
		//for each spell
		//create a spell and wait until it is won or lost -cwkTODO how to wait?
		//when that spell finishes (win or lose) - keep track
		//start the next spell
		//when all spells are done, report number of spells completed, e.g. 1/3

	}
}