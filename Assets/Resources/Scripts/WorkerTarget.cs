using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorkerTarget : MonoBehaviour
{
    public string animAction;
    public Transform grabTarget;
    public Transform setTarget;
    public Transform handTargetRight;
    public Transform handTargetLeft;
    public Transform lookTarget;
    public int order;
}
