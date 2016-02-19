using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {

    public float alpha = 0f;

    private bool shake;
        

	// Use this for initialization
	void Start () {
        Material mat = gameObject.GetComponent<Renderer>().material;
        Color oldColor = mat.color;
        float alpha = 0.0f;
       
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        mat.SetColor("_Color", newColor);
    }
	
	// Update is called once per frame
	void Update () {
       Material mat = gameObject.GetComponent<Renderer>().material;   

        if (alpha >= 0 && alpha <= 1)
        {
            Color oldColor = mat.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
                        
            //Color.Lerp(oldColor, newColor, Time.fixedDeltaTime);
            mat.SetColor("_Color", Color.Lerp(oldColor, newColor, Time.fixedDeltaTime));
        }        
    }

    public void fadeElement(Element element)
    {
		float elementCount = element.count;
        float elementMinCount = element.minCount;   
        
        if(element.minCount < 1)
        {
            elementMinCount = 1;
        }  

        alpha = elementCount / elementMinCount;
        if (alpha > .5f) {
            string test = element.ToString();
         }           
    }		
}
