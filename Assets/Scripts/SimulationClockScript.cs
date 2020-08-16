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
    private CameraScript cameraScript;
    // Start is called before the first frame update
    void Awake()
    {
        simulationClock = canvasSimulationClock.GetComponentInChildren<TextMeshProUGUI>();
        simulationClock.text = "00:00:00";
        cameraScript = gameObject.GetComponent<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime = elapsedTime.AddSeconds(Time.deltaTime* cameraScript.animationSpeed);
        simulationClock.text = elapsedTime.ToString("HH:mm:ss");
    }
}
