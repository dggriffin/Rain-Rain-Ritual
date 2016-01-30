using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell {
	private string name;
	private IDictionary<ElementType, Element> elements =  new Dictionary<ElementType, Element>();

	public Spell(string name, IList<Element> elements){
		this.name = name;
		this.elements = new Dictionary<ElementType, Element>();
		foreach (var element in elements) {
			this.elements.Add (element.Type, element);
		}
	}

	public void Increment(ElementType elementType){		
		var element = getElement(elementType);
		element.Increment();
	}

	private Element getElement(ElementType elementType){
		return elements [elementType];
	}

	public void PrintElements () {
		foreach (var element in elements) {
			element.Value.Print ();
		}
	}
}