using UnityEngine;
using System.Collections;

public class CloudBehavior : MonoBehaviour {
	void Start(){
		print ("////");

	}

	public void growResult (float r)
	{
		var ratio = r;
		print (r);
		
		if (r >= 1) {
			print ("%%%%%%%");

		}
			
			Vector3 scale = transform.localScale;
			scale.x = ratio;
			scale.y = ratio;
			scale.z = ratio;
			transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime);

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
