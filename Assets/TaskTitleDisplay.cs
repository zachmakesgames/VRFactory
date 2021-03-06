using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTitleDisplay : MonoBehaviour
{
    public Text taskTitleText;

    public GameObject FloorStationPrefab;

    private FloorStationMaster FloorStationScript = null;

    private string[] taskTitles = { 
        "Place Band Boards",
        "Place Joist Hangers",
        "Place Joists",
        "Secure Joists"
    };

    private int prefab = 0;

    void Start()
    {
        if (FloorStationPrefab !=  null)
        {
            this.FloorStationScript = FloorStationPrefab.GetComponent<FloorStationMaster>();
            
        }
    }

    private void Update()
    {
        int currentStage = this.FloorStationScript.GetCurrentStage();
        taskTitleText.text = taskTitles[currentStage];
    }
}
