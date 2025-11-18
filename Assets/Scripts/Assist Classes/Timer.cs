using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region Fields
	
    // timer duration
    float totalSeconds = 0;
	
    // timer execution
    float elapsedSeconds = 0;
    bool running = false;
	
    // support for Finished property
    bool started = false;
	
    #endregion
	
    #region Properties
	
    public float Duration
    {
        set
        {
            if (!running)
            {
                totalSeconds = value;
            }
        }
        get {return totalSeconds;}
    }

    public bool Finished
    {
        get { return started && !running; } 
    }
	
    public bool Running
    {
        get { return running; }
    }

    #endregion

    #region Methods

    void Update()
    {	
        // update timer and check for finished
        if (running)
        {
            elapsedSeconds += Time.deltaTime;
            if (elapsedSeconds >= totalSeconds)
            {
                running = false;
            }
        }
    }
	
    public void Run()
    {	
        // only run with valid duration
        if (totalSeconds > 0)
        {
            started = true;
            running = true;
            elapsedSeconds = 0;
        }
    }
	
    #endregion
}