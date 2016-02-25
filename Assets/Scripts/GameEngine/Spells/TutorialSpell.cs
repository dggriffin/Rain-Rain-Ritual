using System;
using UnityEngine;
using System.Collections.Generic;

public class TutorialSpell : RainSpell // TODO if we ever integrate other assets, don't inherit from Rain
{
	public TutorialSpell (string name = null) 
		: base(name) {
	}

	protected override int NumTicksToWin {
		get {
			return 5;
		}
	}

	protected override int MaxTicksForSpell {
		get {
			return 120;
		}
	}

	protected override List<Element> ElementList {
		get {
			var elements = new List<Element> () {
				new Element (ElementType.Earth, 5, 10, 0.01f),
				new Element (ElementType.Wind, 5, 10, 0.01f),
				new Element (ElementType.Fire, 5, 10, 0.01f),
				new Element (ElementType.Water, 5, 10, 0.01f)
			};
			return elements;
		}
	}
}
