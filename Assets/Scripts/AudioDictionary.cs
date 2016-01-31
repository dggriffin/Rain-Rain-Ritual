using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioDictionary : MonoBehaviour {

	private IDictionary<string, AudioSource> dict = null;

	public AudioSource GetSound(string name) {
		if (dict == null) {
			dict = CreateAudioSourceDictionary ();
		}
		return dict [name];
	}

	private IDictionary<string, AudioSource> CreateAudioSourceDictionary() {
		var audioSources = GetComponents<AudioSource>();
		var dict = new Dictionary<string, AudioSource> ();
		foreach (var audioSource in audioSources) {
			dict.Add (audioSource.clip.name.ToLower(), audioSource);
		}
		return dict;
	}
}
