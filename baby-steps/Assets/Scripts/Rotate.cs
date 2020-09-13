using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed  = 0.5f;
    public List<GameObject> rotate;

    void Update()
    {
        foreach (var item in rotate)
        {
            if(item.transform.name == "Red Apple")
                item.transform.Rotate(0,0,speed);
            else
                item.transform.Rotate(0,speed,0);
        }
    }
}
