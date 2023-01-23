using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [Tooltip("������ Ÿ���� ������ ���� TileInfoŸ�� ������ �迭")]
    [SerializeField] private TileInfo[] tileInfoList;
    public TileInfo[] TileInfoList { get { return tileInfoList; } }

    [Tooltip("�� ������ Ÿ�ϸ� ������Ʈ")]
    [SerializeField] private Tilemap GroundTileMap;

    //���� Ÿ�ϸ��� ��ǥ�� ������ �������ϴ�. �� ��ǥ�� tileList[*,*]�� �ε��� ���� �˴ϴ�.
    //https://docs.unity3d.com/kr/2021.3/Manual/Tilemap-Hexagonal.html

    /// <summary>
    /// ���� ��ġ�� Ÿ���� Ŭ����
    /// </summary>
    public class GroundTile
    {
        public TileInfo info; //�� Ÿ�������� ����

        public bool isOwned = false;
        // +���߿� ���� ���� �ǹ��ϴ� �ʵ带 �߰��ϰ� �� ��

        public bool isBuildingOnTile = false;

        public int Productivity
        {
            get
            {
                return isBuildingOnTile ? 0 : info.Productivity;
            }
        } //�� Ÿ���� ���� ��������Ʈ
    }

    /// <summary>
    /// ���� ��ġ�� Ÿ�ϵ��� �迭
    /// </summary>
    private GroundTile[,] tileList;

    public GroundTile CurrentMouseOverlayTile
    {
        get
        {
            Vector3Int TileCoordinate = GroundTileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            
            return GetTile(TileCoordinate);
        }
    }

    /// <returns> �ش� ��ǥ�� �ִ� Ÿ��
    /// </returns>
    public GroundTile GetTile(Vector3Int Coordinate)
    {
        try
        {
            return tileList[Coordinate.x, Coordinate.y];
        }
        catch(IndexOutOfRangeException)
        {
            return null;
        }
    }

    public GroundTile[] GetAroundTiles(Vector3Int CenterTileCoordinate, int range = 1)
    {
        return null;
    }


    private void Start()
    {
        GenerateMap(8, 8);
    }

    //�׽�Ʈ�� �� ���� �Լ�, ����� �� ��ü �� ������ ���� ������
    void GenerateMap(int row, int column)
    {
        tileList = new GroundTile[row, column];

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                tileList[i, j] = new GroundTile() { info = TileInfoList[0] };
                GroundTileMap.SetTile(new Vector3Int(i, j, 0), tileList[i, j].info.TileObject);
            }
        }
    }

    private void Update()
    {
        if(CurrentMouseOverlayTile != null)
        {
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Ÿ�� ���� : " + CurrentMouseOverlayTile.info.Type);
            Debug.Log("Ÿ�� ��������Ʈ : " + CurrentMouseOverlayTile.Productivity);
            Debug.Log("-------------------");
        }
    }
}
