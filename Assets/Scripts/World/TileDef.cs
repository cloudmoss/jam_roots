using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileDef
{
    public string Name { get { return _name; } }
    public bool IsWalkable { get { return _isWalkable; } }
    
    [SerializeField] private string _name;
    [SerializeField] private bool _isWalkable;
}
