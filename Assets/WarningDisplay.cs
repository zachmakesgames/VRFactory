using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningDisplay : MonoBehaviour
{
    public Text WarningText;

    private int prefab = 0;

    // Update is called once per frame
    void Update()
    {
        if(prefab == 0)
        {
            WarningText.text = "This should be a two person carry.";
        }
        else
        {
            WarningText.text = "";
        }
    }
}
