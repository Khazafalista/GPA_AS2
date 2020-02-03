using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public GameObject wallPrefab;

    public float scaleY = 1.0f;
    public float scaleZ = 1.0f;

    public float radius = 50;

    private void Awake()
    {
        GameObject wall;
        for(int i = 0; i < 20; i++)
        {
            wall = Instantiate(wallPrefab, new Vector3(radius * Mathf.Cos(Mathf.Deg2Rad * (9.0f + 18.0f * (float)i)), 
                                                       0, 
                                                       radius * Mathf.Sin(Mathf.Deg2Rad * (9.0f + 18.0f * (float)i))), 
                                                       Quaternion.LookRotation(Vector3.zero - new Vector3(radius * Mathf.Cos(Mathf.Deg2Rad * (9.0f + 18.0f * (float)i)),
                                                       0,
                                                       radius * Mathf.Sin(Mathf.Deg2Rad * (9.0f + 18.0f * (float)i)))));
            wall.transform.localScale += new Vector3(Mathf.PI * 2.0f * radius / 20.0f, scaleY, scaleZ);
        }
    }
}
