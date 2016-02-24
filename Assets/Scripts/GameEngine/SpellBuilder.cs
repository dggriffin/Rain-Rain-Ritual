using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpellBuilder : MonoBehaviour {

	private int curSpellIndex = 0;
	private List<Spell> spellList = new List<Spell> ();
	private Spell curSpell = null;

	private bool isStarted = false;

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
			// Only get the next spell if the current spell is done
			// (i.e. in case this gets called when a spell is in progress, don't get the next spell yet)
			if (curSpell != null && IsSpellOver (curSpell)) {
				GetNextSpellOrEndGame ();
			} else {
				//cwkTODO figure out why this happens!
				//for some reason button click is called multiple times after spacebara is pressed on the instructions
				Debug.Log ("somebody called start spell when the current spell is in progress");
			}
		}

	}

	private Spell CreateRainSpell (string name = "rain") {
		//cwkTODO put back spell
		//		var rain = new RainSpell ("rain", 20, 120);

		//cwkTODO easier settings for testing
		var rain = new RainSpell (name, 5, 20);

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