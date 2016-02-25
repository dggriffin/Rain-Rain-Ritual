using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

	public void Start(){
		var audioPlay = GetComponent<AudioSource>();
		audioPlay.Play ();
	}

	public void LoadScene(int scene){
		SceneManager.LoadScene (scene);
		var spellBuilder = new SpellBuilder ();
		spellBuilder.StartSpell ("DrizzleSpell");
	}
}
