using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MakeWTParent : MonoBehaviour
{
    public bool set = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (set)
        {
            WorkerTarget[] wtl = GetComponentsInChildren<WorkerTarget>();
            foreach (WorkerTarget wt in wtl)
            {
                wt.setTarget = wt.gameObject.transform.parent;
            }
            set = false;
        }
    }
}
