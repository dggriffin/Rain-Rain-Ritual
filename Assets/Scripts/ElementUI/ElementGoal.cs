using UnityEngine;
using System.Collections;

public class ElementGoal : MonoBehaviour {

	//Element type the goals apply to
	public ElementType elementType;
	//X and Z scale for minimum count "ring"
	public float minScale;
	//X and Z scale for maximum count "ring"
	public float maxScale;

	private float goalScaleToIntensity {
		get {
			return 0.39f / 0.05f * (minScale / 0.05f);
		}
	}

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
