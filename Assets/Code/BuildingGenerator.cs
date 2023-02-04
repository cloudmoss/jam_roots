using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingGenerator : MonoBehaviour
{
    public Sprite[] allTiles;
    private List<Sprite> groundTiles;
    private List<Sprite> wallTiles;
    private List<Sprite> roofBottomTiles;
    private List<Sprite> roofTopTiles;
    private List<Sprite> roofMiddleTiles;

    private void Start()
    {
        groundTiles = FindAllTiles("ground");
        wallTiles = FindAllTiles("wall");
        roofBottomTiles = FindAllTiles("roof_bottom");
        roofTopTiles = FindAllTiles("roof_top");
        roofMiddleTiles = FindAllTiles("roof_middle");
    }

    public void Build(int width, int height, Vector3 startPos)
    {
        int wallHeight = 2;
        if (height > 4)
        {
            wallHeight = Random.Range(1, height - 2);
        }
        int roofHeight = height - wallHeight;
        Vector3 tilePos = Vector3.zero;
        bool doorExits = false;

        GameObject container = new GameObject();
        container.transform.position = startPos;
        container.name = "container";

        for (int i = 0; i <= height; i++)
        {
            for (int j = 0; j <= width; j++)
            {
                GameObject tile = new GameObject();
                tile.transform.parent = container.transform;
                tile.name = startPos.x + " " + startPos.y;
                tile.transform.position = tilePos;
                SpriteRenderer tileSprite = tile.AddComponent<SpriteRenderer>();

                if (i == 0)
                {
                    tileSprite.sprite = GetCorrectTile(groundTiles, j);
                    if (IsDoor(tileSprite.sprite))
                    {
                        doorExits = true;
                    }

                    if (!doorExits && j == width - 1)
                    {
                        tileSprite.sprite = FindTileByName(groundTiles, "door");
                    }
                } else if (i > 0 && i < wallHeight && wallHeight > 1)
                {
                    tileSprite.sprite = GetCorrectTile(wallTiles, j);
                } else if (i == wallHeight)
                {
                    tileSprite.sprite = GetCorrectTile(roofBottomTiles, j);
                } else if (i == height)
                {
                    tileSprite.sprite = GetCorrectTile(roofTopTiles, j);
                } else
                {
                    tileSprite.sprite = GetCorrectTile(roofMiddleTiles, j);
                }

                tilePos.x += 1;
            }
            tilePos.x = 0;
            tilePos.y += 1;
        }

        Sprite GetCorrectTile(List<Sprite> list, int j)
        {
            if(j == 0)
            {
                return FindTileByName(list, "left");
            } else if (j == width)
            {
                return FindTileByName(list, "right");
            } else
            {
                return RandomTile(list);
            }
        }
    }

    public List<Sprite> FindAllTiles(string nameContains)
    {
        List<Sprite> tileList = new List<Sprite>();
        foreach(Sprite s in allTiles)
        {
            if (s.name.Contains(nameContains))
            {
                tileList.Add(s);
            }
        }

        return tileList;
    }

    public Sprite FindTileByName(List<Sprite> list, string name)
    {
        foreach(Sprite s in list)
        {
            if (s.name.Contains(name))
            {
                return s;
            }
        }
        return null;
    }

    public Sprite RandomTile(List<Sprite> list)
    {
        int randomSprite = Random.Range(0, list.Count());
        while(list[randomSprite].name.Contains("right") || list[randomSprite].name.Contains("left"))
        {
            randomSprite = Random.Range(0, list.Count());
        }
        return list[randomSprite];
    }

    public bool IsDoor(Sprite s)
    {
        return s.name.Contains("door");
    }
}