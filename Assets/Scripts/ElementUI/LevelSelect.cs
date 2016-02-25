using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//cwkTODO HACK remove this
		var instructionCanvas = GameObject.Find("InstructionCanvas").GetComponent<Canvas> ();
		instructionCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HideLevelSelectCanvas () {
		var canvas = GameObject.Find ("LevelSelectCanvas").GetComponent<Canvas> ();
		canvas.enabled = false;
	}
}
