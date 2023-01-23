using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    None,
    Flat, // ÆòÁö
    Mountain, // »ê¾Ç
    Snow, // ´«
    Forest, // ½£
}



[CreateAssetMenu(fileName = "New Tile",menuName = "Scriptable Objects/Tile")]
public class TileInfo : ScriptableObject
{
    [SerializeField] private TileType type;
    public TileType Type { get { return type; } }

    [SerializeField] private int productivity;
    public int Productivity { get { return productivity; } }

    [SerializeField] private Tile tileObject;
    public Tile TileObject { get { return tileObject; } }
}
