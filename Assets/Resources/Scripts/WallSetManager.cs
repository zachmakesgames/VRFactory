using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSetManager : MonoBehaviour
{
    public GameObject currentUnit;

    public GameObject rollingJacks;

    private float floorjacksDefault_Z = -2.4974f;
    private float floorjacksPwallSet1_Z = -2.258f;
    private float floorjacksPwallSet2_Z = -1.936f;
    private float floorjacksSwallSet_Z = -1.516f;

    public SingleWallCrane singleWallCrane_pWall1;
    public SingleWallCrane singleWallCrane_eWall1;
    public SingleWallCrane singleWallCrane_sWall1;
    public SingleWallCrane singleWallCrane_sWall2;

    // SideWallCrane

    public GameObject pWall_1A;
    public GameObject pWall_2A;
    public GameObject pWall_3A;
    public GameObject pWall_4A;
    public GameObject pWall_5A;
    public GameObject pWall_6A;
    public GameObject pWall_7A;
    public GameObject pWall_8A;
    public GameObject pWall_9A;
    public GameObject pWall_10A;
    public GameObject pWall_1;
    public GameObject pWall_2;
    public GameObject pWall_3;
    public GameObject pWall_4;
    public GameObject pWall_5;
    public GameObject pWall_6;
    public GameObject pWall_7;
    public GameObject pWall_8;
    public GameObject pWall_9;
    public GameObject pWall_10;
    public GameObject sWall_1;
    public GameObject sWall_2;
    public GameObject sWall_3;
    public GameObject sWall_4;
    public GameObject eWall_1;
    public GameObject eWall_2;

    public Vector3 pWall1_craneOffset;
    public Vector3 eWall1_craneOffset;
    public Vector3 sWall1_craneOffset;

    public float rotationSpeed = 25.0f;
    public float floorjackSpeed = 0.05f;
    
    // Start is called before the first frame update
    void Start()
    {
        //floorjacksDefault_Z = rollingJacks.transform.localPosition.z;
        StartCoroutine(WaitForUnit());
    }

    public IEnumerator WaitForUnit()
    {
        yield return new WaitWhile(() => currentUnit == null);
        StartCoroutine(PwallSetOne());
    }

    public IEnumerator MoveJacks(float z)
    {
        Vector3 target = rollingJacks.transform.localPosition;
        target.z = z;
        while (Vector3.Distance(rollingJacks.transform.localPosition, target) > 0.001f)
        {
            rollingJacks.transform.localPosition = Vector3.MoveTowards(rollingJacks.transform.localPosition, target, floorjackSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        
    }

    public IEnumerator PwallSetOne()
    {
        Transform walls = currentUnit.transform.Find("Walls");
        
        float oldX = singleWallCrane_pWall1.sliderail.transform.position.x;
        float oldY = singleWallCrane_pWall1.hook.transform.position.y;
        float oldZ = singleWallCrane_pWall1.winch.transform.position.z;

        yield return StartCoroutine(MoveJacks(floorjacksPwallSet1_Z));

        Transform p10 = walls.Find("Wall_P10");
        yield return PickupWall(singleWallCrane_pWall1, pWall_10, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_10, p10, pWall1_craneOffset);

        Transform p9 = walls.Find("Wall_P9");
        yield return PickupWall(singleWallCrane_pWall1, pWall_9, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_9, p9, pWall1_craneOffset);

        Transform p8 = walls.Find("Wall_P8");
        yield return PickupWall(singleWallCrane_pWall1, pWall_8, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_8, p8, pWall1_craneOffset);

        Transform p7 = walls.Find("Wall_P7");
        yield return PickupWall(singleWallCrane_pWall1, pWall_7, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_7, p7, pWall1_craneOffset);

        Transform p6 = walls.Find("Wall_P6");
        yield return PickupWall(singleWallCrane_pWall1, pWall_6, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_6, p6, pWall1_craneOffset);

        Transform p5 = walls.Find("Wall_P5");
        yield return PickupWall(singleWallCrane_pWall1, pWall_5, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_5, p5, pWall1_craneOffset);

        Transform p4 = walls.Find("Wall_P4");
        yield return PickupWall(singleWallCrane_pWall1, pWall_4, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_4, p4, pWall1_craneOffset);

        Transform p3 = walls.Find("Wall_P3");
        yield return PickupWall(singleWallCrane_pWall1, pWall_3, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_3, p3, pWall1_craneOffset);

        Transform p2 = walls.Find("Wall_P2");
        yield return PickupWall(singleWallCrane_pWall1, pWall_2, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_2, p2, pWall1_craneOffset);

        Transform p1 = walls.Find("Wall_P1");
        yield return PickupWall(singleWallCrane_pWall1, pWall_1, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_1, p1, pWall1_craneOffset);

        Transform p1A = walls.Find("Wall_P1_A");
        yield return PickupWall(singleWallCrane_pWall1, pWall_1A, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_1A, p1A, pWall1_craneOffset);

        Transform p2A = walls.Find("Wall_P2_A");
        yield return PickupWall(singleWallCrane_pWall1, pWall_2A, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_2A, p2A, pWall1_craneOffset);

        Transform p3A = walls.Find("Wall_P3_A");
        yield return PickupWall(singleWallCrane_pWall1, pWall_3A, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_3A, p3A, pWall1_craneOffset);

        Transform p4A = walls.Find("Wall_P4_A");
        yield return PickupWall(singleWallCrane_pWall1, pWall_4A, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_4A, p4A, pWall1_craneOffset);

        Transform p5A = walls.Find("Wall_P5_A");
        yield return PickupWall(singleWallCrane_pWall1, pWall_5A, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_5A, p5A, pWall1_craneOffset);

        Transform p6A = walls.Find("Wall_P6_A");
        yield return PickupWall(singleWallCrane_pWall1, pWall_6A, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_6A, p6A, pWall1_craneOffset);

        Transform p7A = walls.Find("Wall_P7_A");
        yield return PickupWall(singleWallCrane_pWall1, pWall_7A, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_7A, p7A, pWall1_craneOffset);

        Transform p8A = walls.Find("Wall_P8_A");
        yield return PickupWall(singleWallCrane_pWall1, pWall_8A, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_8A, p8A, pWall1_craneOffset);

        Transform p9A = walls.Find("Wall_P9_A");
        yield return PickupWall(singleWallCrane_pWall1, pWall_9A, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_9A, p9A, pWall1_craneOffset);

        Transform p10A = walls.Find("Wall_P10_A");
        yield return PickupWall(singleWallCrane_pWall1, pWall_10A, pWall1_craneOffset);
        yield return SetWall(singleWallCrane_pWall1, pWall_10A, p10A, pWall1_craneOffset);

        oldX = singleWallCrane_eWall1.sliderail.transform.position.x;
        oldY = singleWallCrane_eWall1.hook.transform.position.y;
        oldZ = singleWallCrane_eWall1.winch.transform.position.z;

        yield return StartCoroutine(MoveJacks(floorjacksPwallSet2_Z));

        Transform e2 = walls.Find("Wall_E2");
        yield return PickupWall(singleWallCrane_eWall1, eWall_2, eWall1_craneOffset);
        yield return SetWall(singleWallCrane_eWall1, eWall_2, e2, eWall1_craneOffset);

        Transform e1 = walls.Find("Wall_E1");
        yield return PickupWall(singleWallCrane_eWall1, eWall_1, eWall1_craneOffset);
        yield return SetWall(singleWallCrane_eWall1, eWall_1, e1, eWall1_craneOffset);

        oldX = singleWallCrane_sWall1.sliderail.transform.position.x;
        oldY = singleWallCrane_sWall1.hook.transform.position.y;
        oldZ = singleWallCrane_sWall1.winch.transform.position.z;

        yield return StartCoroutine(MoveJacks(floorjacksSwallSet_Z));

        Transform m1 = walls.Find("Wall_M1");
        yield return PickupWall(singleWallCrane_sWall1, sWall_1, sWall1_craneOffset);
        yield return SetWall(singleWallCrane_sWall1, sWall_1, m1, sWall1_craneOffset);

        Transform m3 = walls.Find("Wall_M3");
        yield return PickupWall(singleWallCrane_sWall1, sWall_3, sWall1_craneOffset);
        yield return SetWall(singleWallCrane_sWall1, sWall_3, m3, sWall1_craneOffset);

        oldX = singleWallCrane_sWall2.sliderail.transform.position.x;
        oldY = singleWallCrane_sWall2.hook.transform.position.y;
        oldZ = singleWallCrane_sWall2.winch.transform.position.z;

        Transform m2 = walls.Find("Wall_M2");
        yield return PickupWall(singleWallCrane_sWall2, sWall_2, sWall1_craneOffset);
        yield return SetWall(singleWallCrane_sWall2, sWall_2, m2, sWall1_craneOffset);

        Transform m4 = walls.Find("Wall_M4");
        yield return PickupWall(singleWallCrane_sWall2, sWall_4, sWall1_craneOffset);
        yield return SetWall(singleWallCrane_sWall2, sWall_4, m4, sWall1_craneOffset);
    }

    public IEnumerator PickupWall(SingleWallCrane singleWallCrane, GameObject wall, Vector3 offset)
    {
        float oldY = singleWallCrane.hook.transform.position.y;
        yield return StartCoroutine(singleWallCrane.MoveSlideRail(wall.transform.position.x + offset.x));
        yield return StartCoroutine(singleWallCrane.MoveWinches(wall.transform.position.z + offset.z));
        yield return StartCoroutine(singleWallCrane.MoveLiftingBeam(wall.transform.position.y + offset.y));
        wall.transform.parent = singleWallCrane.hook.transform;
        yield return StartCoroutine(singleWallCrane.MoveLiftingBeam(oldY));
    }

    public IEnumerator SetWall(SingleWallCrane singleWallCrane, GameObject wall, Transform targetWall, Vector3 offset)
    {
        float oldY = singleWallCrane.hook.transform.position.y;
        yield return StartCoroutine(singleWallCrane.MoveWinches(targetWall.position.z + offset.z));
        yield return StartCoroutine(singleWallCrane.MoveSlideRail(targetWall.position.x + offset.x));
        if (wall.transform.rotation != targetWall.rotation)
        {
            yield return StartCoroutine(RotateWall(wall.transform, targetWall.transform));
        }
        yield return StartCoroutine(singleWallCrane.MoveLiftingBeam(targetWall.position.y + offset.y));
        Destroy(wall);
        MeshRenderer[] mrs = targetWall.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs) { mr.enabled = true; };
        yield return StartCoroutine(singleWallCrane.MoveLiftingBeam(oldY));
    }

    public IEnumerator RotateWall(Transform start, Transform goal)
    {
        while (start.rotation != goal.rotation)
        {
            start.rotation = Quaternion.RotateTowards(start.rotation, goal.rotation, rotationSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
