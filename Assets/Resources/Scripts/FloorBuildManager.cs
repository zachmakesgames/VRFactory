using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBuildManager : MonoBehaviour
{
    public GameObject unitPrefab;
    public GameObject testingUnit;
    private GameObject currentUnit;
    public GameObject startUnit;
    private ModuleUnit currentUnitScript;
    private GameObject nextBoard;
    private WorkerNavigation nextWorker = null;

    public List<WorkerNavigation> floorbuildWorkers;
    public List<WorkerNavigation> laborpool;

    public List<WorkerTarget> randTargets;

    public RoughPlumbElectricalManager rpem;

    public Forklift forklift;
    public ForkliftCrane forkliftCrane;
    public WorkerTarget joisthangerTarget;
    public WorkerTarget drillTarget;
    public WorkerTarget forkliftUnloadTarget;
    private GameObject craneMaterials = null;

    private WorkerTarget chopsaw;
    private WorkerTarget tablesaw;

    public GameObject twofoureightPrefab;
    public GameObject twosixeightPrefab;
    public GameObject twotensixteenPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //chopsaw = transform.Find("ChopsawTarget").gameObject.GetComponent<WorkerTarget>();
        //tablesaw = transform.Find("TablesawTarget").gameObject.GetComponent<WorkerTarget>();
        nextBoard = null;
        StartCoroutine(DispatchRandoms());
        StartCoroutine(ClearUnit());
    }

    private IEnumerator DispatchRandoms()
    {
        int taskCount = 0;
        while (laborpool.Count > 0)
        {
            foreach (WorkerNavigation wn in laborpool)
            {
                if (wn.routineRunning == false && wn.actionRunning == false)
                {
                    WorkerTarget wt = randTargets[taskCount];
                    StartCoroutine(wn.MoveToPlay(wt));
                    taskCount++;
                    if (taskCount >= randTargets.Count)
                    {
                        taskCount = 0;
                    }
                    break;
                }
            }
            yield return null;
        }
    }

    private IEnumerator ClearUnit()
    {
        yield return StartCoroutine(forkliftCrane.MoveToWorkPosition());
        rpem.readyUnit = startUnit;
        yield return new WaitUntil(() => rpem.floorbuildclear);
        yield return StartCoroutine(FloorBuildProcess());
    }

    private IEnumerator ForkliftRequest(string lumberType)
    {
        if (forklift.IsIdle())
        {
            yield return StartCoroutine(forklift.DeliverItem("FloorBuild", lumberType));
        }
        else
        {
            yield return new WaitUntil(() => forklift.IsIdle());
            yield return StartCoroutine(forklift.DeliverItem("FloorBuild", lumberType));
        }
        yield return StartCoroutine(forkliftCrane.PickupMaterials());
        craneMaterials = forkliftCrane.currMaterials;
    }

    public void AddFromWorkerPool()
    {
        if (laborpool.Count > 0)
        {
            WorkerNavigation wn = laborpool[0];
            laborpool.RemoveAt(0);
            floorbuildWorkers.Add(wn);
        }
        
    }

    private bool GetIdleWorker()
    {
        foreach(WorkerNavigation wn in floorbuildWorkers)
        {
            if (wn.routineRunning == false && wn.actionRunning == false)
            {
                nextWorker = wn;
                return true;
            }
        }
        return false;
    }

    private IEnumerator CheckNextBoard(string bname)
    {
        // If craneMaterials are empty get more.
        if (craneMaterials == null)
        {
            yield return StartCoroutine(ForkliftRequest(bname));
            nextBoard = craneMaterials.transform.GetChild(0).gameObject;
        }
        else
        {
            try
            {
                nextBoard = craneMaterials.transform.GetChild(0).gameObject;
            }
            catch (UnityException e)
            {
                string n = e.ToString();
                nextBoard = null;
            }

            if (nextBoard == null)
            {
                Destroy(craneMaterials);
                yield return StartCoroutine(ForkliftRequest(bname));
                nextBoard = craneMaterials.transform.GetChild(0).gameObject;
            }
            else
            {
                if (!nextBoard.name.Contains(bname))
                {
                    Destroy(craneMaterials);
                    yield return StartCoroutine(ForkliftRequest(bname));
                    nextBoard = craneMaterials.transform.GetChild(0).gameObject;
                }
            }
        }
    }

    // Function to build a floor.
    private IEnumerator FloorBuildProcess()
    {   
        currentUnit = Instantiate(unitPrefab, testingUnit.transform.position, testingUnit.transform.rotation);
        currentUnitScript = currentUnit.GetComponent<ModuleUnit>();
        yield return new WaitUntil(() => currentUnitScript.loaded);
        WorkerTarget wt = currentUnitScript.GetNextTarget();
        while (wt != null)
        {
            if (wt.setTarget == null && wt.grabTarget == null)
            {
                if (wt.animAction == "nailgunShootJoist")
                {
                    yield return new WaitUntil(() => GetIdleWorker());
                    if (nextWorker.currentHandObjectRight != nextWorker.nailgun)
                    {
                        yield return StartCoroutine(nextWorker.MoveTo(drillTarget));
                        yield return StartCoroutine(nextWorker.PlayAction("nailgunPickup"));
                    }
                    StartCoroutine(nextWorker.MoveToPlay(wt));
                }
            }
            else
            {
                if (wt.setTarget.gameObject.name.Contains("2x10x16ft"))
                {
                    yield return CheckNextBoard("2x10x16ft");
                    yield return new WaitUntil(() => GetIdleWorker());
                    if (nextWorker.currentHandObjectRight == nextWorker.handDrill)
                    {
                        yield return StartCoroutine(nextWorker.MoveTo(drillTarget));
                        yield return StartCoroutine(nextWorker.PlayAction("drillPlace"));
                    }
                    else if (nextWorker.currentHandObjectRight == nextWorker.nailgun)
                    {
                        yield return StartCoroutine(nextWorker.MoveTo(drillTarget));
                        yield return StartCoroutine(nextWorker.PlayAction("nailgunPlace"));
                    }
                    StartCoroutine(nextWorker.DoGrabPlace(forkliftUnloadTarget, wt, nextBoard, wt.setTarget.gameObject, forkliftUnloadTarget.animAction, wt.animAction, 3));
                    nextBoard = null;
                }
                else if (wt.setTarget.gameObject.name.Contains("Plywood"))
                {
                    yield return CheckNextBoard("Plywood");
                    yield return new WaitUntil(() => GetIdleWorker());
                    if (nextWorker.currentHandObjectRight == nextWorker.handDrill)
                    {
                        yield return StartCoroutine(nextWorker.MoveTo(drillTarget));
                        yield return StartCoroutine(nextWorker.PlayAction("drillPlace"));
                    }
                    else if(nextWorker.currentHandObjectRight == nextWorker.nailgun)
                    {
                        yield return StartCoroutine(nextWorker.MoveTo(drillTarget));
                        yield return StartCoroutine(nextWorker.PlayAction("nailgunPlace"));
                    }
                    StartCoroutine(nextWorker.DoGrabPlace(forkliftUnloadTarget, wt, nextBoard, wt.setTarget.gameObject, "sheetPickupStanding", wt.animAction, 4));
                }
                else if (wt.setTarget.gameObject.name.Contains("Joisthanger"))
                {
                    yield return new WaitUntil(() => GetIdleWorker());
                    if (nextWorker.currentHandObjectRight != nextWorker.handDrill)
                    {
                        yield return StartCoroutine(nextWorker.MoveTo(drillTarget));
                        yield return StartCoroutine(nextWorker.PlayAction(drillTarget.animAction));
                    }
                    StartCoroutine(nextWorker.DoGrabPlace(joisthangerTarget, wt, null, wt.setTarget.gameObject, joisthangerTarget.animAction, wt.animAction));
                }
                else
                {

                }
            }
            yield return null;
            wt = currentUnitScript.GetNextTarget();    
        }
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
