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

	public ElementType Type {
		get { return this.type; }
	}
		
	public void Increment(){
		this.count++;
	}

	public void Decay(){
		this.count--;
	}

	public bool IsInRange(){
		return (count <= maxCount && count >= minCount);
	}

	public void Print() {
		Debug.Log (string.Format("{0}: {1}", type, count));
	}
}
