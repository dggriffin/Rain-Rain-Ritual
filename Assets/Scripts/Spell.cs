using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell {
	private string name;
	private IDictionary<ElementType, Element> elements =  new Dictionary<ElementType, Element>();
	private GameObject inputHandler;

	public Spell(string name, IList<Element> elements){
		this.name = name;
		this.elements = new Dictionary<ElementType, Element>();
		foreach (var element in elements) {
			this.elements.Add (element.Type, element);
		}

		ListenToEvents ();
	}

	private void ListenToEvents() {
		//INPUTHANDLER EXAMPLE
		inputHandler = GameObject.Find ("InputHandler");

		inputHandler.GetComponent<InputHandler> ().FireEvent += FireWrapper;
		inputHandler.GetComponent<InputHandler> ().WaterEvent += WaterWrapper;
		inputHandler.GetComponent<InputHandler> ().AirEvent += WindWrapper;
		inputHandler.GetComponent<InputHandler> ().EarthEvent += EarthWrapper;
		//inputHandler.GetComponent<InputHandler> ().OffBeatEvent += IncrementWrapper;
	}

	private void EarthWrapper(InputHandler handler) {
		Increment (ElementType.Earth);
		PrintElements ();
	}

	private void WindWrapper(InputHandler handler) {
		Increment (ElementType.Wind);
		PrintElements ();
	}

	private void FireWrapper(InputHandler handler) {
		Increment (ElementType.Fire);
		PrintElements ();
	}
		
	private void WaterWrapper(InputHandler handler) {
		Increment (ElementType.Water);
		PrintElements ();
	}

	private void Increment(ElementType elementType){		
		var element = getElement(elementType);
		if (element == null) {
			return;
		}
		element.Increment();
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
}