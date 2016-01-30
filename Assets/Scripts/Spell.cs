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

		inputHandler.GetComponent<InputHandler> ().ElementEvent += Increment;
	}
		
	private void Increment(ElementType elementType){		
		var element = getElement(elementType);
		if (element == null) {
			return;
		}
		element.Increment();
		PrintElements ();
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