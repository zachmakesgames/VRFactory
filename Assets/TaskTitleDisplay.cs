using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTitleDisplay : MonoBehaviour
{
    public Text taskTitleText;

    private string[] taskTitles = { 
        "Place Band Boards",
        "Place Joist Hangers",
        "Place Joists",
        "Secure Joists"
    };

    private int prefab = 0;

    private void Update()
    {
        
        taskTitleText.text = taskTitles[prefab];
    }
}
