using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorStationMaster : MonoBehaviour
{

    public GameObject SideWallsMasterObject;
    public GameObject HeadersMasterObject;
    public GameObject JoistsMasterObject;


    private SideWallsMaster SWMScript;// = SideWallsMasterObject.GetComponent<SideWallsMaster>();
    private HeadersMaster HMScript;// = HeadersMasterObject.GetComponent<HeadersMaster>();
    private JoistsMaster JMScript;// = JoistsMasterObject.GetComponent<JoistsMaster>();

    private int CurrentStage = 0;
    //0 == SideWalls
    //1 == Headers
    //2 == Joists
    //3 == FloorBoards
    //4 == Completed


    private bool IsComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        SWMScript = SideWallsMasterObject.GetComponent<SideWallsMaster>();
        HMScript = HeadersMasterObject.GetComponent<HeadersMaster>();
        JMScript = JoistsMasterObject.GetComponent<JoistsMaster>();

        this.ActivateCurrentStage();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.IsComplete)
        {
            this.CheckCurrentStage();
        }
    }

    public bool GetCompleted()
    {
        return this.IsComplete;
    }

    private void ActivateCurrentStage()
    {
        switch (this.CurrentStage)
        {
            case 0: SWMScript.Enable();
                break;
            case 1: HMScript.Enable();
                break;
            case 2: JMScript.Enable(); 
                break;
            case 3: this.IsComplete = true;
                break;
        }
    }

    public int GetCurrentStage()
    {
        return this.CurrentStage;
    }

    private void CheckCurrentStage()
    {
        switch (this.CurrentStage)
        {
            case 0: this.CheckSideWalls();
                break;
            case 1: this.CheckHeaders();
                break;
            case 2: this.CheckJoists();
                break;
        }
    }

    private void CheckSideWalls()
    {
        if (SWMScript.GetCompleted())
        {
            SWMScript.Disable();
            this.CurrentStage += 1;

            this.ActivateCurrentStage();
        }
    }

    private void CheckHeaders()
    {
        if (HMScript.GetCompleted())
        {
            HMScript.Disable();
            this.CurrentStage += 1;
            this.ActivateCurrentStage();
        }
    }

    private void CheckJoists()
    {
        if (JMScript.GetCompleted())
        {
            JMScript.Disable();
            this.CurrentStage += 1;
            this.ActivateCurrentStage();
        }
    }
}
