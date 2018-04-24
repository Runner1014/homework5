
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{
    public GameObject diskPrefab;

    private List<DiskData> used = new List<DiskData>();
    private List<DiskData> free = new List<DiskData>();

    private void Awake()
    {
        diskPrefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Disk"), Vector3.zero, Quaternion.identity);
        diskPrefab.SetActive(false);
    }

    /// <summary>
    /// 获得飞碟
    /// </summary>
    /// <param name="round"></param>
    /// <returns></returns>
    public GameObject GetDisk(int round, ActionMode mode)
    {
        GameObject newDisk = null;
        if (free.Count > 0)
        {
            newDisk = free[0].gameObject;
            free.Remove(free[0]);
        }
        else
        {
            newDisk = GameObject.Instantiate<GameObject>(diskPrefab, Vector3.zero, Quaternion.identity);
            newDisk.AddComponent<DiskData>();
        }

        //根据round生成不同的飞碟

        switch (round)
        {
            case 0:
                {
                    newDisk.GetComponent<DiskData>().color = Color.blue;
                    newDisk.GetComponent<DiskData>().speed = 6.0f;
                    float RanX = Random.Range(-2f, 2f) < 0 ? -1 : 1;
                    newDisk.GetComponent<DiskData>().direction = new Vector3(RanX, 1.5f, 0);
                    newDisk.GetComponent<Renderer>().material.color = Color.blue;
                    break;
                }
            case 1:
                {
                    newDisk.GetComponent<DiskData>().color = Color.red;
                    newDisk.GetComponent<DiskData>().speed = 8.0f;
                    float RanX = Random.Range(-2f, 2f) < 0 ? -1 : 1;
                    newDisk.GetComponent<DiskData>().direction = new Vector3(RanX, 1.2f, 0);
                    newDisk.GetComponent<Renderer>().material.color = Color.red;
                    break;
                }
            case 2:
                {
                    newDisk.GetComponent<DiskData>().color = Color.black;
                    newDisk.GetComponent<DiskData>().speed = 10.0f;
                    float RanX = Random.Range(-2f, 2f) < 0 ? -1 : 1;
                    newDisk.GetComponent<DiskData>().direction = new Vector3(RanX, 0.9f, 0);
                    newDisk.GetComponent<Renderer>().material.color = Color.black;
                    break;
                }
        }

        if (mode == ActionMode.PHYSICS)
        {
            newDisk.AddComponent<Rigidbody>();
        }

        if(mode == ActionMode.KINEMATIC)
        {
            Destroy(newDisk.GetComponent<Rigidbody>());
        }

        used.Add(newDisk.GetComponent<DiskData>());
        newDisk.name = newDisk.GetInstanceID().ToString();
        return newDisk;
    }

    public void FreeDisk(GameObject disk)
    {
        DiskData tmp = null;
        foreach (DiskData i in used)
        {
            if (disk.GetInstanceID() == i.gameObject.GetInstanceID())
            {
                tmp = i;
            }
        }
        if (tmp != null)
        {
            tmp.gameObject.SetActive(false);
            free.Add(tmp);
            used.Remove(tmp);
        }
    }

}