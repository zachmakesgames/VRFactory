using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleUnit : MonoBehaviour
{
    // The top level parent GameObjects.
    public GameObject floorParent;
    public GameObject wallParent;
    // The toggleable floor for workers.
    public GameObject dynamicFlooring;
    // The joist workerTarget lists split by task.
    public List<WorkerTarget> floorBoardsPerimiterLeftOut;
    public List<WorkerTarget> floorBoardsPerimiterLeftIn;
    public List<WorkerTarget> floorBoardsPerimiterRightOut;
    public List<WorkerTarget> floorBoardsPerimiterRightIn;
    public List<WorkerTarget> floorBoardsJoistsOuter;
    public List<WorkerTarget> floorBoardsJoistsInner;
    public List<WorkerTarget> nailgunTargets;
    // The Decking/Plywood workerTarget list.
    public List<WorkerTarget> decking;
    // The JoistHanger workerTarget lists split by task.
    public List<WorkerTarget> joisthangersLeft;
    public List<WorkerTarget> joisthangersRight;
    public bool loaded = false;
    private int numTaskListsDone;
    private int numTasksDone;

    // Start is called before the first frame update
    void Start()
    {
        if (floorParent == null)
        {
            floorParent = transform.Find("Floor").gameObject;
        }
        if (wallParent == null)
        {
            wallParent = transform.Find("Walls").gameObject;
        }
        if (dynamicFlooring == null)
        {
            dynamicFlooring = transform.Find("ToggleDeckingPlatform").gameObject;
        }

        Transform boardsParent = floorParent.transform.Find("Boards");
        Transform deckingParent = floorParent.transform.Find("Decking");
        Transform joisthangerParent = floorParent.transform.Find("Joisthangers");
        Transform joistparent = boardsParent.Find("Joists");

        floorBoardsPerimiterLeftOut = GetWorkerTargets(boardsParent.Find("Perim_Left_Out"));
        floorBoardsPerimiterLeftIn = GetWorkerTargets(boardsParent.Find("Perim_Left_In"));
        floorBoardsPerimiterRightOut = GetWorkerTargets(boardsParent.Find("Perim_Right_Out"));
        floorBoardsPerimiterRightIn = GetWorkerTargets(boardsParent.Find("Perim_Right_In"));
        floorBoardsJoistsOuter = GetWorkerTargets(joistparent.Find("J_Outer"));
        floorBoardsJoistsInner = GetWorkerTargets(joistparent.Find("J_Inner"));

        decking = GetWorkerTargets(deckingParent);

        joisthangersLeft = GetWorkerTargets(joisthangerParent.Find("JH_Left"));
        joisthangersRight = GetWorkerTargets(joisthangerParent.Find("JH_Right"));

        for (int i = 0; i < floorBoardsJoistsInner.Count; i++)
        {
            GameObject go1 = new GameObject();
            GameObject go2 = new GameObject();
            WorkerTarget wt1 = go1.AddComponent<WorkerTarget>();
            WorkerTarget wt2 = go2.AddComponent<WorkerTarget>();
            wt1.animAction = "nailgunShootJoist";
            wt2.animAction = "nailgunShootJoist";
            go1.transform.position = new Vector3(floorBoardsJoistsInner[i].transform.position.x, floorBoardsJoistsInner[i].transform.position.y, floorBoardsPerimiterLeftOut[0].transform.position.z);
            go2.transform.position = new Vector3(floorBoardsJoistsInner[i].transform.position.x, floorBoardsJoistsInner[i].transform.position.y, floorBoardsPerimiterRightOut[0].transform.position.z);
            go1.transform.rotation = floorBoardsPerimiterLeftOut[0].transform.rotation;
            go2.transform.rotation = floorBoardsPerimiterRightOut[0].transform.rotation;
            nailgunTargets.Add(wt1);
            nailgunTargets.Add(wt2);
        }

        numTaskListsDone = 0;
        numTasksDone = 0;
        loaded = true;
    }

    public int NumTasks()
    {
        return floorBoardsPerimiterLeftIn.Count + floorBoardsPerimiterLeftOut.Count + floorBoardsPerimiterRightIn.Count + floorBoardsPerimiterRightOut.Count + floorBoardsJoistsInner.Count + floorBoardsJoistsOuter.Count + decking.Count + nailgunTargets.Count;
    }

    public WorkerTarget GetNextTarget()
    {
        WorkerTarget ret = null;
        bool done = false;
        switch (numTaskListsDone)
        {
            case 0:
                if (numTasksDone < floorBoardsPerimiterLeftIn.Count)
                {
                    ret = floorBoardsPerimiterLeftIn[numTasksDone];
                    numTasksDone += 1;
                }
                else
                {
                    numTasksDone = 0;
                    numTaskListsDone += 1;
                }
                break;
            case 1:
                if (numTasksDone < floorBoardsPerimiterRightIn.Count)
                {
                    ret = floorBoardsPerimiterRightIn[numTasksDone];
                    numTasksDone += 1;
                }
                else
                {
                    numTasksDone = 0;
                    numTaskListsDone += 1;
                }
                break;
            case 2:
                if (numTasksDone < joisthangersLeft.Count)
                {
                    ret = joisthangersLeft[numTasksDone];
                    numTasksDone += 1;
                }
                else
                {
                    numTasksDone = 0;
                    numTaskListsDone += 1;
                }
                break;
            case 3:
                if (numTasksDone < joisthangersRight.Count)
                {
                    ret = joisthangersRight[numTasksDone];
                    numTasksDone += 1;
                }
                else
                {
                    numTasksDone = 0;
                    numTaskListsDone += 1;
                }
                break;
            case 4:
                if (numTasksDone < floorBoardsJoistsInner.Count)
                {
                    ret = floorBoardsJoistsInner[numTasksDone];
                    numTasksDone += 1;
                }
                else
                {
                    numTasksDone = 0;
                    numTaskListsDone += 1;
                }
                break;
            case 5:  
                if (numTasksDone < floorBoardsPerimiterLeftOut.Count)
                {
                    if (numTasksDone == 1)
                    {
                        ToggleDynamicFloor(true);
                    }

                    ret = floorBoardsPerimiterLeftOut[numTasksDone];
                    numTasksDone += 1;
                }
                else
                {
                    numTasksDone = 0;
                    numTaskListsDone += 1;
                }
                break;
            case 6:
                if (numTasksDone < floorBoardsPerimiterRightOut.Count)
                {
                    ret = floorBoardsPerimiterRightOut[numTasksDone];
                    numTasksDone += 1;
                }
                else
                {
                    numTasksDone = 0;
                    numTaskListsDone += 1;
                }
                break;
            case 7:
                if (numTasksDone < floorBoardsJoistsOuter.Count)
                {
                    ret = floorBoardsJoistsOuter[numTasksDone];
                    numTasksDone += 1;
                }
                else
                {
                    numTasksDone = 0;
                    numTaskListsDone += 1;
                }
                break;
            case 8:
                if (numTasksDone < nailgunTargets.Count)
                {
                    ret = nailgunTargets[numTasksDone];
                    numTasksDone += 1;
                }
                else
                {
                    numTasksDone = 0;
                    numTaskListsDone += 1;
                }
                break;
            case 9:
                if (numTasksDone < decking.Count)
                {
                    ret = decking[numTasksDone];
                    numTasksDone += 1;
                }
                else
                {
                    numTasksDone = 0;
                    numTaskListsDone += 1;
                }
                break;
            default:
                done = true;
                break;
        }

        if (ret == null)
        {
            if (done == true)
            {
                return null;
            }
            else
            {
                return GetNextTarget();
            }
        }
        else
        {
            return ret;
        }
    }

    /// <summary>
    /// Get a sorted list of worker targets from a parent object.
    /// </summary>
    /// <param name="parentObj"> The primary parent GameObject used to search for the secondary parent. Typically use either floorParent or wallParent.</param>
    /// <param name="list"> The list to append the sorted results to.</param>
    /// <param name="pname"> The secondary parent which contains the workerTargets. </param>
    private List<WorkerTarget> GetWorkerTargets(Transform parentObj)
    {
        List<WorkerTarget> list = new List<WorkerTarget>();
        for (int i = 0; i < parentObj.childCount; i++)
        {
            list.Add(parentObj.GetChild(i).gameObject.GetComponentInChildren<WorkerTarget>());
        }
        list.Sort(CompareTargets);
        return list;
    }

    /// <summary>
    /// Compare two WorkerTargets for Sort().
    /// </summary>
    /// <param name="wt1">WorkerTarget One</param>
    /// <param name="wt2">WorkerTarget Two</param>
    /// <returns></returns>
    static int CompareTargets(WorkerTarget wt1, WorkerTarget wt2)
    {
        return wt1.order.CompareTo(wt2.order);
    }

    /// <summary>
    /// Toggle the Active state of the dynamic floor.
    /// </summary>
    public void ToggleDynamicFloor(bool b)
    {
        dynamicFlooring.SetActive(b);
    } 
}
