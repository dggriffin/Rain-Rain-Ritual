using UnityEngine;
using System.Collections;

public class CloudBehavior : MonoBehaviour {
	void Start(){
		print ("////");

	}

	public void growResult (float r)
	{
		r=r*10;
		print (r);
		
		if (r >= 1) {
			print ("%%%%%%%");
		}

		print (r);
		r = (r > 0.5f) ? 0.5f : r;
		Vector3 scale = transform.localScale;
		scale.x = r;
		scale.y = r;
		scale.z = r;
		transform.localScale = Vector3.Lerp(transform.localScale, scale, 4f*Time.deltaTime);
	}

	void Update(){
		transform.Rotate (Random.Range(0,8),2,50 * Time.deltaTime);
	}

//	public void winResult (){
//		Vector3 scale = transform.localScale;
//		scale.x = 100;
//		scale.y = 100;
//		scale.z = 100;
//		transform.localScale = Vector3.Lerp (transform.localScale, scale, Time.deltaTime);
//	}
//
//	public void resetResult (){
//		Vector3 scale = transform.localScale;
//		scale.x = 0;
//		scale.y = 0;
//		scale.z = 0;
//		transform.localScale = Vector3.Lerp (transform.localScale, scale, Time.deltaTime);
//	}
}
