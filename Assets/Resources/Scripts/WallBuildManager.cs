using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuildManager : MonoBehaviour
{
    public WorkerNavigation[] workers;
    public GameObject twobyfourDropoff;
    public GameObject twobysixDropoff;
    public WorkerTarget twobyfourTarget;
    public WorkerTarget twobysixTarget;
    public Forklift forklift;
    public WorkerTarget[] busyTargets;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return StartCoroutine(GetBoards(twobyfourTarget));
        yield return StartCoroutine(GetBoards(twobysixTarget));
        yield return StartCoroutine(KeepBusy());
    }


    private bool HasBoards(GameObject g)
    {
        if (g.transform.childCount > 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator GetBoards(WorkerTarget t)
    {
        if (t == twobyfourTarget)
        {
            if (!HasBoards(twobyfourDropoff))
            {
                if (forklift.IsIdle())
                {
                    yield return StartCoroutine(forklift.DeliverItem("WallBuild", "2x4x8ft"));
                }
                else
                {
                    yield return new WaitUntil(() => forklift.IsIdle());
                    yield return StartCoroutine(forklift.DeliverItem("WallBuild", "2x4x8ft"));
                }
            }
        }
        else if (t == twobysixTarget)
        {
            if (!HasBoards(twobysixDropoff))
            {
                if (forklift.IsIdle())
                {
                    yield return StartCoroutine(forklift.DeliverItem("WallBuild", "2x6x8ft"));
                }
                else
                {
                    yield return new WaitUntil(() => forklift.IsIdle());
                    yield return StartCoroutine(forklift.DeliverItem("WallBuild", "2x6x8ft"));
                }
            }
        }  
    }

    private IEnumerator KeepBusy()
    {
        for (int t = 0; t < 10; t++)
        {
            for (int i = 0; i < busyTargets.Length; i++)
            {
                yield return StartCoroutine(workers[0].MoveTo(busyTargets[i]));
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
