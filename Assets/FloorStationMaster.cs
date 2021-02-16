using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorStationMaster : MonoBehaviour
{

    public GameObject SideWallsMasterObject;
    public GameObject HeadersMasterObject;
    public GameObject JoistsMasterObject;


    private SideWallsMaster SWMScript;// = SideWallsMasterObject.GetComponent<SideWallsMaster>();
                                      //private HeadersMaster HMScript;// = HeadersMasterObject.GetComponent<HeadersMaster>();
                                      //private JoistsMaster JMScript;// = JoistsMasterObject.GetComponent<JoistsMaster>();

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

        this.ActivateCurrentStage();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }

    private void CheckCurrentStage()
    {
        switch (this.CurrentStage)
        {
            case 0: this.CheckSideWalls();
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
}
