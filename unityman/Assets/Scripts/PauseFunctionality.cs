﻿using UnityEngine;
using System.Collections;

public class PauseFunctionality : MonoBehaviour {
    public InputHandler inputHandler;
    public GameObject pauseCanvas;

    void Start()
    {
        if (inputHandler == null)
        {
            inputHandler = GameObject.FindObjectOfType<InputHandler>();
        }
    }

	// Update is called once per frame
	void Update () {
	    if (inputHandler.pause)
        {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true); 
        }
	}

    public void Resume()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
