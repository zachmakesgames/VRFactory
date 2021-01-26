using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberSpawn : MonoBehaviour
{
    public GameObject prefab;
    public GameObject slot0;
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;
    public GameObject slot5;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private string startName;
    private int spawnCount;
    //private bool spawning = false;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = slot0.transform.position;
        startRotation = slot0.transform.rotation;
        startName = slot0.name;
        spawnCount = 0;
    }

    public IEnumerator SpawnLumber()
    {
        yield return new WaitForSecondsRealtime(5.0f);

        slot0 = Instantiate(prefab, startPosition, startRotation);
        slot0.transform.parent = transform;
        spawnCount += 1;
        //spawning = false;
    }

    public int GetSpawnCount()
    {
        return spawnCount;
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }

    private void CombineSlot(GameObject slot)
    {
        MeshFilter[] meshFilters = slot.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while(i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        //MeshFilter nfm = slot.AddComponent<MeshFilter>();
        meshFilters[0].mesh = new Mesh();
        meshFilters[0].mesh.CombineMeshes(combine);
        meshFilters[0].gameObject.SetActive(true);
    }
}