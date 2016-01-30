using UnityEngine;
using System.Collections;

public class Element {

	private ElementType type;
	private int minCount;
	private int maxCount;
	private int count;

	public Element(ElementType type, int minCount, int maxCount){
		this.type = type;
		this.minCount = minCount;
		this.maxCount = maxCount;
	}
		
	public void Increment(){
		count++;
	}

	public void Decay(){
		count--;
	}

	public bool IsInRange(){
		return (count <= maxCount && count >= minCount);
	}
}
