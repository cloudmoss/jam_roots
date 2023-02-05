using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public WorldGen Generator { get { return _worldGen; } }

    public bool IsGenerated { get { return _worldGen.IsGenerated; } }
    public static World Current { get; private set; }
    public static Tile[,] Grid { get { return Current._worldGen.grid; } }
    public BuildingGenerator BuildingGenerator { get; private set; }

    [SerializeField] private int _size = 100;
    [SerializeField] private RoadTiler _roadTiler;

    private WorldGen _worldGen;
    private Texture2D _pathingTexture;
    private GameObject _pathingOverlay;
    private Color[] _pathingClearColors;

    private void Awake() 
    {
        Current = this;    
        _worldGen = new WorldGen();
        BuildingGenerator = gameObject.GetComponent<BuildingGenerator>();
        _pathingOverlay = transform.Find("_pathingOverlay").gameObject;
        _pathingTexture = new Texture2D(_size, _size);
        _pathingTexture.filterMode = FilterMode.Point;
        _pathingOverlay.GetComponent<MeshRenderer>().material.mainTexture = _pathingTexture;
        _pathingOverlay.transform.localScale = new Vector3(_size, _size, 1);
        _pathingOverlay.transform.position = new Vector3(_size / 2f - 0.5f, _size / 2f - 0.5f, -9);

        _pathingClearColors = new Color[_size * _size];
        _pathingTexture.SetPixels(_pathingClearColors);
        _pathingTexture.Apply();

        _roadTiler.Init();
        TileLibrary.GetTile("Road").OverrideTexture(_roadTiler.atlas);
        _worldGen.roadTiler = _roadTiler;

    }

    void Start()
    {
        StartCoroutine(_worldGen.Generate(_size));
    }

    public void ShowPathingTiles(Tile[] tiles)
    {
        _pathingTexture.SetPixels(_pathingClearColors);

        if (tiles != null)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                _pathingTexture.SetPixel(tiles[i].Position.x, tiles[i].Position.y, new Color(0.7f, 1, 0.7f, 0.05f));
            }
        }

        _pathingTexture.Apply();
    }

    public bool IsInBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < _size && position.y >= 0 && position.y < _size;
    }

    public Tile GetTile(Vector2Int position)
    {
        if (!IsInBounds(position))
        {
            Debug.LogError("Tile " + position + " is out of bounds");
            return new Tile();
        }

        return Grid[position.x, position.y];
    }

    public Tile[] GetNeighbours(Tile tile)
    {
        Tile[] neighbours = new Tile[8];
        var i = -1;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                i++;
                Vector2Int neighbourPosition = new Vector2Int(tile.Position.x + x, tile.Position.y + y);
                neighbours[i] = IsInBounds(neighbourPosition) ? GetTile(neighbourPosition) : new Tile(new Vector2Int(), null);
            }
        }

        return neighbours;
    }

    public bool IsVoid(Tile tile)
    {
        return (tile.Def == null);
    }
}
