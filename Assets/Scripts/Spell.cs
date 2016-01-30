using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell {
	private string name;
	private IDictionary<ElementType, Element> elements =  new Dictionary<ElementType, Element>();
	private GameObject metroObject; //TODO delete later

	public Spell(string name, IList<Element> elements){
		this.name = name;
		this.elements = new Dictionary<ElementType, Element>();
		foreach (var element in elements) {
			this.elements.Add (element.Type, element);
		}

		ListenToEvents ();
	}

	private void ListenToEvents() {
		//TODO: change this to listen to the beat event
		metroObject = GameObject.Find("Metronome");
		metroObject.GetComponent<Metronome>().OnTick += IncrementWrapper;
	}

	//TODO remove once we get beat event
	private void IncrementWrapper(Metronome metro) {
		Increment (ElementType.Fire);
		PrintElements ();
	}

	private void Increment(ElementType elementType){		
		var element = getElement(elementType);
		element.Increment();
	}

	private Element getElement(ElementType elementType){
		return elements [elementType];
	}

	public void PrintElements () {
		Debug.Log(string.Format("Spell: {0}", this.name));
		foreach (var element in elements) {
			element.Value.Print ();
		}
	}
}