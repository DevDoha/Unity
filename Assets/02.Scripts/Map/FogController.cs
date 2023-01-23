using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogController : MonoBehaviour
{
    //미구현 상태

    Tilemap FogTileMap; //안개 타일맵

    private void Awake()
    {
        FogTileMap = GetComponent<Tilemap>();
    }
}
