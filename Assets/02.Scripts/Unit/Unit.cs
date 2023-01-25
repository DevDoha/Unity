using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    Transform tr;
    Vector3 inputPos; // ���콺�� �Է¹��� ��ġ
    public float moveSpeed;

    private void Start()
    {
        tr = GetComponent<Transform>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position =
            Vector2.MoveTowards(transform.position, inputPos, Time.deltaTime * moveSpeed);
    }
}
