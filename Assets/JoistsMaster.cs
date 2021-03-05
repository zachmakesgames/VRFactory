using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoistsMaster : MonoBehaviour
{

    public List<GameObject> JoistObjects = new List<GameObject>();

    public int AutoCompleteCount;

    private int CurrentJoistIndex = 0;
    private int JoistCount;

    private bool IsEnabled;
    private bool IsComplete = false;

    private GameObject CurrentJoist;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < this.transform.childCount; ++i)
        {
            JoistObjects.Add(this.transform.GetChild(i).gameObject);
            //Debug.Log("Child name: " + this.transform.GetChild(i).transform.name);
        }

        this.JoistCount = JoistObjects.Count;
        Debug.Log("Joist controller has " + this.JoistCount.ToString() + " joists");
        this.CurrentJoist = this.JoistObjects[this.CurrentJoistIndex];
        FloorStation_Joist joistscript = this.CurrentJoist.GetComponent<FloorStation_Joist>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsEnabled)
        {
            
            if(this.CurrentJoistIndex < this.JoistCount)
            {
                //Auto complete the joists if enough of them are already built
                if (this.AutoCompleteCount > 0 && this.CurrentJoistIndex >= this.AutoCompleteCount)
                {
                    Debug.Log("Auto completing joists");
                    foreach (GameObject g in this.JoistObjects)
                    {
                        FloorStation_Joist s = g.GetComponent<FloorStation_Joist>();
                        s.MakeComplete();
                    }

                    this.IsComplete = true;
                    this.Disable();
                }
                else
                {
                    FloorStation_Joist s = CurrentJoist.GetComponent<FloorStation_Joist>();
                    if (s.GetCompleted())   ////NULL ref exception
                    {
                        s.Disable();
                        this.CurrentJoist = JoistObjects[++this.CurrentJoistIndex];
                        FloorStation_Joist snew = CurrentJoist.GetComponent<FloorStation_Joist>();
                        snew.Enable();
                    }
                }
            }
            else
            {
                this.IsComplete = true;
                this.Disable();
            }
        }
    }

    public bool GetCompleted()
    {
        return this.IsComplete;
    }

    public void Enable()
    {
        this.IsEnabled = true;

        //Enable the current joist
        FloorStation_Joist joistscript = this.CurrentJoist.GetComponent<FloorStation_Joist>();
        joistscript.Enable(); ////NULL ref exception
    }

    public void Disable()
    {
        this.IsEnabled = false;

        //Disable all subobjects too!
        foreach (GameObject g in this.JoistObjects)
        {
            //Get the script that controls the side walls
            ComponentScript joistscript = g.GetComponent<ComponentScript>();
            if (joistscript != null)
            {
                joistscript.Disable();
            }
        }
    }
}
