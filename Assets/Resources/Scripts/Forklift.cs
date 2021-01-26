using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// The Controller Class for forklift game objects.
public class Forklift : MonoBehaviour
{
    // The Forklifts start/parked position.
    private Vector3 parkedPosition;
    // The Forklifts start/parked rotation.                      
    private Quaternion parkedRotation;
    // The Forklifts movement speeds.                 
    public float speed = 1.0f;
    public float turnSpeed = 0.5f;

    // The Forklifts Fork and wheels GameObjects.
    private GameObject forks;
    private GameObject wheelLeft;
    private GameObject wheelRight;
    // The Forklifts Forks start/parked height. (Y Axis)
    private Vector3 forksDefaultLevel;
    // The Forklifts Forks current load.
    private GameObject materials;
    // The Forklifts Forks movement speed.
    public float forkSpeed = 0.25f;
    // The Forklifts Forks lowest height.
    public float forksGroundOffset = -0.2f;

    public List<ForkliftPath> floorBuildPaths;
    public List<ForkliftPath> wallBuildPaths;
    // A list of pathMarkers that define a current delivery.
    private List<ForkliftPathMarker> deliveryPathMarkers;
    // Set to true after instatiating the deliveryPathMarkers list to start a delivery.
    

    // The delivery routine running status. True when delivering materials.
    private bool routineRunning = false;

    // Setup variables and components.
    void Start()
    {

        parkedPosition = transform.position;
        parkedRotation = transform.rotation;

        Transform theforklift = transform.GetChild(0);
        forks = theforklift.Find("forks").gameObject;
        wheelLeft = theforklift.Find("wheel.front.l").gameObject;
        wheelRight = theforklift.Find("wheel.front.r").gameObject;
        forksDefaultLevel = new Vector3(forks.transform.position.x, forks.transform.position.y, forks.transform.position.z);
    }

    // Move the forklift linearly with no rotation.
    IEnumerator LinearMove(Vector3 end)
    {
        while (Vector3.Distance(transform.position, end) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
            yield return null;
        }
    }

    // Move the forklifts forks.
    IEnumerator MoveForks(Vector3 pos)
    {
        while (Vector3.Distance(forks.transform.position, pos) > 0.001f)
        {
            forks.transform.position = Vector3.MoveTowards(forks.transform.position, pos, forkSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Move and rotate the forklift along a bezier curve path from start to end.
    IEnumerator Turn(ForkliftPathMarker start, ForkliftPathMarker end)
    {
        float t = 0.0f;

        while (t <= 1.0f)
        {
            float omt = 1.0f - t;
            Vector3 p0 = (omt * omt * omt) * start.transform.position;
            Vector3 p1 = 3.0f * t * (omt * omt) * start.controlPoint.transform.position;
            Vector3 p2 = 3.0f * (t * t) * omt * end.controlPoint.transform.position;
            Vector3 p3 = (t * t * t) * end.transform.position;
            transform.position = p0 + p1 + p2 + p3;
            transform.rotation = Quaternion.Lerp(start.transform.rotation, end.transform.rotation, t);
            t += (Time.deltaTime * turnSpeed) / Time.timeScale;
            yield return null;
        }
    }

    // Make a material delivery using the deliveryPathMarkers list as the route/item data.
    public IEnumerator DeliverItem(string deliveryLocation, string deliveryItem)
    {
        routineRunning = true;
        if (deliveryLocation.Contains("FloorBuild"))
        {
            deliveryPathMarkers = floorBuildPaths.Find(p => p.item == deliveryItem).nodes;
        }
        else if (deliveryLocation.Contains("WallBuild"))
        {
            deliveryPathMarkers = wallBuildPaths.Find(p => p.item == deliveryItem).nodes;
        }
        else
        {
            routineRunning = false;
            yield break;
        }

        foreach (ForkliftPathMarker fpm in deliveryPathMarkers)
        {
            if (fpm.action == "move")
            {
                yield return StartCoroutine(LinearMove(fpm.transform.position));

                if (fpm.forkAction == "lower")
                {
                    yield return StartCoroutine(MoveForks(new Vector3(forks.transform.position.x, forksDefaultLevel.y + forksGroundOffset, forks.transform.position.z)));
                }
                else if (fpm.forkAction == "load")
                {
                    materials = fpm.interactable;
                    materials.transform.parent = forks.transform;
                    yield return StartCoroutine(MoveForks(new Vector3(forks.transform.position.x, forksDefaultLevel.y - forksGroundOffset, forks.transform.position.z)));
                }
                else if (fpm.forkAction == "raise")
                {
                    yield return StartCoroutine(MoveForks(new Vector3(forks.transform.position.x, forksDefaultLevel.y, forks.transform.position.z)));
                }
                else if (fpm.forkAction == "unload")
                {
                    yield return StartCoroutine(MoveForks(new Vector3(forks.transform.position.x, forksDefaultLevel.y + forksGroundOffset, forks.transform.position.z)));
                    materials.transform.parent = fpm.interactable.transform;
                }
            }
            else if (fpm.action == "turn")
            {
                int turnStartIndex = deliveryPathMarkers.IndexOf(fpm) - 1;
                yield return StartCoroutine(Turn(deliveryPathMarkers[turnStartIndex], fpm));

                if (fpm.forkAction == "lower")
                {
                    yield return StartCoroutine(MoveForks(new Vector3(forks.transform.position.x, forksDefaultLevel.y + forksGroundOffset, forks.transform.position.z)));
                }
                else if (fpm.forkAction == "load")
                {
                    materials = fpm.interactable.GetComponent<LumberSpawn>().slot0;
                    materials.transform.parent = forks.transform;
                    StartCoroutine(fpm.interactable.GetComponent<LumberSpawn>().SpawnLumber());
                    yield return StartCoroutine(MoveForks(new Vector3(forks.transform.position.x, forksDefaultLevel.y, forks.transform.position.z)));
                }
                else if (fpm.forkAction == "raise")
                {
                    yield return StartCoroutine(MoveForks(new Vector3(forks.transform.position.x, forksDefaultLevel.y, forks.transform.position.z)));
                }
                else if (fpm.forkAction == "unload")
                {
                    yield return StartCoroutine(MoveForks(new Vector3(forks.transform.position.x, forksDefaultLevel.y + forksGroundOffset, forks.transform.position.z)));
                    materials.transform.parent = fpm.interactable.transform;
                }

                //yield return StartCoroutine(Turn(fpm, deliveryPathMarkers[turnStartIndex]));
            }
        }
        routineRunning = false;
    }

    public bool IsIdle()
    {
        return !routineRunning;
    }

    // Verify all coroutine processes are terminated.
    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
