using System;
using UnityEngine;
using System.Collections.Generic;

// Easy version of Rain
public class ShowerSpell : RainSpell
{
	public ShowerSpell (string name = null) 
		: base(name) {
	}

	protected override int NumTicksToWin {
		get {
			return 10;
		}
	}

	protected override int MaxTicksForSpell {
		get {
			return 60;
		}
	}

	protected override List<Element> ElementList {
		get {
			var elements = new List<Element> () {
				new Element (ElementType.Water, 15, 25, 1.0f),
				new Element (ElementType.Wind, 15, 25, 0.5f)
			};
			return elements;
		}
	}
}
