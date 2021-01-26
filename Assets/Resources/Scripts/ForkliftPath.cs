using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForkliftPath : MonoBehaviour
{
    public string item;
    public string location;
    public List<ForkliftPathMarker> nodes;
}
