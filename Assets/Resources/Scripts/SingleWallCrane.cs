using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleWallCrane : MonoBehaviour
{
    public GameObject sliderail;
    public GameObject winch;
    public GameObject hook;

    public float moveSpeed = 1.0f;

    private float sliderailDefaultX;
    private float winchDefaultZ;
    private float hookDefaultY;

    void Start()
    {
        sliderailDefaultX = sliderail.transform.position.x;
        winchDefaultZ = winch.transform.position.z;
        hookDefaultY = hook.transform.position.y;
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }

    // Move the Crane Sliderail along the x-axis (global).
    public IEnumerator MoveSlideRail(float goal_x)
    {
        Vector3 goal = new Vector3(goal_x, sliderail.transform.position.y, sliderail.transform.position.z);
        while (Vector3.Distance(goal, sliderail.transform.position) > 0.0001f)
        {
            sliderail.transform.position = Vector3.MoveTowards(sliderail.transform.position, goal, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Move the Crane Winches along the z-axis (global).
    public IEnumerator MoveWinches(float goal_z)
    {
        Vector3 goal = new Vector3(winch.transform.position.x, winch.transform.position.y, goal_z);
        while (Vector3.Distance(goal, winch.transform.position) > 0.0001f)
        {
            winch.transform.position = Vector3.MoveTowards(winch.transform.position, goal, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Move the Crane Forks along the y-axis (global).
    public IEnumerator MoveLiftingBeam(float goal_y)
    {
        Vector3 goal = new Vector3(hook.transform.position.x, goal_y, hook.transform.position.z);
        while (Vector3.Distance(goal, hook.transform.position) > 0.0001f)
        {
            hook.transform.position = Vector3.MoveTowards(hook.transform.position, goal, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
