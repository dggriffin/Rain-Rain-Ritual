using UnityEngine;
using System.Collections;

public class CloudBehavior : MonoBehaviour {

	public void growResult (float r)
	{
		r = (r < 0.1f) ? 0.1f : r;
		Vector3 scale = transform.localScale;

		scale.x = r;
		scale.y = r;
		scale.z = r;
		transform.localScale = scale;
//		transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime);
	}

	void Update(){
		transform.Rotate (Random.Range(0,8),2,50 * Time.deltaTime);
	}

	public void winResult (){
		Vector3 scale = transform.localScale;
		scale.x = 10f/transform.localScale.x;
		scale.y = 10f/transform.localScale.y;
		scale.z = 10f/transform.localScale.z;
		transform.localScale = Vector3.Lerp (transform.localScale, scale, Time.deltaTime);
	}
	public void loseResult (){
		Vector3 scale = transform.localScale;
		scale.x = 100f/transform.localScale.x;
		scale.y = 100f/transform.localScale.y;
		scale.z = 100f/transform.localScale.z;
		transform.localScale = Vector3.Lerp (transform.localScale, scale, Time.deltaTime);
	}
}
