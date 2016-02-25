using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpellBuilder : MonoBehaviour {

	private bool isStarted = false;
	private SpellList spellList = null;
	private Spell curSpell = null;

	public delegate void SpellCompleteEvent (Spell spell);
	public event SpellCompleteEvent OnSpellComplete;

	// Use this for initialization
	void Start () {
		//StartSpell (); //StartSpell is started on the instruction button click/keypress
	}

	public void StartSpell(string spellType = null){
		if (!isStarted) {
			isStarted = true;

			spellList = GameObject.Find ("SpellList").GetComponent<SpellList> ();

			GetNextSpellOrEndGame (spellType);
		} else {
			// Only get the next spell if the current spell is done
			// (i.e. in case this gets called when a spell is in progress, don't get the next spell yet)
			if (curSpell != null && IsSpellOver (curSpell)) {
				GetNextSpellOrEndGame (spellType);
			} else {
				//cwkTODO figure out why this happens!
				//for some reason button click is called multiple times after spacebara is pressed on the instructions
				//Debug.Log ("somebody called start spell when the current spell is in progress");
			}
		}

	}

	private void OnSpellOver (SpellState state, Spell spell) {
		//Debug.Log (spell.Name + ": state is " + state);
		if (IsSpellOver (spell)) {
			StopSpell (spell);

			if (OnSpellComplete != null) {
				OnSpellComplete (spell);
			}

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
		//Debug.Log ("Starting to wait: " + Time.time);

		// Wait to let the last spell's animation play a little bit
		yield return new WaitForSeconds (1.0f);

		//Debug.Log ("Done waiting: " + Time.time);

		if (!spellList.HasNextSpell()) {
			EndGame ();
		} else {
			ShowInstructions ();
		}
	}

	private void ShowInstructions () {
		//cwkTODO InputHandler is still handling the instructions button press, is that ok?
		var instructionCanvas = GameObject.Find("InstructionCanvas").GetComponent<Canvas> ();
		instructionCanvas.enabled = true;
	}

	private void GetNextSpellOrEndGame (string spellType = null) {
		Spell nextSpell;

		if (string.IsNullOrEmpty(spellType)) {
			nextSpell = spellList.GetNextSpell ();
		} else {
			nextSpell = spellList.GetSpellOfType (spellType);
		}

		curSpell = nextSpell;

		if (nextSpell != null) {
			nextSpell.OnStateChange += OnSpellOver;
			nextSpell.StartSpell ();
		} else {
			EndGame ();
		}
	}

	private void EndGame () {
		var gameResults = spellList.GetGameResults ();
		Debug.Log ("GAME OVER: " + gameResults);
	}
}