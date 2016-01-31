using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell {
	private string name;
	private IDictionary<ElementType, Element> elements =  new Dictionary<ElementType, Element>();

	private GameObject inputHandler;
	private GameObject metronome;

	private int numTicksToWin;
	private int maxTicksForSpell;
	private int numTicksInRange = 0;
	private int numTicksElapsed = 0;

	private AudioSource winSound = null;
	private AudioSource loseSound = null;

	private GameObject rain = null;
	private GameObject dryGround = null;
	private GameObject wetGround = null;

	private GameObject theme = null;

	public Spell(string name, IList<Element> elements, int numTicksToWin, int maxTicksForSpell,
		AudioSource winSound = null, AudioSource loseSound = null){
		this.name = name;
		this.elements = new Dictionary<ElementType, Element>();
		foreach (var element in elements) {
			this.elements.Add (element.Type, element);
		}
		this.numTicksToWin = numTicksToWin;
		this.maxTicksForSpell = maxTicksForSpell;

		this.winSound = winSound;
		this.loseSound = loseSound;

		this.rain = GameObject.Find("VFX_Rain");
		if (rain != null) {
			rain.SetActive (false); // no rain at the beginning of the spell
		}

		this.dryGround = GameObject.Find ("DryGround");
		if (dryGround != null) {
			dryGround.SetActive (true);
		}

		this.wetGround = GameObject.Find ("WetGround");
		if (wetGround != null) {
			wetGround.SetActive (false);
		}


		this.theme = GameObject.Find ("ThemeSource");

		ListenToEvents ();
	}

	private void ListenToEvents() {
		inputHandler = GameObject.Find ("InputHandler");
		inputHandler.GetComponent<InputHandler> ().ElementEvent += Increment;

		metronome = GameObject.Find ("Metronome");
		metronome.GetComponent<Metronome>().OnTick += RangeCheck;
		metronome.GetComponent<Metronome>().OnTick += Decay;
	}
		
	private void Increment(ElementType elementType){
		var element = getElement(elementType);
		if (element == null) {
			return;
		}
		element.Increment();
        incrementElement(element);
//		PrintElements ();
	}

	private void Decay(){
		foreach (var element in elements) {
			element.Value.Decay ();
            decrementElement(element.Value);
		}
		//Debug.Log ("Decaying");
		//PrintElements ();
	}

	private void RangeCheck() {
		if (numTicksElapsed > maxTicksForSpell) {
			return;
		}

		numTicksElapsed++;
		if (allElementsInRange ()) {
			Debug.Log ("*************************");
			numTicksInRange++;

		} else {
			numTicksInRange = 0;
		}

		GameObject.Find ("Cloud").GetComponent<CloudBehavior> ().growResult (((float)numTicksInRange/(float)numTicksToWin)/2f);

		if (numTicksInRange == numTicksToWin) {
			win ();
		}

		if (numTicksElapsed > maxTicksForSpell) {
			lose ();
		}
	}

	private void win() {
		Debug.Log ("YOU WIN!" + "Elapsed: " + numTicksElapsed);
		if (winSound != null) {
			winSound.Play ();
		}
		if (rain != null) {
			rain.SetActive (true);
		}

		if (dryGround != null) {
			dryGround.SetActive (false);
		}

		if (wetGround != null) {
			wetGround.SetActive (true);
		}

		GameObject.Find ("Cloud").GetComponent<CloudBehavior> ().winResult ();
		resetGame ();
	}

	private void lose() {
		Debug.Log ("YOU LOSE! (too many ticks) Elapsed: " + numTicksElapsed);
		if (loseSound != null) {
			loseSound.Play ();
		}

		if (rain != null) {
			rain.SetActive (false);
		}

		if (dryGround != null) {
			dryGround.SetActive (true);
		}

		if (wetGround != null) {
			wetGround.SetActive (false);
		}

		theme.SendMessage ("StopMusic");
		
		GameObject.Find ("Cloud").GetComponent<CloudBehavior> ().loseResult ();
		resetGame ();
	}

	private bool allElementsInRange() {
		bool allInRange = true;
		foreach (var element in elements) {
			allInRange = allInRange && element.Value.IsInRange();
		}
		return allInRange;
	}

	private Element getElement(ElementType elementType){
		return elements.ContainsKey(elementType) ? elements [elementType] : null;
	}

	public void PrintElements () {
		Debug.Log(string.Format("Spell: {0}", this.name));
		foreach (var element in elements) {
			element.Value.Print ();
		}
	}

    private void incrementElement(Element element)
    {
        if (element.Type.Equals(ElementType.Fire))
        {
            GameObject.Find("Fire").GetComponent<Pulse>().fadeIn(element);
        }
        else if (element.Type.Equals(ElementType.Earth))
        {
            GameObject.Find("Earth").GetComponent<Pulse>().fadeIn(element);
        }
        else if (element.Type.Equals(ElementType.Wind))
        {
            GameObject.Find("Wind").GetComponent<Pulse>().fadeIn(element);
        }
        else if (element.Type.Equals(ElementType.Water))
        {
            GameObject.Find("Water").GetComponent<Pulse>().fadeIn(element);
        }
    }
    private void decrementElement(Element element)
    {
        if (element.Type.Equals(ElementType.Fire))
        {
            GameObject.Find("Fire").GetComponent<Pulse>().fadeOut(element);
        }
        else if (element.Type.Equals(ElementType.Earth))
        {
            GameObject.Find("Earth").GetComponent<Pulse>().fadeOut(element);
        }
        else if (element.Type.Equals(ElementType.Wind))
        {    
            GameObject.Find("Wind").GetComponent<Pulse>().fadeOut(element);
        }
        else if (element.Type.Equals(ElementType.Water))
        {
            GameObject.Find("Water").GetComponent<Pulse>().fadeOut(element);
        }
    }

	private void resetGame(){
		numTicksInRange = 0;
		numTicksElapsed = 0;

		foreach (var element in elements) {
			element.Value.count = 0;
		}
	}

}