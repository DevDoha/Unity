using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogController : MonoBehaviour
{
    //�̱��� ����

    Tilemap FogTileMap; //�Ȱ� Ÿ�ϸ�

    private void Awake()
    {
        FogTileMap = GetComponent<Tilemap>();
    }
}
