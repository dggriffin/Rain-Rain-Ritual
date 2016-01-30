using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell {
	private string name;
	private IDictionary<ElementType, Element> elements =  new Dictionary<ElementType, Element>();
	private GameObject inputHandler, metronome;
	private int numTicksToWin;
	private int maxTicksForSpell;
	private int numTicksInRange = 0;
	private int numTicksElapsed = 0;

	public Spell(string name, IList<Element> elements, int numTicksToWin, int maxTicksForSpell){
		this.name = name;
		this.elements = new Dictionary<ElementType, Element>();
		foreach (var element in elements) {
			this.elements.Add (element.Type, element);
		}
		this.numTicksToWin = numTicksToWin;
		this.maxTicksForSpell = maxTicksForSpell;

		ListenToEvents ();
	}

	private void ListenToEvents() {
		inputHandler = GameObject.Find ("InputHandler");
		inputHandler.GetComponent<InputHandler> ().ElementEvent += Increment;

		metronome = GameObject.Find ("Metronome");
		metronome.GetComponent<Metronome>().OnTick += RangeCheck;
		metronome.GetComponent<Metronome>().OnTick += Decay;
	}
		
	private void Increment(ElementType elementType){		
		var element = getElement(elementType);
		if (element == null) {
			return;
		}
		element.Increment();
        incrementElement(element);
		PrintElements ();
	}

	private void Decay(){
		foreach (var element in elements) {
			element.Value.Decay ();
		}
		//Debug.Log ("Decaying");
		//PrintElements ();
	}

	private void RangeCheck() {
		numTicksElapsed++;

		if (allElementsInRange()) {
			numTicksInRange++;
		} else {
			numTicksInRange = 0;
		}

		if (numTicksInRange == numTicksToWin) {
			Debug.Log ("YOU WIN!" + "Elapsed: " + numTicksElapsed);
		}

		if (numTicksElapsed > maxTicksForSpell) {
			Debug.Log ("YOU LOSE! (too many ticks) Elapsed: " + numTicksElapsed);
		}
	}

	private bool allElementsInRange() {
		bool allInRange = true;
		foreach (var element in elements) {
			allInRange = allInRange && element.Value.IsInRange();
		}
		return allInRange;
	}

	private Element getElement(ElementType elementType){
		return elements.ContainsKey(elementType) ? elements [elementType] : null;
	}

	public void PrintElements () {
		Debug.Log(string.Format("Spell: {0}", this.name));
		foreach (var element in elements) {
			element.Value.Print ();
		}
	}

    private void incrementElement(Element element)
    {
        GameObject.Find("Fire").GetComponent<Pulse>().fadeIn();
    }
}