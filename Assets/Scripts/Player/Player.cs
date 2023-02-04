using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{
    public static Player Current { get; private set; }
    
    public Tentacle[] tentacles { get; private set; }


    private SpriteRenderer _spriteRenderer;
    private Vector2 _position;
    private SphereCollider _collider;

    private void Awake() {
        _collider = gameObject.AddComponent<SphereCollider>();
        _collider.radius = 1.5f;

        _position = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Current = this;

        tentacles = GetComponentsInChildren<Tentacle>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Physics.Raycast(mousePos, Vector3.forward, out RaycastHit hitInfo, 1000f);
            if (hitInfo.collider != null)
            {
                var tentacle = hitInfo.collider.GetComponent<Entity>();

                if (tentacle != null)
                {
                    UnitInspectorUI.Current.Open(tentacle);
                }
            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                    UnitInspectorUI.Current.Close();
            }
        }
    }

    public bool TestHit(Vector2 position)
    {
        return (Vector2.Distance(position, _position) < 1.5f);
    }
}
