using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject manager;

    private void Awake()
    {
        if (Manager.instance == null)
        {
            Instantiate(manager);
        }
    }
}
