using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkliftCrane : MonoBehaviour
{
    // The moveable Crane Components.
    public GameObject sliderail;
    public GameObject frame;
    public GameObject spinner;
    public GameObject forks;
    public GameObject loadPoint;

    // Movement speeds.
    public float moveSpeed;
    public float angularSpeed;
    
    // SlideRail Positions
    private float sliderailDefaultX;
    public float sliderailMaterialPickupX; //-53.62
    public float sliderailEndX; // -13.45
    public float sliderailUnloadX;
    
    // Fork Positions.
    private float forksDefaultY;
    public float forksMaterialPickupY; // 1.252
    public float forksUnloadY; // 2.13

    // Frame Positions.
    private float frameDefaultZ;
    public float frameWorkAreaZ; //-86.75
    public float frameMaterialPickupZ; //-79.88

    // Crane Carrying Material.
    public GameObject currMaterials;
    
    // Is the Crane currently not in operation.
    private bool idle = true;

    private WorkerTarget unloadTarget;

    // Start is called before the first frame update
    void Start()
    {
        sliderailDefaultX = sliderail.transform.position.x;
        forksDefaultY = forks.transform.position.y;
        frameDefaultZ = frame.transform.position.z;
        unloadTarget = GetComponentInChildren<WorkerTarget>();
        //spinner_default_z_rot = spinner.transform.rotation.eulerAngles.z;
    }

    // Clean up on Quit.
    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }

    public bool IsIdle()
    {
        return idle;
    }

    // Move the Crane Sliderail along the x-axis (global).
    public IEnumerator MoveSlideRail(float goal_x)
    {
        Vector3 goal = new Vector3(goal_x, sliderail.transform. position.y, sliderail.transform.position.z);
        while (Vector3.Distance(goal, sliderail.transform.position) > 0.0001f)
        {
            sliderail.transform.position = Vector3.MoveTowards(sliderail.transform.position, goal, moveSpeed * Time.deltaTime);
            yield return null;
        }  
    }

    // Move the Crane Frame along the z-axis (global).
    public IEnumerator MoveFrame(float goal_z)
    {
        Vector3 goal = new Vector3(frame.transform.position.x, frame.transform.position.y, goal_z);
        while (Vector3.Distance(goal, frame.transform.position) > 0.0001f)
        {
            frame.transform.position = Vector3.MoveTowards(frame.transform.position, goal, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Move the Crane Forks along the y-axis (global).
    public IEnumerator MoveForks(float goal_y)
    {
        Vector3 goal = new Vector3(forks.transform.position.x, goal_y, forks.transform.position.z);
        while (Vector3.Distance(goal, forks.transform.position) > 0.0001f)
        {
            forks.transform.position = Vector3.MoveTowards(forks.transform.position, goal, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Rotate the Crane Forks about the z-axis (local).
    public IEnumerator RotateForks(float goal_angle)
    {
        Quaternion goal = Quaternion.Euler(0.0f, 0.0f, goal_angle);
        while (Quaternion.Angle(spinner.transform.localRotation, goal) > 0.0001f)
        {
            spinner.transform.localRotation = Quaternion.RotateTowards(spinner.transform.localRotation, goal, angularSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Load the Crane with materials.
    public IEnumerator PickupMaterials()
    {
        idle = false;
        Vector3 sliderail_old = sliderail.transform.position;
        Vector3 forks_old = forks.transform.position;
        Vector3 frame_old = frame.transform.position;

        yield return StartCoroutine(MoveSlideRail(sliderailMaterialPickupX));
        yield return StartCoroutine(MoveForks(forksMaterialPickupY));
        yield return StartCoroutine(RotateForks(-90.0f));
        yield return StartCoroutine(MoveFrame(frameMaterialPickupZ));
        for (int i = 0; i < loadPoint.transform.childCount; i++)
        {
            if (loadPoint.transform.GetChild(i).name.Contains("Sticker"))
            {
                continue;
            }
            else
            {
                currMaterials = loadPoint.transform.GetChild(i).gameObject;
                currMaterials.transform.parent = forks.transform;
            }
        }
        yield return StartCoroutine(MoveFrame(frame_old.z));
        yield return StartCoroutine(RotateForks(0.0f));
        yield return StartCoroutine(MoveForks(forks_old.y));
        yield return StartCoroutine(MoveSlideRail(sliderail_old.x));
        idle = true;
    }

    // Put the Crane into its working position.
    public IEnumerator MoveToWorkPosition()
    {
        idle = false;
        yield return StartCoroutine(MoveFrame(frameWorkAreaZ));
        yield return StartCoroutine(MoveForks(forksUnloadY));
        yield return StartCoroutine(MoveSlideRail(sliderailUnloadX));
        idle = true;
    }
}
