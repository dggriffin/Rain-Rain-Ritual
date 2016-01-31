using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {

    public float emissionIntensity = 0f;
    public float oldEmissionIntensity = 0f;
    public float alpha = 0f;
        

	// Use this for initialization
	void Start () {
        Material mat = gameObject.GetComponent<Renderer>().material;
        Color oldColor = mat.color;
        float alpha = 0.0f;
        Color newColor = new Color(oldColor.r, oldColor.b, oldColor.g, alpha);
        mat.SetColor("_Color", newColor);
    }
	
	// Update is called once per frame
	void Update () {
       Material mat = gameObject.GetComponent<Renderer>().material;
       oldEmissionIntensity = Mathf.MoveTowards(oldEmissionIntensity, emissionIntensity, 1f);
        //mat.SetFloat("_EmissiveIntensity", oldEmissionIntensity);

       // Color oldColor = mat.color;
        //float alpha = 0.0f;
        //Color newColor = new Color(oldColor.r, oldColor.b, oldColor.g, alpha);
       // mat.SetColor("_Color", newColor);
    }

    public void fadeIn(Element element)
    {
        float elementCount = element.count;
        float elementMinCount = element.minCount;

        alpha = elementCount / elementMinCount;

        if (alpha >= 0)
        {
            Material mat = gameObject.GetComponent<Renderer>().material;
            Color oldColor = mat.color;
            Color newColor = new Color(oldColor.r, oldColor.b, oldColor.g, alpha);
            mat.SetColor("_Color", newColor);
        }
    }

    public void fadeOut(Element element)
    {
        float elementCount = element.count;
        float elementMinCount = element.minCount;

        alpha = elementCount / elementMinCount;
        if (alpha <= 1)
        {
            Material mat = gameObject.GetComponent<Renderer>().material;
            Color oldColor = mat.color;
            Color newColor = new Color(oldColor.r, oldColor.b, oldColor.g, alpha);
            mat.SetColor("_Color", newColor);
        }
    }
}
