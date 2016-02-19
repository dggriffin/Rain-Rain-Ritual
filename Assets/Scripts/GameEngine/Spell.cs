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
	private GameObject cloud = null;

	private GameObject theme = null;

	private GameObject winBox = null;
	private GameObject loseBox = null;

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
			//rain.SetActive (false); // no rain at the beginning of the spell
		}

		this.dryGround = GameObject.Find ("DryGround");
		if (dryGround != null) {
			dryGround.SetActive (true);
		}

		this.wetGround = GameObject.Find ("WetGround");
		if (wetGround != null) {
			wetGround.SetActive (false);
		}

		this.cloud = GameObject.Find ("Cloud");

		this.theme = GameObject.Find ("ThemeSource");

//		this.winBox = GameObject.Find ("Canvas").GetCom("RainWinBox");

//		foreach (Transform t in GameObject.Find("Canvas").transform) {
//			if (t.name == "RainWinBox") {
//				t.GetComponent<CanvasRenderer>().
//			}
//		}
		this.loseBox = GameObject.Find ("RainLoseBox");

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
		updateElementUI(element);
		//		PrintElements ();
	}

	private void Decay(){
		foreach (var element in elements) {
			element.Value.Decay ();
			updateElementUI(element.Value);
		}
		//Debug.Log ("Decaying");
		//PrintElements ();
	}

	private void RangeCheck() {
		if (numTicksElapsed > maxTicksForSpell) {
			lose ();
			return;
		}

		numTicksElapsed++;
		if (allElementsInRange ()) {
			Debug.Log ("*************************");
			numTicksInRange++;
		} else {
			numTicksInRange = 0;
		}

		if (this.cloud != null) {
			this.cloud.GetComponent<CloudBehavior> ().growResult (((float)numTicksInRange / (float)numTicksToWin) / 2f);
		}

		if (numTicksInRange == numTicksToWin) {
			win ();
			return;
		}
	}

	private void win() {
		Debug.Log ("YOU WIN!" + "Elapsed: " + numTicksElapsed);
		if (winSound != null) {
			winSound.Play ();
		}
		if (rain != null) {
			//rain.SetActive (true);
			foreach (Transform t in rain.transform) {
				t.GetComponent<MeshRenderer> ().enabled = true;
			}
		}

		if (dryGround != null) {
			dryGround.SetActive (false);
		}

		if (wetGround != null) {
			wetGround.SetActive (true);
		}

		GameObject.Find ("Cloud").GetComponent<CloudBehavior> ().winResult ();
		//winBox.SetActive (true);
		//winBox.GetComponent<Renderer>().enabled = true;

		endGame ();
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

		//theme.SendMessage ("StopMusic");
		if (loseBox != null) {
			loseBox.SetActive (true);
		}
		GameObject.Find ("Cloud").GetComponent<CloudBehavior> ().loseResult ();
		endGame ();
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

	private void updateElementUI(Element element)
	{
		//Fade-In element Graphic
		GameObject.Find(element.Type.ToString()).GetComponent<Pulse>().fadeElement(element);

		//Scale Element Circle
		Transform transform = GameObject.Find(element.Type.ToString() + "Circle").GetComponent<Transform>();
		Vector3 scale = scaleElement(element, transform.localScale);
		transform.localScale = scale;

		//Scale Element LightSource (for the glow effect)
		var test = element.elementGoal.getIntensityCoefficientBasedOffGoalUI();
		GameObject.Find (element.Type.ToString()).GetComponent<Light> ().range = element.elementGoal.getIntensityCoefficientBasedOffGoalUI() * (element.count / element.minCount);

	}

	private Vector3 scaleElement(Element element, Vector3 scale)
	{
		Vector3 newScale = scale;
		float minCount = element.minCount;
		var test = element.elementGoal.getScaleCoefficientBasedOffGoalUI ();
		float changeFactor = element.elementGoal.getScaleCoefficientBasedOffGoalUI() * ( element.count / minCount);
		newScale.Set(changeFactor, scale.y, changeFactor);
		return newScale;       
	}

	private void endGame(){
		//for when we were reseting the game:
		//numTicksInRange = 0;
		//numTicksElapsed = 0;

		//TODO: stop dancing
		//TODO: stop music

		foreach (var element in elements) {
			element.Value.count = 0;
		}

		if (this.cloud != null) {
			this.cloud.SetActive (false);
		}
	}

}