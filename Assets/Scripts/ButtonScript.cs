using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ButtonScript : MonoBehaviour
{
    private float prevTimeScale = 1f;
    public Button playPauseButton;
    public Sprite playImage;
    public Sprite pauseImage;
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
        prevTimeScale = Time.timeScale;
    }

    public void onSlowMo()
    {
        Time.timeScale = Mathf.Max(0.1f, Time.timeScale - 0.5f);
        prevTimeScale = Time.timeScale;
    }

    public void onPlayPause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = prevTimeScale;
            playPauseButton.image.sprite = pauseImage;
        }
        else
        {
            Time.timeScale = 0;
            playPauseButton.image.sprite = playImage;
        }
        
    }
}
