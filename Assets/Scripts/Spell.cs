using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell {
	private string name;
	private IDictionary<ElementType, Element> elements =  new Dictionary<ElementType, Element>();

	public Spell(string name, IDictionary<ElementType, Element> elements){
		this.name = name;
		this.elements = elements;
	}

	public void Increment(ElementType elementType){		
		var element = getElement(elementType);
		element.Increment();
	}

	private Element getElement(ElementType elementType){
		return elements [elementType];
	}
}