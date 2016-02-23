using UnityEngine;
using System.Collections;

public class CloudBehavior : MonoBehaviour {

	private bool growStarted = false;
	private int count = 0;
	private float r = 0;

	public void growResult (float r)
	{
		growStarted = true;
		r = (r < 0.1f) ? 0.1f : r;
		this.r = r;
	}

	public void reset ()
	{
		growStarted = false;
		count = 0;
		r = 0;
	}

	void Update(){
		transform.Rotate (Random.Range(0,8),2,50 * Time.deltaTime);
		if (growStarted) {
			if (count < 10) {
				count++;
				Vector3 scale = transform.localScale;
				scale.x = Mathf.Lerp (scale.x, scale.x + (count*(r-scale.x)/10)/(Time.deltaTime * 30), Time.time);
				scale.y = Mathf.Lerp (scale.y, scale.y + (count*(r-scale.y)/10)/(Time.deltaTime * 30), Time.time);
				scale.z = Mathf.Lerp (scale.z, scale.y + (count*(r-scale.z)/10)/(Time.deltaTime * 30), Time.time);
				transform.localScale = scale;
			} else {
				count = 0;
				growStarted = false;
			}
		}
	}

	public void winResult (){
		r = 0;
		Vector3 scale = transform.localScale;
		scale.x = 10f/transform.localScale.x;
		scale.y = 10f/transform.localScale.y;
		scale.z = 10f/transform.localScale.z;
		transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime);
	}
	public void loseResult (){
		r = 0;
		Vector3 scale = transform.localScale;
		scale.x = 100f/transform.localScale.x;
		scale.y = 100f/transform.localScale.y;
		scale.z = 100f/transform.localScale.z;
		transform.localScale = Vector3.Lerp (transform.localScale, scale, Time.deltaTime);
	}

//	void Update() {
//		Vector3 scale = transform.localScale;
//		scale.x = 10f/transform.localScale.x;
//		scale.y = 10f/transform.localScale.y;
//		scale.z = 10f/transform.localScale.z;
//		transform.localScale = Vector3.Lerp (transform.localScale, scale, Time.deltaTime);
//
//		transform.position = new Vector3(Mathf.Lerp(minimum, maximum, Time.time), 0, 0);
//	}
}
