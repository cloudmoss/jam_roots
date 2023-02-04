using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadTiler
{
    private class Def
    {
        public bool[,] signs;
        public Texture2D[] textures;
        public float angle = 0;
        public int atlasIndicesStart;
        public int atlasIndicesEnd;

        public Def(bool[,] signs, Texture2D[] textures, float angle = 0)
        {
            this.signs = signs;
            this.textures = textures;
            this.angle = angle;
        }
    }

    [SerializeField] private Texture2D[] _top;
    [SerializeField] private Texture2D[] _bottom;
    [SerializeField] private Texture2D[] _left;
    [SerializeField] private Texture2D[] _right;

    [SerializeField] private Texture2D[] _crossBotRight;
    [SerializeField] private Texture2D[] _crossBotLeft;
    [SerializeField] private Texture2D[] _crossTopRight;
    [SerializeField] private Texture2D[] _crossTopLeft;

    private List<Def> _defs = new List<Def>();
    public Texture2D atlas;

    public void Init()
    {

        // Straight
        _defs.Add(new Def(new bool[3, 3] {
            { false, false, false },
            { true,  true,  true },
            { true,  true, true }
        }, _top, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { true, true, true },
            { true,  true,  true },
            { false,  false, false }
        }, _bottom, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { true, true, false },
            { true,  true,  false },
            { true,  true, false }
        }, _right, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { false, true, true },
            { false,  true,  true },
            { false,  true, true }
        }, _left, 0));


        // Cross
        _defs.Add(new Def(new bool[3, 3] {
            { true, true, true },
            { true,  true,  true },
            { false,  true, true }
        }, _crossBotLeft, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { true, true, true },
            { true,  true,  true },
            { true,  true, false }
        }, _crossBotRight, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { false, true, true },
            { true,  true,  true },
            { true,  true, true }
        }, _crossTopLeft, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { true, true, false },
            { true,  true,  true },
            { true,  true, true }
        }, _crossTopRight, 0));

        // T
        _defs.Add(new Def(new bool[3, 3] {
            { false, false, true },
            { true,  true,  true },
            { true,  true, true }
        }, _top, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { true, false, false },
            { true,  true,  true },
            { true,  true, true }
        }, _top, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { true, true, true },
            { true,  true,  true },
            { false,  false, true }
        }, _bottom, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { true, true, true },
            { true,  true,  true },
            { true,  false, false }
        }, _bottom, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { true, true, true },
            { true,  true,  false },
            { true,  true, false }
        }, _right, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { true, true, false },
            { true,  true,  false },
            { true,  true, true }
        }, _right, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { true, true, true },
            { false,  true,  true },
            { false,  true, true }
        }, _left, 0));

        _defs.Add(new Def(new bool[3, 3] {
            { false, true, true },
            { false,  true,  true },
            { true,  true, true }
        }, _left, 0));

        CreateAtlas();
    }

    public Vector2[] GetUvForTile(Tile[] neighbors)
    {
        var roadTile = TileLibrary.GetTile("Road");
        var signs = new bool[3, 3];

        var index = 0;
        for(int x = 0; x < 3; x++)
        {
            for(int y = 0; y < 3; y++)
            {
                if (x == 1 && y == 1)
                    continue;

                signs[x, y] = (neighbors[index].Def == roadTile);
                index++;
            }
        }

        foreach(var def in _defs)
        {
            if (def.signs[0, 0] == signs[0, 0] &&
                def.signs[0, 1] == signs[0, 1] &&
                def.signs[0, 2] == signs[0, 2] &&
                def.signs[1, 0] == signs[1, 0] &&
                def.signs[1, 2] == signs[1, 2] &&
                def.signs[2, 0] == signs[2, 0] &&
                def.signs[2, 1] == signs[2, 1] &&
                def.signs[2, 2] == signs[2, 2])
            {
                
                return Random.value > 0.8f ? GetUvInAtlas(Random.Range(def.atlasIndicesStart, def.atlasIndicesEnd)) : GetUvInAtlas(def.atlasIndicesStart);
            }
        }

        return null;
    }

    public void CreateAtlas()
    {
        List<Texture2D> textures = new List<Texture2D>();

        foreach(var def in _defs)
        {
            def.atlasIndicesStart = textures.Count;
            textures.AddRange(def.textures);
            def.atlasIndicesEnd = textures.Count;
        }

        var atlasSize = Mathf.CeilToInt(Mathf.Sqrt(textures.Count)) * 64;
        atlas = new Texture2D(atlasSize, atlasSize);
        atlas.filterMode = FilterMode.Point;

        var index = 0;

        for(int x = 0; x < atlasSize / 64; x++)
        {
            for(int y = 0; y < atlasSize / 64; y++)
            {
                atlas.SetPixels(x * 64, y * 64, 64, 64, textures[index].GetPixels());

                index++;

                if (index >= textures.Count)
                    break;
            }

            if (index >= textures.Count)
                break;
        }

        atlas.Apply();
    }

    Vector2[] GetUvInAtlas(int index)
    {
        var uv = new Vector2[4];
        float size = (atlas.width / 64);

        var y = index % (int)size;
        var x = Mathf.FloorToInt(index / (int)size);

        var div = 1f / size;

        var pad = 0.0001f;

        return new Vector2[4]
        {
            new Vector2(x * div + pad,        y * div + div - pad),
            new Vector2(x * div + pad,        y * div + pad),
            new Vector2(x * div + div - pad,  y * div + pad),
            new Vector2(x * div + div - pad,  y * div + div - pad)
        };
    }
}
