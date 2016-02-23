using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellBuilder : MonoBehaviour {

	private int curSpellIndex = 0;
	private List<Spell> spellList = new List<Spell> ();

	public AudioDictionary audioDict; //set in the UI

	// Use this for initialization
	void Start () {
		//StartSpell ();
	}

	public void StartSpell(){
		var elements = new List<Element> () {
			//new Element (ElementType.Fire, 0, 5, 5),
			//new Element (ElementType.Earth, 0, 5, 5),

			//cwkTODO put back water and wind settings
//			new Element (ElementType.Water, 25, 32, 1.0f),
//			new Element (ElementType.Wind, 25, 32, 0.5f)
			new Element (ElementType.Water, 5, 10, 1.0f),
			new Element (ElementType.Wind, 5, 10, 0.5f)
		};

		//cwkTODO put back spell
//		var rain = new Spell ("rain", elements, 20, 120,
//			audioDict.GetSound("thunderclap"), audioDict.GetSound("cloudfailure"));
		var rain = new Spell ("rain", elements, 5, 20,
			audioDict.GetSound("thunderclap"), audioDict.GetSound("cloudfailure"));

		spellList.Add (rain);

		rain.OnStateChange += StartNextSpell;

		rain.StartSpell ();

		curSpellIndex++;

		var rain2 = new Spell ("rain2", elements, 5, 20,
			audioDict.GetSound("thunderclap"), audioDict.GetSound("cloudfailure"));

		spellList.Add (rain2);

		//cwkTODO notes for SpellList / Level / SpellBook
		//initialize a list of spells
		//for each spell
		//create a spell and wait until it is won or lost -cwkTODO how to wait?
		//when that spell finishes (win or lose) - keep track
		//start the next spell
		//when all spells are done, report number of spells completed, e.g. 1/3

		//cwkTODO
		//create a separate initialize/start method from the spell constructor
		//so that we can make spells and put them in the list, but not start them yet
		//create SpellState class with Win, Lose, InProgress
		//while true, get spell state, if InProgress, sleep for 1 second
		//if spell state is win or lose, update the appropriate counter
		//start next spell

		//cwkTODO
		//fill array with spells
		//if array has at least one spell
		//take the first spell
		//start it pass it a callback for when it is "done"
		//callback is back here
		//callback increments stats, takes next spell from array
		//start the spell and pass it the same callback
		//if the list does not have any more spells, call the compute score method
		//which prints out the tally of all the spells

		//cwkTODO after talking with greyson
		//figure out how game talks to canvas
	}

	private void StartNextSpell (SpellState state, Spell spell) {
		Debug.Log (spell.Name + ": state is " + state);
		if (state == SpellState.Win || state == SpellState.Lose) {
			var nextSpell = GetNextSpell ();

			if (nextSpell == null) {
				return;
			}

			nextSpell.OnStateChange += StartNextSpell;

			nextSpell.StartSpell ();
		}
	}

	private Spell GetNextSpell () {
		if (spellList == null || spellList.Count < 1) {
			return null;
		}

		if (curSpellIndex >= spellList.Count) {
			Debug.Log ("GAME OVER");
			return null;
		}

		var nextSpell = spellList [curSpellIndex];

		curSpellIndex++;

		return nextSpell;
	}
}