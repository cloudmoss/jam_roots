using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen
{
    public event System.Action OnGenerated;
    public bool IsGenerated { get; private set; }

    public int cityBlockSize = 10;
    public Tile[,] grid;

    public Vector2Int[] SpawnPoints { get; private set; }

    private int size;
    private int roadWidth = 2;

    public IEnumerator Generate(int size)
    {
        grid = new Tile[size, size];
        this.size = grid.GetLength(0);

        var wallTile = TileLibrary.GetTile("Wall");

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                grid[x, y] = new Tile(new Vector2Int(x, y), wallTile);
            }
        }

        GenerateStreets();
        GeneratePlaza();

        foreach(var def in TileLibrary.Tiles)
        {
            var obj = new GameObject(def.Name);
            obj.transform.SetParent(World.Current.transform);
            var mf = obj.AddComponent<MeshFilter>();
            var mr = obj.AddComponent<MeshRenderer>();

            mr.material = new Material(Shader.Find("Unlit/Texture"));
            mr.material.mainTexture = def.Texture;
            
            mf.mesh = GenerateMeshFor(def);
        }

        SetupSpawnPoints();

        Debug.Log("World generated");
        IsGenerated = true;
        OnGenerated?.Invoke();

        yield return null;
    }

    void SetupSpawnPoints()
    {
        var spawns = new List<Vector2Int>();
        var tile = TileLibrary.GetTile("Pavement");

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if ((x == 0 || x == size - 1 || y == 0 || y == size - 1) && grid[x, y].Def == tile)
                {
                    spawns.Add(new Vector2Int(x, y));
                }
            }
        }

        Debug.Log("Spawn points: " + spawns.Count);
        SpawnPoints = spawns.ToArray();
    }

    void GenerateStreets()
    {
        var roadWidth = 2;
        var tile = TileLibrary.GetTile("Pavement");

        // vertical streets
        for(int x = cityBlockSize; x < size - cityBlockSize; x += cityBlockSize + roadWidth)
        {
            for(int y = 0; y < size; y ++)
            {
                grid[x, y] = new Tile(new Vector2Int(x, y), tile);
            }

            for (int y = 0; y < size; y++)
            {
                grid[x + 1, y] = new Tile(new Vector2Int(x + 1, y), tile);
            }
        }

        // horizontal streets
        for (int y = cityBlockSize; y < size - cityBlockSize; y += cityBlockSize + roadWidth)
        {
            for (int x = 0; x < size; x++)
            {
                if (grid[x, y].Def != tile)
                    grid[x, y] = new Tile(new Vector2Int(x, y), tile);
            }

            for (int x = 0; x < size; x++)
            {
                if (grid[x, y + 1].Def != tile)
                    grid[x, y + 1] = new Tile(new Vector2Int(x, y + 1), tile);
            }
        }
    }

    void GeneratePlaza()
    {
        var tile = TileLibrary.GetTile("Grass");
        var middle = (size / (cityBlockSize + roadWidth)) / 2 * (cityBlockSize + roadWidth);

        for (int x = middle; x < middle + cityBlockSize; x++ )
        {
            for (int y = middle; y < middle + cityBlockSize; y++)
            {
                grid[x, y] = new Tile(new Vector2Int(x, y), tile);
            }
        }
    }

    public Mesh GenerateMeshFor(TileDef tiletype)
    {
        var mesh = new Mesh();

        var tris = new List<int>();
        var verts = new List<Vector3>();
        var uvs = new List<Vector2>();
        var offset = new Vector2(0, 0);

        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                var tile = grid[x, y];
                if (tile.Def == tiletype)
                {
                    verts.Add(new Vector3(tile.Position.x + offset.x - 0.5f,   tile.Position.y + offset.y - 0.5f));
                    verts.Add(new Vector3(tile.Position.x + offset.x + 0.5f,   tile.Position.y + offset.y - 0.5f));
                    verts.Add(new Vector3(tile.Position.x + offset.x + 0.5f,   tile.Position.y + offset.y + 0.5f));
                    verts.Add(new Vector3(tile.Position.x + offset.x - 0.5f ,  tile.Position.y + offset.y + 0.5f));

                    tris.Add(verts.Count - 3);
                    tris.Add(verts.Count - 4);
                    tris.Add(verts.Count - 2);

                    tris.Add(verts.Count - 2);
                    tris.Add(verts.Count - 4);
                    tris.Add(verts.Count - 1);

                    uvs.Add(new Vector2(0, 0));
                    uvs.Add(new Vector2(1, 0));
                    uvs.Add(new Vector2(1, 1));
                    uvs.Add(new Vector2(0, 1));
                }
            }
        }

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateBounds();

        return mesh;
    }
}
