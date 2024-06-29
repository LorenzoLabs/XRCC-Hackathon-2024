using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public bool isWon;

    private bool isTimerRunning;
    private float timer;
    private float timerThreshold = 3f; 

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided trigger enter with: " + other.gameObject.tag);
        if (other.CompareTag("grabbable"))
        {
            isTimerRunning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("grabbable"))
        {
            isTimerRunning = false;
            timer = 0f; // Reset timer when the grabbable object exits
        }
    }

    private void Start()
    {
        isWon = false;
        isTimerRunning = false;
        timer = 0f;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;

            if (timer >= timerThreshold)
            {
                isWon = true;
                // Optionally, you might want to call a method like setbool(true) here
            }
        }
        else
        {
            timer = 0f; // Reset timer if no grabbable object is inside
            isWon = false;
        }
    }
}
