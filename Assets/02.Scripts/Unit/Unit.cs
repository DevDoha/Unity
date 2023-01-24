using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    Transform tr;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }
}
