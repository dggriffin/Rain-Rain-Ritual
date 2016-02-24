using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpellBuilder : MonoBehaviour {

	private bool isStarted = false;
	private SpellList spellList = null;
	private Spell curSpell = null;

	// Use this for initialization
	void Start () {
		//StartSpell (); //StartSpell is started on the instruction button click/keypress
	}

	public void StartSpell(){
		if (!isStarted) {
			isStarted = true;

			spellList = GameObject.Find ("SpellList").GetComponent<SpellList> ();

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
		
	private void StartFirstSpell () {
		var firstSpell = GetNextSpell ();

		if (firstSpell == null) {
			return;
		}

		firstSpell.OnStateChange += OnSpellOver;
		firstSpell.StartSpell ();
	}

	private void OnSpellOver (SpellState state, Spell spell) {
		Debug.Log (spell.Name + ": state is " + state);
		if (IsSpellOver (spell)) {
			StopSpell (spell);

			// Ref: http://answers.unity3d.com/questions/350721/c-yield-waitforseconds.html
			StartCoroutine (WaitThenShowNextInstructions (spell));
		}
	}

	private bool IsSpellOver (Spell spell) {
		return spell.State == SpellState.Win || spell.State == SpellState.Lose;
	}

	private void StopSpell (Spell spell) {
		spell.OnStateChange -= OnSpellOver;
		spell.StopSpell ();
	}

	private IEnumerator WaitThenShowNextInstructions (Spell spell) {
		Debug.Log ("Starting to wait: " + Time.time);

		// Wait to let the last spell's animation play a little bit
		yield return new WaitForSeconds (1.0f);

		Debug.Log ("Done waiting: " + Time.time);

		if (!spellList.HasNextSpell()) {
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
			nextSpell.OnStateChange += OnSpellOver;
			nextSpell.StartSpell ();
		} else {
			EndGame ();
		}
	}

	private Spell GetNextSpell () {
		var nextSpell = spellList.GetNextSpell ();
		curSpell = nextSpell;
		return nextSpell;
	}

	private void EndGame () {
		var gameResults = spellList.GetGameResults ();
		Debug.Log ("GAME OVER: " + gameResults);
	}
}