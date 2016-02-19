using UnityEngine;
using System.Collections;

public class RainFall : MonoBehaviour {

	public int materialIndex = 0;
	private Vector2 uvAnimationRate = new Vector2( 0.0f, 0.5f );
	public string textureName = "_MainTex";
	
	Vector2 uvOffset = Vector2.zero;
	
	void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
		}
	}
}
//http://wiki.unity3d.com/index.php?title=Scrolling_UVs
