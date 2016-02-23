using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuScreenUI : MonoBehaviour {

	public GameObject helpScreen;

	public void ShowHelp(){
		helpScreen.SetActive (true);
	}
	
	public void HideHelp(){
		helpScreen.SetActive (false);
	}
}