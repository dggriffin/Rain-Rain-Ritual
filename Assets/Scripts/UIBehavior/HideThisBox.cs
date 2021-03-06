﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HideThisBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	public void HideBox(){
		//Don't use setActive because we can't access the object again later
		var instructionCanvas = GameObject.Find("InstructionCanvas").GetComponent<Canvas> ();
		instructionCanvas.enabled = false;
	}

    public void HideWinBox()
    {
		var winCanvas = GameObject.Find ("WinCanvas").GetComponent<Canvas> ();
		winCanvas.enabled = false;
    }

    public void HideLoseBox()
    {
		var loseCanvas = GameObject.Find ("LoseCanvas").GetComponent<Canvas> ();
		loseCanvas.enabled = false;
    }
}