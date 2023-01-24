using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using static MapManager;

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

        public Vector3Int position;

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

    public static GroundTile NullTile;

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
            return NullTile;
        }
    }

    /// <returns>한 타일에서 일정 거리 내에 있는 타일들의 배열
    /// </returns>
    public GroundTile[] GetNeighborTiles(GroundTile CenterTile)
    {
        List<Vector3Int> CoordinateList = new();

        CoordinateList.AddRange(GetNeighborCoordinate(CenterTile.position));

        List<GroundTile> TilesToReturn = new();

        foreach (Vector3Int _coordinate in CoordinateList)
        {
            if (GetTile(_coordinate) != NullTile)
            {
                TilesToReturn.Add(GetTile(_coordinate));
            }
        }

        return TilesToReturn.ToArray();
    }
    /// <returns>한 타일에서 일정 거리 내에 있는 타일들의 배열
    /// </returns>
    public GroundTile[] GetNeighborTiles(GroundTile CenterTile, int range)
    {
        List<Vector3Int> CoordinateList = new();

        void AddCoordinate(Vector3Int _coordinate)
        {
            if (!CoordinateList.Contains(_coordinate))
            {
                CoordinateList.Add(_coordinate);
            }
        } //해당 좌표의 타일을 반환될 타일 리스트에 추가

        CoordinateList.Add(CenterTile.position);

        if (range >= 1)
        {
            for (int i = 0; i < range; i++)
            {
                List<Vector3Int> InstanceList = new();

                foreach (Vector3Int _coordinate in CoordinateList)
                {
                    InstanceList.Add(_coordinate);
                }

                foreach (Vector3Int _coordinate in InstanceList)
                {
                    foreach (Vector3Int _neighborCoordinate in GetNeighborCoordinate(_coordinate))
                    {
                        AddCoordinate(_neighborCoordinate);
                    }
                }
            }
        }

        List<GroundTile> TilesToReturn = new();

        foreach (Vector3Int _coordinate in CoordinateList)
        {
            if (GetTile(_coordinate) != NullTile)
            {
                TilesToReturn.Add(GetTile(_coordinate));
            }
        }

        return TilesToReturn.ToArray();
    }
    /// <returns>타일들에서 일정 거리 내에 있는 모든 타일들의 배열
    /// </returns>
    public GroundTile[] GetNeighborTiles(GroundTile[] CenterTiles, int range)
    {
        List<Vector3Int> CoordinateList = new();

        void AddCoordinate(Vector3Int _coordinate)
        {
            if (!CoordinateList.Contains(_coordinate))
            {
                CoordinateList.Add(_coordinate);
            }
        } //해당 좌표의 타일을 반환될 타일 리스트에 추가

        foreach(GroundTile _centerTile in CenterTiles)
        {
            CoordinateList.Add(_centerTile.position);
        }

        if (range >= 1)
        {
            for (int i = 0; i < range; i++)
            {
                List<Vector3Int> InstanceList = new();

                foreach (Vector3Int _coordinate in CoordinateList)
                {
                    InstanceList.Add(_coordinate);
                }

                foreach (Vector3Int _coordinate in InstanceList)
                {
                    foreach (Vector3Int _neighborCoordinate in GetNeighborCoordinate(_coordinate))
                    {
                        AddCoordinate(_neighborCoordinate);
                    }
                }
            }
        }

        List<GroundTile> TilesToReturn = new();

        foreach (Vector3Int _coordinate in CoordinateList)
        {
            if (GetTile(_coordinate) != NullTile)
            {
                TilesToReturn.Add(GetTile(_coordinate));
            }
        }

        return TilesToReturn.ToArray();
    }

    /// <returns>한 타일의 일정거리에 있는 외곽선 타일들의 배열
    /// </returns>
    public GroundTile[] GetOutlineTiles(GroundTile CenterTile, int range)
    {
        List<GroundTile> TilesToReturn = new();

        if (range < 1)
        {
            TilesToReturn.Add(CenterTile);
            return TilesToReturn.ToArray();
        }

        TilesToReturn.AddRange(GetNeighborTiles(CenterTile, range));

        foreach (GroundTile TileToRemove in GetNeighborTiles(CenterTile, range - 1))
        {
            TilesToReturn.Remove(TileToRemove);
        }

        return TilesToReturn.ToArray();
    }
    /// <returns>타일들의 일정거리의 외곽선 타일들의 배열
    /// </returns>
    public GroundTile[] GetOutlineTiles(GroundTile[] CenterTiles, int range)
    {
        List<GroundTile> TilesToReturn = new();

        if (range < 1)
        {
            foreach (GroundTile _centerTile in CenterTiles)
            {
                TilesToReturn.Add(_centerTile);
            }
            return TilesToReturn.ToArray();
        }

        TilesToReturn.AddRange(GetNeighborTiles(CenterTiles, range));

        foreach (GroundTile TileToRemove in GetNeighborTiles(CenterTiles, range - 1))
        {
            TilesToReturn.Remove(TileToRemove);
        }

        return TilesToReturn.ToArray();
    }



    private void Awake()
    {
        NullTile = new GroundTile() { info = tileInfoList[0] };
    }

    private void Start()
    {
        GenerateMap(16, 12);
        GenerateLand();
    }

    //테스트용 맵 생성 함수, 제대로 된 전체 맵 생성은 추후 구현띠
    #region 후추수정영역
    void GenerateMap(int row, int column)
    {
        tileList = new GroundTile[row, column];

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                SetGroundTile(new Vector3Int(i, j), 1);
            }
        }
    }
    void GenerateLand()
    {
        GenerateSnow();
        GenerateForest();
        GenerateForest();
        GenerateMountain();
    }

    void GenerateSnow()
    {
        int snow_x = UnityEngine.Random.Range(0, tileList.GetLength(0));
        int snow_y = UnityEngine.Random.Range(0, tileList.GetLength(1));

        GroundTile firstTile = SetGroundTile(new Vector3Int(snow_x, snow_y), 3);
        int secondTileIndex = UnityEngine.Random.Range(1, GetNeighborTiles(firstTile, 3).Length);
        GroundTile secondTile = SetGroundTile(GetNeighborTiles(firstTile, 3)[secondTileIndex], 3);

        SetGroundTiles(GetNeighborTiles(new GroundTile[2] { firstTile, secondTile }, 3), 3);
    }
    void GenerateForest()
    {
        int forest_x = UnityEngine.Random.Range(0, tileList.GetLength(0));
        int forest_y = UnityEngine.Random.Range(0, tileList.GetLength(1));

        GroundTile firstTile = SetGroundTile(new Vector3Int(forest_x, forest_y), 4);
        int secondTileIndex = UnityEngine.Random.Range(1, GetNeighborTiles(firstTile).Length);
        GroundTile secondTile = SetGroundTile(GetNeighborTiles(firstTile)[secondTileIndex], 4);

        int mountainIndex = UnityEngine.Random.Range(1, GetOutlineTiles(new GroundTile[2] { firstTile, secondTile }, 3).Length);
        SetGroundTile(GetOutlineTiles(new GroundTile[2] { firstTile, secondTile }, 3)[secondTileIndex], 2);
    }
    void GenerateMountain()
    {
        int mountain_x = UnityEngine.Random.Range(0, tileList.GetLength(0));
        int mountain_y = UnityEngine.Random.Range(0, tileList.GetLength(1));

        GroundTile firstTile = SetGroundTile(new Vector3Int(mountain_x, mountain_y), 2);
        int secondTileIndex = UnityEngine.Random.Range(1, GetNeighborTiles(firstTile).Length);
        SetGroundTile(GetNeighborTiles(firstTile)[secondTileIndex], 2);
    }
    #endregion

    /// <summary>
    /// 최초 타일을 배치합니다.
    /// </summary>
    private GroundTile SetGroundTile(Vector3Int _coordinate, int tileInfoIndex)
    {
        try
        {
            tileList[_coordinate.x, _coordinate.y] = new GroundTile() { info = TileInfoList[tileInfoIndex], position = _coordinate };
            GroundTileMap.SetTile(new Vector3Int(_coordinate.x, _coordinate.y), tileList[_coordinate.x, _coordinate.y].info.TileObject);

            return tileList[_coordinate.x, _coordinate.y];
        }
        catch(IndexOutOfRangeException)
        {
            return NullTile;
        }
    }
    /// <summary>
    /// 최초 타일을 배치합니다.
    /// </summary>
    private GroundTile SetGroundTile(GroundTile groundTile, int tileInfoIndex)
    {
        if(groundTile == NullTile)
        {
            return NullTile;
        }

        groundTile.info = TileInfoList[tileInfoIndex];
        GroundTileMap.SetTile(groundTile.position, groundTile.info.TileObject);

        return groundTile;
    }

    /// <summary>
    /// 최초 타일을 배치합니다.
    /// </summary>
    private GroundTile[] SetGroundTiles(GroundTile[] groundTiles, int tileInfoIndex)
    {
        List<GroundTile> TilesToReturn = new();

        foreach (GroundTile groundTile in groundTiles)
        {
            if (groundTile == NullTile)
            {
                continue;
            }

            groundTile.info = TileInfoList[tileInfoIndex];
            GroundTileMap.SetTile(groundTile.position, groundTile.info.TileObject);

            TilesToReturn.Add(groundTile);
        }

        return TilesToReturn.ToArray();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DebugTile(CurrentMouseOverlayTile);
        }
    }

    //테스트함수
    public static void DebugTile(GroundTile tile)
    {
        Debug.Log("타일 종류 : " + tile.info.Type + " / " + "타일 생산포인트 : " + tile.Productivity);
        Debug.Log("--------------");
    }

    /// <summary>
    /// 육각 타일의 주변 좌표를 반환하는 함수. 클래스내 다른 함수에서만 사용됨
    /// </summary>
    private Vector3Int[] GetNeighborCoordinate(Vector3Int CenterCoordinate)
    {
        List<Vector3Int> CoordinateToReturn = new();

        void AddCoordinate(Vector3Int _coordinate)
        {
            if (!CoordinateToReturn.Contains(_coordinate))
            {
                CoordinateToReturn.Add(_coordinate);
            }
        } //해당 좌표를 반환될 좌표 리스트에 추가

        AddCoordinate(CenterCoordinate);

        if (CenterCoordinate.y % 2 == 0)
        {
            AddCoordinate(CenterCoordinate + new Vector3Int(1, 0));
            AddCoordinate(CenterCoordinate + new Vector3Int(-1, 0));
            AddCoordinate(CenterCoordinate + new Vector3Int(0, 1));
            AddCoordinate(CenterCoordinate + new Vector3Int(-1, 1));
            AddCoordinate(CenterCoordinate + new Vector3Int(0, -1));
            AddCoordinate(CenterCoordinate + new Vector3Int(-1, -1));
        }
        else
        {
            AddCoordinate(CenterCoordinate + new Vector3Int(1, 0));
            AddCoordinate(CenterCoordinate + new Vector3Int(-1, 0));
            AddCoordinate(CenterCoordinate + new Vector3Int(0, 1));
            AddCoordinate(CenterCoordinate + new Vector3Int(1, 1));
            AddCoordinate(CenterCoordinate + new Vector3Int(0, -1));
            AddCoordinate(CenterCoordinate + new Vector3Int(1, -1));
        }

        return CoordinateToReturn.ToArray();
    }
}
