using System;
using UnityEngine;
using System.Collections.Generic;

public class WarSpell : Spell
{
	private GameObject cloud = null;
	private GameObject rain = null;

	public WarSpell (string name = null) 
		: base(name) {
	}

	protected override int NumTicksToWin {
		get {
			return 20; // original
			//return 5; // easier setting for testing
		}
	}

	protected override int MaxTicksForSpell {
		get {
			return 120; // original
			//return 20; // easier setting for testing
		}
	}

	protected override List<Element> ElementList {
		get {
			var elements = new List<Element> () {
				new Element (ElementType.Water, 25, 32, 1.0f),
				new Element (ElementType.Wind, 25, 32, 0.5f)
			};
			return elements;
		}
	}

	protected override AudioSource WinSound {
		get { return audioDict.GetSound ("thunderclap"); }
	}

	protected override AudioSource LoseSound {
		get { return audioDict.GetSound ("cloudfailure"); }
	}

	protected override void CenterObjectInitialize () {
		this.cloud = GameObject.Find ("Cloud");
		//cwkTODO ask Christina why cloud is warping initially
		if (this.cloud != null) {
			this.cloud.GetComponent<CloudBehavior> ().reset ();
			this.cloud.GetComponent<CloudBehavior> ().growResult (0);
		}
	}

	protected override void CenterObjectUpdate () {
		if (this.cloud != null) {
			this.cloud.GetComponent<CloudBehavior> ().growResult (((float)numTicksInRange / (float)NumTicksToWin) / 2f);
		}
	}

	protected override void CenterObjectWin () {
		if (this.cloud != null) {
			this.cloud.GetComponent<CloudBehavior> ().winResult ();
		} else {
			Debug.Log ("cloud is null in win");
		}
	}

	protected override void CenterObjectLose () {
		if (this.cloud != null) {
			this.cloud.GetComponent<CloudBehavior> ().loseResult ();
		} else {
			Debug.Log ("cloud is null is lose");
		}
	}

	protected override void WinAnimationInitialize () {
		this.rain = GameObject.Find("VFX_Rain");
	}

	protected override void ShowWinAnimation (bool show) {
		if (rain != null) {
			foreach (Transform t in rain.transform) {
				t.GetComponent<MeshRenderer> ().enabled = show;
			}
		}
	}
}

