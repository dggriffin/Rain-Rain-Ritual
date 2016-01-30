using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellBuilder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var elements = new List<Element> () {
			new Element (ElementType.Fire, 8, 10),
			new Element (ElementType.Earth, 2, 4)
		};

		var audioSources = GetComponents<AudioSource>();
		print (audioSources [0].clip.name);
		var angryCow = audioSources [0];

		var volcano = new Spell ("volcano", elements, 3, 5, angryCow, angryCow);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
