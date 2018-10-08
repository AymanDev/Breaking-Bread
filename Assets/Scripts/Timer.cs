using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text text;
    public int seconds;

    private void Start()
    {
        text = GetComponent<Text>();
        new DateTime();
        InvokeRepeating("Tick", 1f, 1f);
    }

    private void Tick()
    {
        seconds++;
        var time = TimeSpan.FromSeconds(seconds);
        var str = time.ToString();
        text.text = "Time: " + str;
    }
}