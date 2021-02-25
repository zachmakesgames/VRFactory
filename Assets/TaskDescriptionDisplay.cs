using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskDescriptionDisplay : MonoBehaviour
{
    public Text TaskDescriptionText;
    void Update()
    {
        TaskDescriptionText.text = "Testing";
    }
}
