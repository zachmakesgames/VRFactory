using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoughPlumbElectricalManager : MonoBehaviour
{
    public WorkerNavigation[] workers;

    public GameObject readyUnit;

    public WorkerTarget[] targets;

    public WorkerTarget toolsTarget;

    public FloorCrane floorCrane;

    public WallSetManager wallSetManager;

    public GameObject wallSetFloorJacks;

    public bool floorbuildclear;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        floorbuildclear = false;
        yield return StartCoroutine(WaitForUnit());
    }

    public IEnumerator WaitForUnit()
    {
        floorbuildclear = false;
        yield return new WaitUntil(() => readyUnit != null);
        yield return StartCoroutine(floorCrane.PickupFloor(readyUnit));
        yield return StartCoroutine(floorCrane.SetFloorOnRack());
        floorbuildclear = true;
        StartCoroutine(DoWork());
    }

    public IEnumerator DoWork()
    {
        yield return StartCoroutine(workers[0].MoveTo(toolsTarget));
        yield return StartCoroutine(workers[0].PlayAction("drillPickup"));

        for (int j = 0; j < targets.Length; j++)
        {  
            yield return StartCoroutine(workers[0].MoveTo(targets[j]));
            yield return StartCoroutine(workers[0].PlayAction(targets[j].animAction));
            yield return new WaitForSeconds(1.0f);
        }
        
        yield return StartCoroutine(floorCrane.SetFloorAtWallSet(readyUnit, wallSetFloorJacks));
        wallSetManager.currentUnit = readyUnit;
        readyUnit = null;
        StartCoroutine(WaitForUnit());
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
