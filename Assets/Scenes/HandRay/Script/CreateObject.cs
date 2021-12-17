using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{
    public GameObject sphere;


    public void Create()
    {
        Vector3 PointPos = GameObject.Find("Point").transform.position;
        Quaternion PointRot = GameObject.Find("Point").transform.rotation;

        GameObject gameObject1 = Instantiate(sphere, PointPos, PointRot);
    }
}
