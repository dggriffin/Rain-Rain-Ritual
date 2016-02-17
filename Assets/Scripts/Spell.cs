using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

	private Text earthText = null;
	private Text fireText = null;
	private Text waterText = null;
	private Text windText = null;

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

		InitializeText ();

		ListenToEvents ();
	}

	private void InitializeText() {
		this.earthText = GameObject.Find ("EarthText").GetComponent<Text>();
		this.earthText.text = "";

		this.fireText = GameObject.Find ("FireText").GetComponent<Text> ();
		this.fireText.text = "";

		this.waterText = GameObject.Find ("WaterText").GetComponent<Text> ();
		this.waterText.text = "";

		this.windText = GameObject.Find ("WindText").GetComponent<Text> ();
		this.windText.text = "";
	}

	private void ListenToEvents() {
		inputHandler = GameObject.Find ("InputHandler");
		inputHandler.GetComponent<InputHandler> ().ElementEvent += Increment;

		metronome = GameObject.Find ("Metronome");
		metronome.GetComponent<Metronome>().OnTick += RangeCheck;
		metronome.GetComponent<Metronome>().OnTick += Decay;
	}

	private void Increment(ElementType elementType, bool isOffbeat){
		if (isOffbeat) {
			ShowOffbeatText (elementType);
			return;
		}

		HideOffbeatText ();

		var element = getElement(elementType);
		if (element == null) {
			return;
		}
		element.Increment();
		incrementElement(element);
		//		PrintElements ();
	}

	private void ShowOffbeatText(ElementType elementType) {
		//Debug.Log ("OFFBEAT!");
		var camera = (GameObject.Find ("Main Camera")).GetComponent<Camera>();

		string elementCircleName = null;
		GameObject elementCircle = null;
		Vector3 elementCircleScreenPosition;
		Vector3 textPositionOffset = new Vector3 (0, 0, 0); // moving the text so it centers on the element
		Text elementText = null;

		switch (elementType) {

		case ElementType.Earth:
			elementCircleName = "EarthCircle";
			elementText = this.earthText;
			textPositionOffset = new Vector3 (30, -20, 0);
			break;
		case ElementType.Fire:
			elementCircleName = "FireCircle";
			elementText = this.fireText;
			textPositionOffset = new Vector3 (10, -10, 0);
			break;
		case ElementType.Water:
			elementCircleName = "WaterCircle";
			elementText = this.waterText;
			textPositionOffset = new Vector3 (-20, 0, 0);
			break;
		case ElementType.Wind:
			elementCircleName = "WindCircle";
			elementText = this.windText;
			textPositionOffset = new Vector3 (-10, 0, 0);
			break;
		}

		if (elementCircleName != null) {
			elementCircle = GameObject.Find (elementCircleName);

			// convert element's position to a screen position
			// since text position is relative to the screen (since it is part of UI/canvas)
			elementCircleScreenPosition = camera.WorldToScreenPoint (elementCircle.transform.position);

			elementText.transform.position = elementCircleScreenPosition;
			elementText.transform.Translate (textPositionOffset);
			elementText.text = "OFFBEAT!";
		}
	}

	private void HideOffbeatText() {
		this.earthText.text = "";
		this.fireText.text = "";
		this.waterText.text = "";
		this.windText.text = "";
	}

	private void Decay(){
		foreach (var element in elements) {
			decrementElement(element.Value);
			element.Value.Decay ();            
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

		if (this.cloud != null) {
			this.cloud.GetComponent<CloudBehavior> ().growResult (((float)numTicksInRange / (float)numTicksToWin) / 2f);
		}

		if (numTicksInRange == numTicksToWin) {
			win ();
			return;
		}

		if (numTicksElapsed > maxTicksForSpell) {
			lose ();
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
		winBox.GetComponent<Renderer>().enabled = true;

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

	private void incrementElement(Element element)
	{
		if (element.Type.Equals(ElementType.Fire))
		{
			GameObject.Find("Fire").GetComponent<Pulse>().fadeIn(element);
			Transform transform = GameObject.Find("FireCircle").GetComponent<Transform>();
			Vector3 scale = scaleElement(element, transform.localScale, false);
			transform.localScale = scale;
		}
		else if (element.Type.Equals(ElementType.Earth))
		{
			GameObject.Find("Earth").GetComponent<Pulse>().fadeIn(element);
			Transform transform = GameObject.Find("EarthCircle").GetComponent<Transform>();
			Vector3 scale = scaleElement(element, transform.localScale, false);
			transform.localScale = scale;
		}
		else if (element.Type.Equals(ElementType.Wind))
		{
			GameObject.Find("Wind").GetComponent<Pulse>().fadeIn(element);
			Transform transform = GameObject.Find("WindCircle").GetComponent<Transform>();
			Vector3 scale = scaleElement(element, transform.localScale, false);
			transform.localScale = scale;
		}
		else if (element.Type.Equals(ElementType.Water))
		{
			GameObject.Find("Water").GetComponent<Pulse>().fadeIn(element);
			Transform transform = GameObject.Find("WaterCircle").GetComponent<Transform>();
			Vector3 scale = scaleElement(element, transform.localScale, false);
			transform.localScale = scale;
		}
	}
	private void decrementElement(Element element)
	{
		if (element.Type.Equals(ElementType.Fire) && element.count != 0)
		{
			GameObject.Find("Fire").GetComponent<Pulse>().fadeOut(element);
			Transform transform = GameObject.Find("FireCircle").GetComponent<Transform>();
			Vector3 scale = scaleElement(element, transform.localScale, true);
			transform.localScale = scale;
		}
		else if (element.Type.Equals(ElementType.Earth) && element.count != 0)
		{
			GameObject.Find("Earth").GetComponent<Pulse>().fadeOut(element);          
			Transform transform = GameObject.Find("EarthCircle").GetComponent<Transform>();
			Vector3 scale = scaleElement(element, transform.localScale, true);
			transform.localScale = scale;
		}
		else if (element.Type.Equals(ElementType.Wind) && element.count != 0)
		{    
			GameObject.Find("Wind").GetComponent<Pulse>().fadeOut(element);
			Transform transform = GameObject.Find("WindCircle").GetComponent<Transform>();
			Vector3 scale = scaleElement(element, transform.localScale, true);
			transform.localScale = scale;
		}
		else if (element.Type.Equals(ElementType.Water) && element.count != 0)
		{
			GameObject.Find("Water").GetComponent<Pulse>().fadeOut(element);
			Transform transform = GameObject.Find("WaterCircle").GetComponent<Transform>();
			Vector3 scale = scaleElement(element, transform.localScale, true);
			transform.localScale = scale;
		}
	}

	private Vector3 scaleElement(Element element, Vector3 scale, bool shrink)
	{
		Vector3 newScale = scale;


		float minCount = element.minCount;
		float changeFactor = (.035f / minCount);

		if (!shrink)
		{
			newScale.Set(scale.x + changeFactor, scale.y, scale.z + changeFactor);
		}
		else
		{
			newScale.Set(scale.x - changeFactor, scale.y, scale.z - changeFactor);
		}

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