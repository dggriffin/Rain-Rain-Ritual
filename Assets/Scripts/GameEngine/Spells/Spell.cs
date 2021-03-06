﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spell {
	private string name;
	private IDictionary<ElementType, Element> elements = null;

	private SpellState state = SpellState.NotStarted;

	private int numTicksToWin;
	private int maxTicksForSpell;
	protected int numTicksInRange = 0;
	protected int numTicksElapsed = 0;

	protected AudioDictionary audioDict;
	private AudioSource winSound = null;
	private AudioSource loseSound = null;

	private MeshRenderer dryGroundMeshRenderer = null;
	private MeshRenderer wetGroundMeshRenderer = null;

	private GameObject theme = null;

    private Text earthText = null;
    private Text fireText = null;
	private Text waterText = null;
	private Text windText = null;
	private Text tickCountText = null;

	private GameObject winBox = null;
	private GameObject loseBox = null;

	public delegate void StateChangeEvent(SpellState state, Spell spell);
	public event StateChangeEvent OnStateChange;

	public Spell(string name = null) {
		this.name = name;

		this.audioDict = GameObject.Find ("AudioDictionary").GetComponent<AudioDictionary> ();

		WinAnimationInitialize ();

		this.theme = GameObject.Find ("ThemeSource");

//		this.winBox = GameObject.Find ("Canvas").GetCom("RainWinBox");

//		foreach (Transform t in GameObject.Find("Canvas").transform) {
//			if (t.name == "RainWinBox") {
//				t.GetComponent<CanvasRenderer>().
//			}
//		}
		this.loseBox = GameObject.Find ("RainLoseBox");
	}

	public void StartSpell () {
		//Debug.Log (Name + " START");

		InitializeGround ();

		InitializeText ();

		CenterObjectInitialize ();

		ShowWinAnimation (false);

		// listening to events really "starts" the spell
		ListenToEvents ();

		state = SpellState.InProgress;
		NotifyStateChange ();
	}

	//cwkTODO not sure if this is necessary
	public void StopSpell () {
		//Debug.Log (Name + " STOP");
	}

	public SpellState State { 
		get { 
			return state;
		}
	}

	public string Name {
		get {
			if (name == null) {
				return this.GetType ().ToString ();
			}
			return name;
		}
	}

	protected virtual int NumTicksToWin {
		get {
			return numTicksToWin;
		}
	}

	protected virtual int MaxTicksForSpell {
		get {
			return maxTicksForSpell;
		}
	}

	protected virtual List<Element> ElementList {
		get { 
			return new List<Element> ();
		}
	}

	protected IDictionary<ElementType, Element> Elements {
		get {
			if (this.elements == null) {
				this.elements = new Dictionary<ElementType, Element>();
				foreach (var element in ElementList) {
					this.elements.Add (element.Type, element);
				}
			}
			return this.elements;
		}
	}

	protected virtual AudioSource WinSound {
		get { return this.winSound; }
	}

	protected virtual AudioSource LoseSound {
		get { return this.loseSound; }
	}

	protected virtual void CenterObjectInitialize () {
	}

	protected virtual void CenterObjectUpdate () {
	}

	protected virtual void CenterObjectWin () {
	}

	protected virtual void CenterObjectLose () {
	}

	protected virtual void WinAnimationInitialize () {
	}

	protected virtual void ShowWinAnimation(bool show) {
	}

	private void NotifyStateChange() {
		if (OnStateChange != null) {
			OnStateChange (this.state, this);
		}
	}

	private void InitializeGround() {
		var dryGround = GameObject.Find ("DryGround");
		if (dryGround != null) {
			this.dryGroundMeshRenderer = dryGround.GetComponent<MeshRenderer> ();
			if (this.dryGroundMeshRenderer != null) {
				this.dryGroundMeshRenderer.enabled = true;
			}
		}

		var wetGround = GameObject.Find ("WetGround");
		if (wetGround != null) {
			this.wetGroundMeshRenderer = wetGround.GetComponent<MeshRenderer> ();
			if (this.wetGroundMeshRenderer != null) {
				this.wetGroundMeshRenderer.enabled = false;
			}
		}
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

		this.tickCountText = GameObject.Find ("TickCount").GetComponent<Text> ();
		this.tickCountText.text = "";
	}

	private void ListenToEvents() {
		var inputHandler = GameObject.Find ("InputHandler");
		inputHandler.GetComponent<InputHandler> ().ElementEvent += Increment;

		var metronome = GameObject.Find ("Metronome");
		metronome.GetComponent<Metronome>().OnTick += RangeCheck;
		metronome.GetComponent<Metronome>().OnTick += Decay;
	}

	private void StopListeningToEvents() {
		var inputHandler = GameObject.Find ("InputHandler");
		inputHandler.GetComponent<InputHandler> ().ElementEvent -= Increment;

		var metronome = GameObject.Find ("Metronome");
		metronome.GetComponent<Metronome>().OnTick -= RangeCheck;
		metronome.GetComponent<Metronome>().OnTick -= Decay;
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
		updateElementUI(element);
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

			//Debug.Log (elementCircleName + " is OFFBEAT!");
		}
	}

	private void HideOffbeatText() {
		this.earthText.text = "";
		this.fireText.text = "";
		this.waterText.text = "";
		this.windText.text = "";
	}


	private void Decay(){
		foreach (var element in Elements) {
			element.Value.Decay ();
			updateElementUI(element.Value);
		}
		//Debug.Log ("Decaying");
		//PrintElements ();
	}

	private void RangeCheck() {
		if (numTicksElapsed > MaxTicksForSpell) {
			lose ();
			return;
		}

		numTicksElapsed++;
		if (allElementsInRange ()) {
			var ticksLeft = NumTicksToWin - numTicksInRange;
			//Debug.Log ("Num Ticks in Range: " + numTicksInRange + "/" + NumTicksToWin);
			//Debug.Log ("Ticks left: " + ticksLeft);
			this.tickCountText.text = ticksLeft.ToString();

			numTicksInRange++;
		} else {
			numTicksInRange = 0;
			this.tickCountText.text = "";
		}

		CenterObjectUpdate ();

		if (numTicksInRange == NumTicksToWin) {
			win ();
			return;
		}
	}

	private void win() {
		Debug.Log (Name + ": YOU WIN!" + "Elapsed: " + numTicksElapsed);
		if (WinSound != null) {
			WinSound.Play ();
		}

		ShowWinAnimation (true);

		ShowWetGround (true);

		CenterObjectWin ();

		//winBox.SetActive (true);
		//winBox.GetComponent<Renderer>().enabled = true;

		state = SpellState.Win;
		endSpell ();
	}

	private void lose() {
		Debug.Log (Name + ": YOU LOSE! (too many ticks) Elapsed: " + numTicksElapsed);
		if (LoseSound != null) {
			LoseSound.Play ();
		}

		ShowWinAnimation (false);

		ShowWetGround (false);

		//theme.SendMessage ("StopMusic");
		if (loseBox != null) {
			loseBox.SetActive (true);
		}

		CenterObjectLose ();

		state = SpellState.Lose;
		endSpell ();
	}

	private void ShowWetGround (bool showWetGround){
		if (this.dryGroundMeshRenderer != null) {
			this.dryGroundMeshRenderer.enabled = !showWetGround;
		}

		if (this.wetGroundMeshRenderer != null) {
			this.wetGroundMeshRenderer.enabled = showWetGround;
		}
	}

	private bool allElementsInRange() {
		bool allInRange = true;
		foreach (var element in Elements) {
			allInRange = allInRange && element.Value.IsInRange();
		}
		return allInRange;
	}

	private Element getElement(ElementType elementType){
		return Elements.ContainsKey(elementType) ? Elements [elementType] : null;
	}

	public void PrintElements () {
		Debug.Log(string.Format("Spell: {0}", Name));
		foreach (var element in Elements) {
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
		GameObject.Find (element.Type.ToString()).GetComponent<Light> ().range = element.elementGoal.getIntensityCoefficientBasedOffGoalUI() * (element.count / element.minCount);

	}

	private Vector3 scaleElement(Element element, Vector3 scale)
	{
		Vector3 newScale = scale;
		float minCount = element.minCount;
		float changeFactor = element.elementGoal.getScaleCoefficientBasedOffGoalUI() * ( element.count / minCount);
		newScale.Set(changeFactor, scale.y, changeFactor);
		return newScale;       
	}

	private void endSpell(){
		//for when we were reseting the game:
		//numTicksInRange = 0;
		//numTicksElapsed = 0;

		//TODO: stop dancing
		//TODO: stop music

		foreach (var element in Elements) {
			element.Value.count = 0;
		}

		this.tickCountText.text = "";

		NotifyStateChange ();

		StopListeningToEvents ();
	}

}