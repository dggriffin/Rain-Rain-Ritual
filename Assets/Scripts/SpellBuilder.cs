using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellBuilder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var elements = new List<Element> () {
			new Element (ElementType.Fire, 10, 100),
			new Element (ElementType.Earth, 0,0)
		};

		var audioSources = GetAudioSourceDictionary();
		var volcano = new Spell ("volcano", elements, 3, 30, audioSources["volcanoeruption"], audioSources["volcanorumble"]);
	}

	private IDictionary<string, AudioSource> GetAudioSourceDictionary() {
		var audioSources = GetComponents<AudioSource>();
		var dict = new Dictionary<string, AudioSource> ();
		foreach (var audioSource in audioSources) {
			dict.Add (audioSource.clip.name, audioSource);
		}
		return dict;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
