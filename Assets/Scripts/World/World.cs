using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public event System.Action OnGenerated;

    public bool IsGenerated { get; private set;}
    public static World Current { get; private set; }
    public static Tile[,] Grid { get { return Current._worldGrid; } }

    [SerializeField] private Vector2Int _size = new Vector2Int(300, 300);

    private Tile[,] _worldGrid;
    private Texture2D _pathingTexture;
    private GameObject _pathingOverlay;
    private Color[] _pathingClearColors;

    private void Awake() 
    {
        Current = this;    
        _pathingOverlay = transform.Find("_pathingOverlay").gameObject;
        _pathingTexture = new Texture2D(_size.x, _size.y);
        _pathingTexture.filterMode = FilterMode.Point;
        _pathingOverlay.GetComponent<MeshRenderer>().material.mainTexture = _pathingTexture;
        _pathingOverlay.transform.localScale = new Vector3(_size.x, _size.y, 1);
        _pathingOverlay.transform.position = new Vector3(_size.x / 2f - 0.5f, _size.y / 2f - 0.5f, -100);

        _pathingClearColors = new Color[_size.x * _size.y];
        _pathingTexture.SetPixels(_pathingClearColors);
        _pathingTexture.Apply();
    }

    void Start()
    {
        StartCoroutine(Generate());
    }

    public void ShowPathingTiles(Tile[] tiles)
    {
        _pathingTexture.SetPixels(_pathingClearColors);

        if (tiles != null)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                _pathingTexture.SetPixel(tiles[i].Position.x, tiles[i].Position.y, new Color(1, 1, 1, 0.2f));
            }
        }

        _pathingTexture.Apply();
    }

    IEnumerator Generate()
    {
        _worldGrid = new Tile[_size.x, _size.y];

        var pavementTileDef = TileLibrary.GetTile("Pavement");

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                _worldGrid[x, y] = new Tile(new Vector2Int(x, y), pavementTileDef);
            }
        }

        Debug.Log("World generated");
        IsGenerated = true;
        OnGenerated?.Invoke();

        yield return null;
    }

    public bool IsInBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < _size.x && position.y >= 0 && position.y < _size.y;
    }

    public Tile GetTile(Vector2Int position)
    {
        if (!IsInBounds(position))
        {
            Debug.LogError("Tile " + position + " is out of bounds");
            return new Tile();
        }

        return _worldGrid[position.x, position.y];
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
