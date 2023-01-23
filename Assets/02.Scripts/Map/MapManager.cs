using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [Tooltip("종류별 타일의 정보를 담은 TileInfo타입 에셋의 배열")]
    [SerializeField] private TileInfo[] tileInfoList;
    public TileInfo[] TileInfoList { get { return tileInfoList; } }

    [Tooltip("땅 지형의 타일맵 컴포넌트")]
    [SerializeField] private Tilemap GroundTileMap;

    //육각 타일맵의 좌표는 다음을 따랐습니다. 이 좌표는 tileList[*,*]의 인덱스 값이 됩니다.
    //https://docs.unity3d.com/kr/2021.3/Manual/Tilemap-Hexagonal.html

    /// <summary>
    /// 땅에 배치된 타일의 클래스
    /// </summary>
    public class GroundTile
    {
        public TileInfo info; //이 타일종류의 정보

        public bool isOwned = false;
        // +나중에 속한 팀을 의미하는 필드를 추가하게 될 듯

        public bool isBuildingOnTile = false;

        public int Productivity
        {
            get
            {
                return isBuildingOnTile ? 0 : info.Productivity;
            }
        } //이 타일의 최종 생산포인트
    }

    /// <summary>
    /// 땅에 배치된 타일들의 배열
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

    /// <returns> 해당 좌표에 있는 타일
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

    //테스트용 맵 생성 함수, 제대로 된 전체 맵 생성은 추후 구현띠
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
            Debug.Log("타일 종류 : " + CurrentMouseOverlayTile.info.Type);
            Debug.Log("타일 생산포인트 : " + CurrentMouseOverlayTile.Productivity);
            Debug.Log("-------------------");
        }
    }
}
