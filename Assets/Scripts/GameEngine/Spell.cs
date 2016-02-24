using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spell {
	private string name;
	private IDictionary<ElementType, Element> elements =  new Dictionary<ElementType, Element>();

	private SpellState state = SpellState.NotStarted;

	protected int numTicksToWin;
	protected int maxTicksForSpell;
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

	private GameObject winBox = null;
	private GameObject loseBox = null;

	public delegate void StateChangeEvent(SpellState state, Spell spell);
	public event StateChangeEvent OnStateChange;

	public Spell(string name, IList<Element> elements, int numTicksToWin, int maxTicksForSpell, AudioDictionary audioDict){
		this.name = name;
		this.elements = new Dictionary<ElementType, Element>();
		foreach (var element in elements) {
			this.elements.Add (element.Type, element);
		}
		this.numTicksToWin = numTicksToWin;
		this.maxTicksForSpell = maxTicksForSpell;

		this.audioDict = audioDict;

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
		Debug.Log (this.name + " START");

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
		Debug.Log (this.name + " STOP");
	}

	public SpellState State { 
		get { 
			return state;
		}
	}

	public string Name {
		get {
			return name;
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
			//Debug.Log ("*************************");
			numTicksInRange++;
		} else {
			numTicksInRange = 0;
		}

		CenterObjectUpdate ();

		if (numTicksInRange == numTicksToWin) {
			win ();
			return;
		}
	}

	private void win() {
		Debug.Log (this.name + ": YOU WIN!" + "Elapsed: " + numTicksElapsed);
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
		Debug.Log (this.name + ": YOU LOSE! (too many ticks) Elapsed: " + numTicksElapsed);
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

		foreach (var element in elements) {
			element.Value.count = 0;
		}

		NotifyStateChange ();

		StopListeningToEvents ();
	}

}