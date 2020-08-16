using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private CameraScript cameraScript;
    // Start is called before the first frame update
    void Awake()
    {
        cameraScript = gameObject.GetComponentInParent<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void onFastForward()
    {
        cameraScript.animationSpeed += 0.25f;
    }

    public void onSlowMo()
    {
        cameraScript.animationSpeed = Mathf.Max(0.1f, cameraScript.animationSpeed - 0.25f);
    }
}
