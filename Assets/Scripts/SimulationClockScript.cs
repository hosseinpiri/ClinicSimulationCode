using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class SimulationClockScript : MonoBehaviour
{
    private TextMeshProUGUI simulationClock;
    public GameObject canvasSimulationClock;
    private DateTime elapsedTime;
    public CameraScript cameraScript;
    // Start is called before the first frame update
    void Awake()
    {
        cameraScript = gameObject.GetComponent<CameraScript>();
        simulationClock = canvasSimulationClock.GetComponentInChildren<TextMeshProUGUI>();
        simulationClock.text = "00:00:00";
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(cameraScript.elapsedTime);
        elapsedTime = elapsedTime.Date + time;
        simulationClock.text = elapsedTime.ToString("HH:mm:ss");
    }
}
