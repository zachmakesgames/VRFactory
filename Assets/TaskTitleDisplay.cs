using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTitleDisplay : MonoBehaviour
{
    public Text taskTitleText;

    private void Update()
    {
        taskTitleText.text = "Testing";
    }
}
