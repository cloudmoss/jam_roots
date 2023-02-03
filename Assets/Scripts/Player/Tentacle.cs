using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Tentacle : MonoBehaviour
{
    public Vector2Int startPosition;
    public int length;

    private Task<List<Tile>> _pathTask = null;
    private MeshFilter _filter;
    private MeshRenderer _renderer;
    private Vector2[] _currentVertices;

    private void Awake() {
        _filter = gameObject.AddComponent<MeshFilter>();
        _renderer = gameObject.AddComponent<MeshRenderer>();
    }

    public void Move(Vector2Int position)
    {
        Debug.Log("Move to " + position);

        _pathTask = Pathfinding.GetPathAsync(startPosition, position);
        StartCoroutine(WaitForPath());
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
            Debug.DrawLine(new Vector2(last.x, last.y), new Vector2(path[i].Position.x, path[i].Position.y), Color.yellow, 60);
            last = path[i].Position;
            _currentVertices[i] = new Vector2(path[i].Position.x, path[i].Position.y);
        }

        _pathTask.Dispose();
    }

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Move(new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y)));
        }
    }

    private void OnDestroy() {
        if (_pathTask != null)
        {
            _pathTask.Dispose();
        }
    }

    public void GenerateMesh()
    {

    }
}
