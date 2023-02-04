using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleExtension : MonoBehaviour
{
    public string Name { get { return _name; } }
    public string Description { get { return _description; } }
    public ResourceClass[] Cost { get { return _cost; } }

    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private ResourceClass[] _cost;

    protected Tentacle _tentacle;

    private void Start() {
        _tentacle = transform.parent.GetComponent<Tentacle>();
    }

}
