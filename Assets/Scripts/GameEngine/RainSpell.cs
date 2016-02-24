using System;
using UnityEngine;
using System.Collections.Generic;

public class RainSpell : Spell
{
	private GameObject cloud = null;
	private GameObject rain = null;

	public RainSpell (string name, IList<Element> elements, int numTicksToWin, int maxTicksForSpell) 
		: base(name, elements, numTicksToWin, maxTicksForSpell) {
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
			this.cloud.GetComponent<CloudBehavior> ().growResult (((float)numTicksInRange / (float)numTicksToWin) / 2f);
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

