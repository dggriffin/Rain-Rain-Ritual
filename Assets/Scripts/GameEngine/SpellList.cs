using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellList : MonoBehaviour {

	private int curSpellIndex = 0;

	private List<Spell> spells = new List<Spell> ();

	// Use this for initialization
	void Start () {
		//cwkTODO replace rain with other spells
		spells.Add (CreateRainSpell ("rain1"));
		spells.Add (CreateRainSpell ("rain2"));
		spells.Add (CreateRainSpell ("rain3"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool HasNextSpell () {
		return spells != null && curSpellIndex < spells.Count;
	}

	public Spell GetSpellOfType (string type) {
		foreach (var spell in spells) {

			curSpellIndex++;

			var spellType = spell.GetType ().ToString ();
			if (spellType == type) {
				return spell;
			}
		}
		return null;
	}

	//cwkTODO pass in a spell
	//cwkTODO take into account user data
	public Spell GetNextSpell () {
		if (spells == null || spells.Count < 1 || curSpellIndex >= spells.Count) {
			return null;
		}

		var nextSpell = spells [curSpellIndex];

		curSpellIndex++;

		return nextSpell;
	}

	public string GetGameResults () {
		int wins = 0;

		List<string> states = new List<string>();

		foreach (var spell in spells) {
			states.Add (spell.State.ToString());
			if (spell.State == SpellState.Win) {
				wins++;
			}
		}

		var gameResults = string.Format("{0}/{1}: ({2})",
			wins,
			spells.Count,
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
