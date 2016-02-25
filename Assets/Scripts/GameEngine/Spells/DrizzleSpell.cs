using System;
using UnityEngine;
using System.Collections.Generic;

// Super easy version of Rain
public class DrizzleSpell : RainSpell
{
	public DrizzleSpell (string name = null) 
		: base(name) {
	}

	protected override int NumTicksToWin {
		get {
			return 5;
		}
	}

	protected override int MaxTicksForSpell {
		get {
			return 30;
		}
	}

	protected override List<Element> ElementList {
		get {
			var elements = new List<Element> () {
				new Element (ElementType.Water, 5, 15, 1.0f),
				new Element (ElementType.Wind, 5, 15, 0.5f)
			};
			return elements;
		}
	}
}
