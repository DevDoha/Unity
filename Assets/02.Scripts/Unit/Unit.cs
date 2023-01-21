using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitInfo info;

    protected Transform tr;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }
}
