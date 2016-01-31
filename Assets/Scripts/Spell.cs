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
//		growResult ();
        incrementElement(element);
		PrintElements ();
	}

	private void Decay(){
//		growResult ();
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
			numTicksInRange++;

		} else {
			numTicksInRange = 0;
		}

		growResult ();

		if (numTicksInRange == numTicksToWin) {
			win ();
		}

//		// TODO: CLEAN THIS UP:::
//		// RESET THE GAME
//		if (numTicksInRange > numTicksToWin + 5) {
//			GameObject.Find ("Cloud").GetComponent<CloudBehavior> ().resetResult();
//			numTicksInRange = 0;
//			numTicksElapsed = 0;
//		}

		if (numTicksElapsed > maxTicksForSpell) {
			lose ();
		}
	}

	private void win() {
		Debug.Log ("YOU WIN!" + "Elapsed: " + numTicksElapsed);
		if (winSound != null) {
			winSound.Play ();
			//				GameObject.Find ("Cloud").GetComponent<CloudBehavior> ().winResult ();
			if (rain != null) {
				rain.SetActive (true);
			}
		}
	}

	private void lose() {
		Debug.Log ("YOU LOSE! (too many ticks) Elapsed: " + numTicksElapsed);
		if (loseSound != null) {
			loseSound.Play ();
		}

		if (rain != null) {
			rain.SetActive (false);
		}
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

	private void growResult()
	{
		GameObject.Find ("Cloud").GetComponent<CloudBehavior> ().growResult (numTicksInRange/numTicksToWin);
	}


}