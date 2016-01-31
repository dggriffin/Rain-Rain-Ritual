using UnityEngine;
using System.Collections;

public class Element {

	private ElementType type;
    public int minCount;
    public int maxCount;
	public int count = 0;

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
		if (this.count > 0) {
			this.count--;
		}
	}

	public bool IsInRange(){
		return (count <= maxCount && count >= minCount);
	}

	public void Print() {
		Debug.Log (string.Format("{0}: {1}", type, count));
	}
}
