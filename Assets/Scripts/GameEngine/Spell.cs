using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spell {
	private string name;
	private IDictionary<ElementType, Element> elements =  new Dictionary<ElementType, Element>();

	private SpellState state = SpellState.NotStarted;

	private int numTicksToWin;
	private int maxTicksForSpell;
	private int numTicksInRange = 0;
	private int numTicksElapsed = 0;

	private AudioSource winSound = null;
	private AudioSource loseSound = null;

	private GameObject rain = null;
	private MeshRenderer dryGroundMeshRenderer = null;
	private MeshRenderer wetGroundMeshRenderer = null;
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

		InitializeGround ();

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

		state = SpellState.InProgress;
	}

	public SpellState State { 
		get { 
			return state;
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
			foreach (Transform t in rain.transform) {
				t.GetComponent<MeshRenderer> ().enabled = true;
			}
		}

		ShowWetGround (true);

		GameObject.Find ("Cloud").GetComponent<CloudBehavior> ().winResult ();
		//winBox.SetActive (true);
		//winBox.GetComponent<Renderer>().enabled = true;

		state = SpellState.Win;
		endSpell ();
	}

	private void lose() {
		Debug.Log ("YOU LOSE! (too many ticks) Elapsed: " + numTicksElapsed);
		if (loseSound != null) {
			loseSound.Play ();
		}

		if (rain != null) {
			rain.SetActive (false);
		}

		ShowWetGround (false);

		//theme.SendMessage ("StopMusic");
		if (loseBox != null) {
			loseBox.SetActive (true);
		}
		GameObject.Find ("Cloud").GetComponent<CloudBehavior> ().loseResult ();

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

		if (this.cloud != null) {
			this.cloud.SetActive (false);
		}

		//cwkTODO send message that this spell is over
	}

}