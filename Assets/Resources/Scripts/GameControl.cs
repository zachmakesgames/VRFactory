using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    private const float MIN_TIME_SCALE = 1.0f;
    private const float MAX_TIME_SCALE = 15.0f;
    public FloorBuildManager fbm;
    public Animator flyoverCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            Time.timeScale -= 1.0f;
            if (Time.timeScale < MIN_TIME_SCALE)
            {
                Time.timeScale = MIN_TIME_SCALE;
            }
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            Time.timeScale += 1.0f;
            if (Time.timeScale > MAX_TIME_SCALE)
            {
                Time.timeScale = MAX_TIME_SCALE;
            }
        }

        if (Input.GetKeyDown(KeyCode.Insert))
        {
            if (fbm)
            {
                fbm.AddFromWorkerPool();
            }
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            if (flyoverCamera.GetFloat("Speed") > 0.0f)
            {
                flyoverCamera.SetFloat("Speed", 0.0f);
            }
            else
            {
                flyoverCamera.SetFloat("Speed", 1.0f);
            }
        }
    }
}
