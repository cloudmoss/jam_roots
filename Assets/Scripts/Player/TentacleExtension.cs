using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleExtension : MonoBehaviour
{
    public string Name { get { return _name; } }

    [SerializeField] private string _name;

    protected Tentacle _tentacle;

    private void Awake() {
        _tentacle = transform.parent.GetComponent<Tentacle>();
    }

}
