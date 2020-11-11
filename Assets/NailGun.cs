using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGun : MonoBehaviour
{

    public float Range = 100f;

    //Delay between when the nail gun can fire
    public float FireDelay = 0.2f;

    //The location to cast the hit ray from
    public Transform NailOrigin;


    //The prefab to spawn at the hit location
    public GameObject NailObject;


    //Private Variables

    //The time since the last fire
    float FireTime = 0f;

    //Is the nail gun delaying until the next fire?
    bool InFireDelay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If the button Fire1 is clicked and InFireDelay is false, then fire
        if (Input.GetButtonDown("Fire1") && InFireDelay == false)
        {
            //Set InFireDelay to true because we just fired
            InFireDelay = true;

            //Reset the fire time
            FireTime = 0f;

            //Fire the nailgun
            Fire();

        }
        else
        {
            //Otherwise check if we're still in the fire delay
            if (InFireDelay == true)
            {
                //If yes and FireTime is less than FireDelay, update the FireTime
                if (FireTime < FireDelay) 
                { 
                    FireTime += Time.deltaTime;
                }
                else
                {
                    //Otherwise FireTime >= FireDelay so we're not in fire delay anymore
                    InFireDelay = false;

                }
            }
        }
    }


    void Fire()
    {
        //Use a RaycastHit to check if the nailgun would hit something
        RaycastHit HitInfo;
        
        //Check if we hit something
        if(Physics.Raycast(NailOrigin.position, NailOrigin.forward, out HitInfo, Range))
        {


            //Do some hot vector math to determine how the nail should be oriented

            //Calculate the vector from the nail gun to the hit point
            Vector3 Ray = NailOrigin.position - HitInfo.point;

            //Then get the angle between the up vector and the ray
            float Angle = Vector3.Angle(new Vector3(0, 1, 0), Ray);

            //Then get the axis to rotate the nail about by using the cross product
            Vector3 Axis = Vector3.Cross(new Vector3(0, 1, 0), Ray);

            //Now we can spawn the nail at the hit point and set the rotation to match the angle it was fired from
            Instantiate(NailObject, HitInfo.point, Quaternion.AngleAxis(Angle, Axis));
        }
    }
}
