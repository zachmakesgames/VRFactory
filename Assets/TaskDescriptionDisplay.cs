using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskDescriptionDisplay : MonoBehaviour
{
    public Text TaskDescriptionText;

    private string[] TaskDescriptions = {
        "- Go to the board storage area.\n" +
        "- Pick up a band board and carry it over to the building table and place it\n" +
        "- Repeat the same thing for the 2nd band board.",

        "- Pick up joist hangers and start installing them one by one by nailing them to the band board.",

        "- Pick up a joist from the board storage area.\n" +
        "- Start with one end (rim joist) and continue with the floor joists.",

        "- Nail the joists to the joist hangers."
    };
    void Update()
    {
        TaskDescriptionText.text = TaskDescriptions[2];
    }
}
