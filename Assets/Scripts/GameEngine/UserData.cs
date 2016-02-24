using UnityEngine;
using System.Collections;

public class UserData : MonoBehaviour {

	//cwkTODO notes
	//cwkTODO might have to do something like this: http://answers.unity3d.com/questions/610893/how-do-i-save-a-custom-class-of-variables-to-playe.html
	//when a spell is won/lost
	/*
	 * RainSpell : { wins: 5, losses: 3 } //computed value: ratio: 5/8
	 * if a spell is not there, add it
	 * if a spell is not there, it means it is locked
	 * if a spell is there, it means it is unlocked
	 * /

	// Use this for initialization
	void Start () {

		//cwkTODO delete all to clean up once
		//PlayerPrefs.SetString ("test", "hello world");
		//var testVal = PlayerPrefs.GetString ("test");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
