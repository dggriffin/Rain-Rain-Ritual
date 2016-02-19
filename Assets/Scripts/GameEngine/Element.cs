using UnityEngine;
using System.Collections;

public class Element {

	private ElementType type;
	public ElementGoal elementGoal;
    public int minCount;
    public int maxCount;
	public float count = 0;
	public float decayRate;

	public Element(ElementType type, int minCount, int maxCount, float decayRate){
		this.type = type;
		this.minCount = minCount;
		this.maxCount = maxCount;
		this.decayRate = decayRate;
		this.elementGoal = GameObject.Find (this.type.ToString () + "Goals").GetComponent<ElementGoal> ();
		setupElementGoal ();
	}

	public ElementType Type {
		get { return this.type; }
	}
		
	public void Increment(){
		this.count++;
	}

	public void Decay(){
		if (this.count > 0) {
			this.count = this.count - this.decayRate;
		}
	}

	public bool IsInRange(){
		return (count <= maxCount && count >= minCount);
	}

	public void Print() {
		Debug.Log (string.Format("{0}: {1}", type, count));
	}

	private void setupElementGoal() {
		elementGoal.elementType = type;
		float percentIncrease = (float) maxCount / minCount;
		elementGoal.updateMaxScale (elementGoal.minScale * percentIncrease);
	}
}
