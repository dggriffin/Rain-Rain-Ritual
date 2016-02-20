using UnityEngine;
using System.Collections;

public class ElementGoal : MonoBehaviour {

	//Element type the goals apply to
	public ElementType elementType;
	//X and Z scale for minimum count "ring"
	public float minScale;
	//X and Z scale for maximum count "ring"
	public float maxScale;

	// These numbers were purely based off observation:
	// a minScale ring of 0.05f was "filled" by light intensity at a value of 0.39f
	// We use this information to generate a percentage based off the current actual minScale
	private float goalScaleToIntensity {
		get {
			return 0.39f / 0.05f * (minScale / 0.05f);
		}
	}

	// These numbers were purely based off observation:
	// a minScale ring of 0.05f was "filled" by an element circle with a scale of 0.035f
	// We use this information to generate a percentage based off the current actual minScale
	private float goalScaleToCircle {
		get {
			return (0.035f / 0.05f) * (minScale / 0.05f);
		}
	}
	// Use this for initialization
	void Start () {
		this.minScale = GameObject.Find (elementType.ToString () + "Min").GetComponent<Transform> ().lossyScale.x;
		this.maxScale = GameObject.Find (elementType.ToString () + "Max").GetComponent<Transform> ().lossyScale.x;
	}

	public float getScaleCoefficientBasedOffGoalUI () {
		return this.minScale * goalScaleToCircle;
	}

	public float getIntensityCoefficientBasedOffGoalUI () {
		return this.minScale * goalScaleToIntensity;
	}

	public void updateMaxScale (float newMaxScale) {
		Vector3 newScale = GameObject.Find (elementType.ToString () + "Max").GetComponent<Transform> ().localScale;
		newScale.Set(newMaxScale, newScale.y, newMaxScale);
		GameObject.Find (elementType.ToString () + "Max").GetComponent<Transform> ().localScale = newScale;
		maxScale = newMaxScale;
	}
}
