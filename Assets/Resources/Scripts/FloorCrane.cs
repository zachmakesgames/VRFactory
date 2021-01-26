using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCrane : MonoBehaviour
{
    // The moveable Crane Components.
    public GameObject sliderail;
    public GameObject winches;
    public GameObject liftingBeam;

    // Movement speeds.
    public float moveSpeed;

    // SlideRail Positions
    private float sliderailDefaultX;
    public float sliderailDeckPickupX; // -23.75
    public float sliderailDeckDropoffX; // 9.79
    public float sliderailWallsetDropoffX;

    // Winch Positions.
    private float winchDefaultZ;
    public float winchDeckPickupZ; // -86.88
    public float winchDeckDropoffRightZ; // -87.13
    public float winchDeckDropoffLeftZ; // -80.66
    public float winchWallsetDropoffZ;

    // Lifting Beam Positions.
    private float liftingBeamDefaultY;
    public float liftingBeamDeckPickupY; // 2.58
    public float liftingBeamDeckDropoffY; // 4.45
    public float liftingBeamWallsetDropoffY;

    // Is the Crane currently not in operation.
    public bool idle = true;
    private GameObject currentCarryObject;

    // Start is called before the first frame update
    void Start()
    {
        sliderailDefaultX = sliderail.transform.position.x;
        winchDefaultZ = winches.transform.position.z;
        liftingBeamDefaultY = liftingBeam.transform.position.y;
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }

    // Move the Crane Sliderail along the x-axis (global).
    IEnumerator MoveSlideRail(float goal_x)
    {
        Vector3 goal = new Vector3(goal_x, sliderail.transform.position.y, sliderail.transform.position.z);
        while (Vector3.Distance(goal, sliderail.transform.position) > 0.0001f)
        {
            sliderail.transform.position = Vector3.MoveTowards(sliderail.transform.position, goal, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Move the Crane Winches along the z-axis (global).
    IEnumerator MoveWinches(float goal_z)
    {
        Vector3 goal = new Vector3(winches.transform.position.x, winches.transform.position.y, goal_z);
        while (Vector3.Distance(goal, winches.transform.position) > 0.0001f)
        {
            winches.transform.position = Vector3.MoveTowards(winches.transform.position, goal, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Move the Crane Forks along the y-axis (global).
    IEnumerator MoveLiftingBeam(float goal_y)
    {
        Vector3 goal = new Vector3(liftingBeam.transform.position.x, goal_y, liftingBeam.transform.position.z);
        while (Vector3.Distance(goal, liftingBeam.transform.position) > 0.0001f)
        {
            liftingBeam.transform.position = Vector3.MoveTowards(liftingBeam.transform.position, goal, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Load the Crane with a Built Floor.
    public IEnumerator PickupFloor(GameObject f)
    {
        idle = false;
        Vector3 sliderail_old = sliderail.transform.position;
        Vector3 winches_old = winches.transform.position;
        Vector3 liftingBeam_old = liftingBeam.transform.position;

        yield return StartCoroutine(MoveSlideRail(sliderailDeckPickupX));
        yield return StartCoroutine(MoveWinches(winchDeckPickupZ));
        yield return StartCoroutine(MoveLiftingBeam(liftingBeamDeckPickupY));
        currentCarryObject = f;
        f.transform.parent = liftingBeam.transform;
        yield return StartCoroutine(MoveLiftingBeam(liftingBeam_old.y));
        yield return StartCoroutine(MoveWinches(winches_old.z));
        yield return StartCoroutine(MoveSlideRail(sliderail_old.x));
        idle = true;
    }

    public IEnumerator SetFloorOnRack()
    {
        idle = false;
        Vector3 sliderail_old = sliderail.transform.position;
        Vector3 winches_old = winches.transform.position;
        Vector3 liftingBeam_old = liftingBeam.transform.position;

        yield return StartCoroutine(MoveSlideRail(sliderailDeckDropoffX));
        yield return StartCoroutine(MoveWinches(winchDeckDropoffRightZ));
        yield return StartCoroutine(MoveLiftingBeam(liftingBeamDeckDropoffY));
        currentCarryObject.transform.parent = null;
        yield return StartCoroutine(MoveLiftingBeam(liftingBeam_old.y));
        yield return StartCoroutine(MoveWinches(winches_old.z));
        yield return StartCoroutine(MoveSlideRail(sliderail_old.x));
        idle = true;
    }

    public IEnumerator SetFloorAtWallSet(GameObject f, GameObject fj)
    {
        idle = false;
        Vector3 sliderail_old = sliderail.transform.position;
        Vector3 winches_old = winches.transform.position;
        Vector3 liftingBeam_old = liftingBeam.transform.position;

        yield return StartCoroutine(MoveSlideRail(sliderailDeckDropoffX));
        yield return StartCoroutine(MoveWinches(winchDeckDropoffRightZ));
        yield return StartCoroutine(MoveLiftingBeam(liftingBeamDeckDropoffY));
        f.transform.parent = liftingBeam.transform;
        yield return StartCoroutine(MoveLiftingBeam(liftingBeam_old.y));
        yield return StartCoroutine(MoveWinches(winchWallsetDropoffZ));
        yield return StartCoroutine(MoveLiftingBeam(liftingBeamWallsetDropoffY));
        f.transform.parent = fj.transform;
        yield return StartCoroutine(MoveLiftingBeam(liftingBeam_old.y));
        yield return StartCoroutine(MoveWinches(winches_old.z));
        yield return StartCoroutine(MoveSlideRail(sliderail_old.x));
        idle = true;
    }
}
