using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void onFastForward()
    {
        Time.timeScale += 0.5f;
    }

    public void onSlowMo()
    {
        Time.timeScale = Mathf.Max(0.1f, Time.timeScale - 0.5f);
    }
}
