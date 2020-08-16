using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    public GameObject parent;
    public int destFloor;
    public int srcFloor;
    private Color color;
    private bool fadeIn;
    private bool fadeOut;
    public float currAlpha;
    private float finalAlpha;
    private float fadeSpeed = 2f;
    // Start is called before the first frame update
    void Awake()
    {
        
    }
    void Start()
    {
        //color = Random.ColorHSV();
        color = new Color(1F, 165F/255F, 0F);
        Color newColor = new Color(color.r, color.g, color.b, 0);
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", newColor);
        fadeIn = true;
        currAlpha = 0;
        finalAlpha = color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            currAlpha += Time.deltaTime * fadeSpeed;
            Color newColor = new Color(color.r, color.g, color.b, Mathf.Min(currAlpha, 1f));
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", newColor);
            if (Mathf.Approximately(currAlpha, finalAlpha) || finalAlpha < currAlpha || currAlpha >= 1f) fadeIn = false;
        }
        if (fadeOut)
        {
            currAlpha += Time.deltaTime * fadeSpeed;
            Color newColor = new Color(color.r, color.g, color.b, Mathf.Max(0, finalAlpha - currAlpha));
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", newColor);
            if (Mathf.Approximately(finalAlpha - currAlpha, 0) || finalAlpha - currAlpha <= 0) 
                Destroy(gameObject);
        }
    }

    public bool animateDestroy()
    {
        fadeOut = true;
        currAlpha = 0;
        return true;
    }
}
