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
	}

	private void StartNextSpell (SpellState state, Spell spell) {
		Debug.Log (spell.Name + ": state is " + state);
		if (state == SpellState.Win || state == SpellState.Lose) {
			spell.StopSpell ();

			//cwkTODO keep track of how many wins

			// Ref: http://answers.unity3d.com/questions/350721/c-yield-waitforseconds.html
			StartCoroutine (WaitThenStartNextSpell (spell));
		}
	}

	private IEnumerator WaitThenStartNextSpell (Spell spell) {
		Debug.Log ("Starting to wait: " + Time.time);

		// Wait to let the last spell's animation play a little bit
		// cwkTODO show UI here
		yield return new WaitForSeconds (1.0f);

		Debug.Log ("Done waiting: " + Time.time);

		var nextSpell = GetNextSpell ();

		if (nextSpell != null) {
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