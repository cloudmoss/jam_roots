using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class Tentacle : MonoBehaviour
{
    private static Tentacle _currentlySelected;

    public int length;

    [SerializeField] private Material _material;
    [SerializeField] private AnimationCurve _thicknessCurve;
    [SerializeField] private float _thickness;
    [SerializeField] private GameObject _handle;
    [SerializeField] private float _handleRadius = 0.5f;

    private CircleCollider2D _collider;
    private Entity _entity = new Entity("Tentacle", true);
    private Task<List<Tile>> _pathTask = null;
    private MeshFilter _filter;
    private MeshRenderer _renderer;
    private Vector2[] _currentVertices;
    private bool _grabbed = false;
    private Vector2Int _targetPos;
    private bool _recalculating = false;
    private Vector2Int _startPosition;
    private Vector2Int _endPosition;
    private Rigidbody2D _rb;

    private void Awake() 
    {
        _startPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        _endPosition = _startPosition;
        _targetPos = _endPosition;
        _filter = gameObject.AddComponent<MeshFilter>();
        _renderer = gameObject.AddComponent<MeshRenderer>();
        _renderer.material = _material;
        _collider = _handle.AddComponent<CircleCollider2D>();
        _collider.radius = _handleRadius;
        _rb = gameObject.AddComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
    }

    public void Move(Vector2Int position)
    {
        Debug.Log("Move to " + position);

        //_entity.ClearBlockers();
        _pathTask = Pathfinding.GetPathAsync(_startPosition, position, length);
        StartCoroutine(WaitForPath());
        _recalculating = true;
    }

    IEnumerator WaitForPath()
    {
        while (!_pathTask.IsCompleted)
        {
            yield return null;
        }

        var path = _pathTask.Result;
        Debug.Log(path.Count);
        var last = path[0].Position;
        _currentVertices = new Vector2[path.Count];

        for (int i = 0; i < path.Count; i++)
        {
            Debug.DrawLine(new Vector2(last.x, last.y), new Vector2(path[i].Position.x, path[i].Position.y), Color.yellow, 3);
            last = path[i].Position;
            _currentVertices[i] = new Vector2(path[i].Position.x, path[i].Position.y);
        }

        _pathTask.Dispose();
        _endPosition = path[path.Count - 1].Position;

        GenerateMesh();
        _recalculating = false;
        //_entity.SetOccupiedTiles(path.Skip(2).ToArray());
    }

    private void OnDestroy() {
        if (_pathTask != null)
        {
            _pathTask.Dispose();
        }
    }

    public void GenerateMesh()
    {
        if (_filter.mesh != null)
        {
            DestroyImmediate(_filter.mesh);
        }

        var vertices = new List<Vector3>();
        var uvs = new List<Vector2>();
        var tris = new List<int>();

        var mesh = new Mesh();
        var tpos = transform.position;

        var lastVert = new Vector3(_currentVertices[0].x, _currentVertices[0].y, 0);

        var connectingVert1 = lastVert;
        var connectingVert2 = lastVert;

        for (int i = 1; i < _currentVertices.Length; i++)
        {
            var curVert = new Vector3(_currentVertices[i].x, _currentVertices[i].y, 0);
            var fwd = (_currentVertices[i] - _currentVertices[i - 1]).normalized;
            var rightv2 = RotateVector(fwd, 90);
            var right = new Vector3(rightv2.x, rightv2.y, 0);

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));

            var thickness = _thicknessCurve.Evaluate((float)i / _currentVertices.Length) * _thickness;

            
            var vert1 = i == 1 ? lastVert + (right * thickness) : connectingVert1;
            var vert2 = i == 1 ? lastVert - (right * thickness) : connectingVert2;
            var vert3 = curVert + (right * thickness);
            var vert4 = curVert - (right * thickness);

            connectingVert1 = vert3;
            connectingVert2 = vert4;

            vertices.Add(vert1 - tpos);
            vertices.Add(vert2 - tpos);
            vertices.Add(vert3 - tpos);
            vertices.Add(vert4 - tpos);

            lastVert = curVert;
        }

        for(int i = 0; i < vertices.Count; i+=4)
        {
            tris.Add(i);
            tris.Add(i + 2);
            tris.Add(i + 1);

            tris.Add(i + 1);
            tris.Add(i + 2);
            tris.Add(i + 3);
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = tris.ToArray();

        _filter.mesh = mesh;
    }

    // Rotate a vector by an angle in degrees
    public static Vector2 RotateVector(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    private void Update() 
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        var dist = Vector3.Distance(mouseWorldPos, _handle.transform.position);

        //Debug.DrawLine(new Vector2(startPosition.x, startPosition.y), new Vector2(mouseWorldPos.x, mouseWorldPos.y), Color.red);

        if (_currentlySelected == null && dist < _handleRadius)
        {
            _handle.transform.localScale = Vector3.one + (Vector3.one * (0.25f * Mathf.Sin(Time.time * 4f)));

            if (Input.GetMouseButtonDown(0))
            {
                _grabbed = true;
                _currentlySelected = this;
            }
        }
        else
        {
            _handle.transform.localScale = Vector3.one;
        }

        if(_grabbed)
        {
            _targetPos = new Vector2Int(Mathf.RoundToInt(mouseWorldPos.x), Mathf.RoundToInt(mouseWorldPos.y));

            _handle.transform.position = mouseWorldPos;

            if (!Input.GetMouseButton(0))
            {
                _grabbed = false;
                _handle.transform.position = new Vector3(_targetPos.x, _targetPos.y, 0);
                _currentlySelected = null;
            }
        }

        if (_targetPos != _endPosition && !_recalculating)
        {
            Move(_targetPos);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent<EnemyBase>(out var enemy)) {
            enemy.Kill();
            Debug.Log("Killed enemy");
        }
    }
}
