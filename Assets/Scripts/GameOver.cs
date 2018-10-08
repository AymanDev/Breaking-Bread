using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        text.text = "Score: " + Convert.ToInt32(GameObject.Find("ScoreText").GetComponent<Score>().score);
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}