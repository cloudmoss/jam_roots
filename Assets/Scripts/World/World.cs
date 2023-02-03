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

    private void Awake() 
    {
        Current = this;    
    }

    void Start()
    {
        StartCoroutine(Generate());
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
        var i = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                Vector2Int neighbourPosition = new Vector2Int(tile.Position.x + x, tile.Position.y + y);
                neighbours[i] = IsInBounds(neighbourPosition) ? GetTile(neighbourPosition) : new Tile(new Vector2Int(), null);
                i++;
            }
        }

        return neighbours;
    }

    public bool IsVoid(Tile tile)
    {
        return (tile.Def == null);
    }
}
