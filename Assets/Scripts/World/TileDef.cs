using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileDef
{
    public string Name { get { return _name; } }
    public bool IsWalkable { get { return _isWalkable; } }
    public Texture2D Texture { get { return _texture; } }
    
    [SerializeField] private string _name;
    [SerializeField] private bool _isWalkable;
    [SerializeField] private Texture2D _texture;

    public void OverrideTexture(Texture2D texture)
    {
        _texture = texture;
    }
}
