using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EleDirection
{
    UP, DOWN, invalid
}

public enum EventName
{
    hall_queue, elevator_load, doctor_visited, doctor_queue, invalid

}
public class Event
{
    public float time { get; set; }
    public EventName eventName { get; set; }
    public int floorNum { get; set; }
    public int? eleNum { get; set; }
    public EleDirection? eleDir { get; set; }
    public int? clinicNum { get; set; }
    public int newVal { get; set; }
    public int? toDrop { get; set; }

}
