using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Queue<GameObject> objQ = new();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GetObject(PoolType.Sphere);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            GetObject(PoolType.Cube);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!objQ.Count.Equals(0))
            {
                Resend(objQ.Dequeue());
            }
           
        }  
    }






    private void GetObject(PoolType poolType)
    {
        var obj = PoolManager.Instance.GetObject(poolType);
        obj.transform.position = transform.position;
        objQ.Enqueue(obj);
    }

    private void Resend(GameObject obj)
    {
        PoolManager.Instance.ReturnObject(obj);
    }
}
