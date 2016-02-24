using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellList : MonoBehaviour {

	private int curSpellIndex = 0;

	private List<Spell> spellList = new List<Spell> (); //cwkTODO rename to spells

	// Use this for initialization
	void Start () {
		//cwkTODO replace rain with other spells
		spellList.Add (CreateRainSpell ("rain1"));
		spellList.Add (CreateRainSpell ("rain2"));
		spellList.Add (CreateRainSpell ("rain3"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool HasNextSpell () {
		return spellList != null && curSpellIndex < spellList.Count;
	}

	//cwkTODO take into account user data
	public Spell GetNextSpell () {
		if (spellList == null || spellList.Count < 1 || curSpellIndex >= spellList.Count) {
			return null;
		}

		var nextSpell = spellList [curSpellIndex];

		curSpellIndex++;


		return nextSpell;
	}

	public string GetGameResults () {
		int wins = 0;

		List<string> states = new List<string>();

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

		return gameResults;
	}

	private Spell CreateRainSpell (string name = "rain") {
		//cwkTODO put back spell
		//		var rain = new RainSpell ("rain", 20, 120);

		//cwkTODO easier settings for testing
		var rain = new RainSpell (name, 5, 20);

		return rain;
	}
}
