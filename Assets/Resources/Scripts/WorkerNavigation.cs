using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class WorkerNavigation : MonoBehaviour
{
    // The workers NavMeshAgent and Animator Components.
    private NavMeshAgent nma;
    private Animator anim;
    // Is the worker running a Coroutine?
    public bool routineRunning = false;
    public bool actionRunning = false;
    // The workers hand sockets.
    public Transform handRight;
    public Transform handLeft;
    // The workers hand drill.
    public GameObject handDrill;
    // The hand drill handle for IK targeting.
    public Transform handDrillHandle;
    // The Workers nailgun.
    public GameObject nailgun;
    // The nailgun handle for IK targeting.
    public Transform nailgunHandle;
    // Current object shown in hand.
    public GameObject currentHandObjectLeft;
    public GameObject currentHandObjectRight;
    // Current IK targets.
    public Transform handRightIKTarget;
    public Transform handLeftIKTarget;
    public Transform lookIKTarget;
    public GameObject destroyObj;
    public GameObject visibiltyObj;
    // GrabBoard() flags for animator stateVar.
    public const int TWO_BY_FOUR_BY_EIGHT = 1;
    public const int TWO_BY_SIX_BY_EIGHT = 2;
    public const int TWO_BY_TEN_BY_SIXTEEN = 3;
    public const int PLYWOOD_SHEET = 4;
    public const int SHEETROCK_SHEET = 5;
    // Prefabs in worker hands. Used in ShowBoard.
    public GameObject twoByfourByeight;
    public GameObject twoBysixByeight;
    public GameObject twoBytenBysixteen;
    public GameObject plywoodSheet;
    public GameObject sheetrockSheet;
    public GameObject joisthanger;
    // Ik targeting On/Off flag.
    private bool doIK;
    private bool nearOtherNav;

    // Start is called before the first frame update
    void Start()
    {
        // Set the private fields. Public fields should be set through the editor. 
        nma = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        doIK = false;
        nearOtherNav = false;
    }

    // IK Callback.
    private void OnAnimatorIK(int layerIndex)
    {
        if (doIK)
        {
            if (handRightIKTarget)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.5f);
                //anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.1f);
                anim.SetIKPosition(AvatarIKGoal.RightHand, handRightIKTarget.position);
                //anim.SetIKRotation(AvatarIKGoal.RightHand, handRightIKTarget.rotation);
            }

            if (handLeftIKTarget)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
                //anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.8f);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, handLeftIKTarget.position);
                //anim.SetIKRotation(AvatarIKGoal.LeftHand, handLeftIKTarget.rotation);
            }

            if (lookIKTarget)
            {
                anim.SetLookAtWeight(0.5f);
                anim.SetLookAtPosition(lookIKTarget.position);
            }
        }
        
    }

    public IEnumerator DoGrabPlace(WorkerTarget wt1, WorkerTarget wt2, GameObject destObj, GameObject visObj, string action1, string action2, int stateVar1 = 0, int stateVar2 = 0)
    {
        routineRunning = true;
        //yield return new WaitUntil(() => (anim.GetCurrentAnimatorStateInfo(0).IsTag("IdleAnim") && !anim.IsInTransition(0)));
        destroyObj = destObj;
        yield return StartCoroutine(MoveTo(wt1));
        yield return StartCoroutine(PlayAction(action1, stateVar1));
        //yield return new WaitUntil(() => (anim.GetCurrentAnimatorStateInfo(0).IsTag("IdleAnim") && !anim.IsInTransition(0)));
        visibiltyObj = visObj;
        yield return StartCoroutine(MoveTo(wt2));
        yield return StartCoroutine(PlayAction(action2, stateVar2));
        routineRunning = false;
    }

    public IEnumerator AvoidOtherNavs()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(nma.transform.position, nma.pathEndPosition - nma.transform.position, out hit, 10.0f))
            {
                if (hit.transform.gameObject.ToString().Contains("Worker"))
                {
                    //Debug.Log(hit.transform.gameObject.ToString());
                    if (nma.hasPath)
                    {
                        nma.isStopped = true;
                        anim.SetFloat("velocity", -1.0f);
                        if (nma.nextPosition.z > nma.transform.position.z)
                        {
                            nma.nextPosition = nma.nextPosition + new Vector3(0.0f, 0.0f, -0.002f);
                        }
                        else
                        {
                            nma.nextPosition = nma.nextPosition + new Vector3(0.0f, 0.0f, 0.002f);
                        }
                        //yield return new WaitForSeconds(Random.value);
                        anim.SetFloat("velocity", 1.0f);
                        nma.isStopped = false;
                    }
                }   
            }
            yield return null;
        }
    }

    // Move the worker to a WorkerTarget.transform.position and align with WorkerTarget.transform.rotation.
    public IEnumerator MoveTo(WorkerTarget target)
    {
        //Vector3 adjustedPos = target.position;
        //adjustedPos.y = transform.position.y;
        
        bool res = nma.SetDestination(target.gameObject.transform.position);
        if (res)
        {
            actionRunning = true;
            Coroutine avoidance = StartCoroutine(AvoidOtherNavs());
            yield return new WaitWhile(() => (nma.pathPending));

            anim.SetFloat("velocity", 1.0f);
            yield return new WaitWhile(() => nma.remainingDistance > nma.stoppingDistance);
            nma.ResetPath();
            anim.SetFloat("velocity", -1.0f);

            nma.updateRotation = false;
            transform.rotation = target.transform.rotation;
            nma.updateRotation = true;

            StopCoroutine(avoidance);
            actionRunning = false;
        }
        else
        {
            actionRunning = false;
            yield break;
        }
    }

    public IEnumerator MoveToPlay(WorkerTarget target)
    {
        //Vector3 adjustedPos = target.position;
        //adjustedPos.y = transform.position.y;
        bool res = false;
        try
        {
            res = nma.SetDestination(target.gameObject.transform.position);
        }
        catch(System.Exception e)
        {
            //Debug.LogException(e);
        }

        if (res)
        {
            actionRunning = true;
            Coroutine avoidance = StartCoroutine(AvoidOtherNavs());
            yield return new WaitWhile(() => (nma.pathPending));

            anim.SetFloat("velocity", 1.0f);
            yield return new WaitWhile(() => nma.remainingDistance > nma.stoppingDistance);
            nma.ResetPath();
            anim.SetFloat("velocity", -1.0f);

            nma.updateRotation = false;
            transform.rotation = target.transform.rotation;
            nma.updateRotation = true;

            StopCoroutine(avoidance);
            actionRunning = false;
        }
        else
        {
            actionRunning = false;
            yield break;
        }
        yield return StartCoroutine(PlayAction(target.animAction));
    }

    // Play an animation state in the Workers Animator.
    // string state = <Trigger_Name>
    // <Trigger_Name> = a valid Trigger in the Workers Animator.                
    // int stateVar = <stateVar>
    // <stateVar> = an optional valid integer to flag objects the should be shown during the action.
    public IEnumerator PlayAction(string state, int stateVar=0)
    {
        actionRunning = true;
        anim.SetInteger("stateVar", stateVar);
        anim.SetTrigger(state);
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => (anim.GetCurrentAnimatorStateInfo(0).IsTag("IdleAnim") && !anim.IsInTransition(0)));
        actionRunning = false;
    }

    // Shows a board in the Workers hand. 
    // Set the flag <stateVar> in the animator to indicate which board. 
    public void ShowBoard()
    {
        if (currentHandObjectRight == null)
        {
            int stateVar = anim.GetInteger("stateVar");
            switch (stateVar)
            {
                case TWO_BY_FOUR_BY_EIGHT:
                    currentHandObjectRight = twoByfourByeight;
                    break;
                case TWO_BY_SIX_BY_EIGHT:
                    currentHandObjectRight = twoBysixByeight;
                    break;
                case TWO_BY_TEN_BY_SIXTEEN:
                    currentHandObjectRight = twoBytenBysixteen;
                    break;
                case PLYWOOD_SHEET:
                    currentHandObjectRight = plywoodSheet;
                    break;
                case SHEETROCK_SHEET:
                    currentHandObjectRight = sheetrockSheet;
                    break;
                default:
                    break;
            }
            if (currentHandObjectRight != null)
            {
                MeshRenderer[] cmr = currentHandObjectRight.GetComponentsInChildren<MeshRenderer>();
                MeshRenderer mr = currentHandObjectRight.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.enabled = true;
                }
                foreach(MeshRenderer m in cmr)
                {
                    m.enabled = true;
                }
                handRightIKTarget = currentHandObjectRight.transform;
            }
        }

        if (destroyObj)
        {
            Destroy(destroyObj);
            destroyObj = null;
        }
    }

    // Hide the Workers currently visible board.
    public void HideBoard()
    {
        if (currentHandObjectRight != null)
        {
            MeshRenderer[] cmr = currentHandObjectRight.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer mr = currentHandObjectRight.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.enabled = false;
            }
            foreach (MeshRenderer m in cmr)
            {
                m.enabled = false;
            }
            handRightIKTarget = null;
            currentHandObjectRight = null;
        }

        if (visibiltyObj)
        {
            visibiltyObj.GetComponent<MeshRenderer>().enabled = true;
            visibiltyObj = null;
        }
    }

    // Show the workers hand drill.
    public void ShowDrill()
    {
        if (currentHandObjectRight == null)
        {
            currentHandObjectRight = handDrill;
            MeshRenderer[] cmr = currentHandObjectRight.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer mr = currentHandObjectRight.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.enabled = true;
            }
            foreach (MeshRenderer m in cmr)
            {
                m.enabled = true;
            }
            handRightIKTarget = handDrillHandle.transform;
        }     
    }

    // Hide the Workers hand drill.
    public void HideDrill()
    {
        if (currentHandObjectRight != null)
        {
            MeshRenderer[] cmr = currentHandObjectRight.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer mr = currentHandObjectRight.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.enabled = false;
            }
            foreach (MeshRenderer m in cmr)
            {
                m.enabled = false;
            }
            handRightIKTarget = null;
            currentHandObjectRight = null;
        }
    }

    // Show the workers nailgun.
    public void ShowNailgun()
    {
        if (currentHandObjectRight == null)
        {
            currentHandObjectRight = nailgun;
            MeshRenderer[] cmr = currentHandObjectRight.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer mr = currentHandObjectRight.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.enabled = true;
            }
            foreach (MeshRenderer m in cmr)
            {
                m.enabled = true;
            }
            handRightIKTarget = nailgunHandle.transform;
        }
    }

    // Hide the Workers hand drill.
    public void HideNailgun()
    {
        if (currentHandObjectRight != null)
        {
            MeshRenderer[] cmr = currentHandObjectRight.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer mr = currentHandObjectRight.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.enabled = false;
            }
            foreach (MeshRenderer m in cmr)
            {
                m.enabled = false;
            }
            handRightIKTarget = null;
            currentHandObjectRight = null;
        }
    }

    // Shows a joisthanger in the Workers hand.
    public void ShowJoisthanger()
    {
        if (currentHandObjectLeft == null)
        { 
            MeshRenderer[] cmr = joisthanger.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer mr = joisthanger.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.enabled = true;
            }
            foreach (MeshRenderer m in cmr)
            {
                m.enabled = true;
            }
            handLeftIKTarget = joisthanger.transform;
            currentHandObjectLeft = joisthanger;
        }
    }

    // Hide the Workers currently visible board.
    public void HideJoisthanger()
    {
        if (currentHandObjectLeft != null)
        {
            MeshRenderer[] cmr = joisthanger.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer mr = joisthanger.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.enabled = false;
            }
            foreach (MeshRenderer m in cmr)
            {
                m.enabled = false;
            }
            handLeftIKTarget = null;
            currentHandObjectLeft = null;
        }

        if (visibiltyObj)
        {
            visibiltyObj.GetComponent<MeshRenderer>().enabled = true;
            visibiltyObj = null;
        }
    }

    // Toggle the Workers IK flag.
    public void ToggleIK()
    {
        doIK = !doIK;
    }

    // Cleanup any processes on exit.
    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
