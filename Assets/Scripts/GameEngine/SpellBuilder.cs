using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpellBuilder : MonoBehaviour {

	private int curSpellIndex = 0;
	private List<Spell> spellList = new List<Spell> ();
	private Spell curSpell = null;

	private bool isStarted = false;

	public AudioDictionary audioDict; //set in the UI

	// Use this for initialization
	void Start () {
		//StartSpell (); //StartSpell is started on the instruction button click/keypress
	}

	public void StartSpell(){
		if (!isStarted) {
			isStarted = true;

			spellList.Add (CreateRainSpell ("rain1"));
			spellList.Add (CreateRainSpell ("rain2"));
			spellList.Add (CreateRainSpell ("rain3"));

			StartFirstSpell ();
		} else {
			if (curSpell != null && IsSpellOver (curSpell)) {
				GetNextSpellOrEndGame ();
			} else {
				//cwkTODO figure out why this happens!
				Debug.Log ("somebody called start spell when the current spell is in progress");
			}
		}

	}

	private Spell CreateRainSpell (string name = "rain") {
		var elements = new List<Element> () {
			//new Element (ElementType.Fire, 0, 5, 5),
			//new Element (ElementType.Earth, 0, 5, 5),

			//cwkTODO put back water and wind settings
			//			new Element (ElementType.Water, 25, 32, 1.0f),
			//			new Element (ElementType.Wind, 25, 32, 0.5f)

			//cwkTODO easier settings for testing
			new Element (ElementType.Water, 5, 10, 1.0f),
			new Element (ElementType.Wind, 5, 10, 0.5f)
		};

		//cwkTODO put back spell
		//		var rain = new Spell ("rain", elements, 20, 120,
		//			audioDict.GetSound("thunderclap"), audioDict.GetSound("cloudfailure"));

		//cwkTODO easier settings for testing
		var rain = new Spell (name, elements, 5, 20,
			audioDict.GetSound("thunderclap"), audioDict.GetSound("cloudfailure"));

		return rain;
	}

	private void StartFirstSpell () {
		if (spellList == null || spellList.Count < 1) {
			return;
		}

		var firstSpell = spellList [0];
		firstSpell.OnStateChange += StartNextSpell;
		firstSpell.StartSpell ();
		curSpellIndex++;
		curSpell = firstSpell;
	}

	private void StartNextSpell (SpellState state, Spell spell) {
		Debug.Log (spell.Name + ": state is " + state);
		if (IsSpellOver (spell)) {
			StopSpell (spell);

			// Ref: http://answers.unity3d.com/questions/350721/c-yield-waitforseconds.html
			StartCoroutine (WaitThenStartNextSpell (spell));
		}
	}

	private bool IsSpellOver (Spell spell) {
		return spell.State == SpellState.Win || spell.State == SpellState.Lose;
	}

	private void StopSpell (Spell spell) {
		spell.OnStateChange -= StartNextSpell;
		spell.StopSpell ();
	}

	private IEnumerator WaitThenStartNextSpell (Spell spell) {
		Debug.Log ("Starting to wait: " + Time.time);

		// Wait to let the last spell's animation play a little bit
		// cwkTODO show UI here
		yield return new WaitForSeconds (1.0f);

		Debug.Log ("Done waiting: " + Time.time);

		if (curSpellIndex >= spellList.Count) {
			EndGame ();
		} else {
			ShowInstructions ();
		}
	}

	private void ShowInstructions () {
		var instructions = GameObject.Find ("RainInstructionBox");
		instructions.GetComponent<Image> ().canvas.enabled = true;
		//cwkTODO InputHandler is still handling the instructions button press, is that ok?
	}

	private void GetNextSpellOrEndGame () {
		var nextSpell = GetNextSpell ();

		if (nextSpell != null) {
			nextSpell.OnStateChange += StartNextSpell;
			nextSpell.StartSpell ();
		} else {
			EndGame ();
		}
	}

	private Spell GetNextSpell () {
		if (spellList == null || spellList.Count < 1 || curSpellIndex >= spellList.Count) {
			return null;
		}

		var nextSpell = spellList [curSpellIndex];

		curSpellIndex++;
		curSpell = nextSpell;

		return nextSpell;
	}

	private void EndGame () {
		List<string> states = new List<string>();
		int wins = 0;

		foreach (var spell in spellList) {
			states.Add (spell.State.ToString());
			if (spell.State == SpellState.Win) {
				wins++;
			}
		}

		var gameResults = string.Format("{0}/{1}: ({2})",
			wins,
			spellList.Count,
			string.Join(", ", states.ToArray())
		);

		Debug.Log ("GAME OVER: " + gameResults);
	}
}