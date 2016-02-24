using UnityEngine;
using System.Collections;

public class UserData : MonoBehaviour {

	//cwkTODO short term solution
	// just add the spell type to the list to show that the user beat it

	//cwkTODO long term solution
	//cwkTODO might have to do something like this: http://answers.unity3d.com/questions/610893/how-do-i-save-a-custom-class-of-variables-to-playe.html
	//when a spell is won/lost
	/*
	 * RainSpell : { wins: 5, losses: 3 } //computed value: ratio: 5/8
	 * if a spell is not there, add it
	 * if a spell is not there, it means it is locked
	 * if a spell is there, it means it is unlocked
	 */

	SpellBuilder spellBuilder;

	// Use this for initialization
	void Start () {

		//For DEV: comment the following line to reset the preferences
		//PlayerPrefs.DeleteAll ();

		//PlayerPrefs.SetString ("test", "hello world");
		//var testVal = PlayerPrefs.GetString ("test");

		spellBuilder = GameObject.Find("SpellBuilder").GetComponent<SpellBuilder> ();
		spellBuilder.OnSpellComplete += SaveSpellResult;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void SaveSpellResult (Spell spell) {
		Debug.Log ("save spell result!");
		var spellType = spell.GetType ().ToString ();

		if (spell.State == SpellState.Win) {
			if (!HasUserCompletedSpell (spellType)) {
				PlayerPrefs.SetString (spellType, spellType); // value does not matter right now
			}
		}
	}

	private bool HasUserCompletedSpell (string spellType)
	{
		var savedSpellResult = PlayerPrefs.GetString (spellType);
		return !string.IsNullOrEmpty(savedSpellResult);
	}
}
