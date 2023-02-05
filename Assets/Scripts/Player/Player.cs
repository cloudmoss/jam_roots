using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class Player : Entity
{
    public static Player Current { get; private set; }
    public static TentacleExtension[] Extensions { get; private set; }

    public List<Tentacle> tentacles { get; private set; } = new List<Tentacle>();

    [SerializeField] private Sprite[] _stages;
    [SerializeField] private Sprite _growIcon;

    private int level = 1;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _position;
    private SphereCollider _collider;
    private Tentacle _tentacleTemplate;


    private void Awake() {
        _collider = gameObject.AddComponent<SphereCollider>();
        _collider.radius = 1.5f;

        _position = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Current = this;

        _tentacleTemplate = GetComponentInChildren<Tentacle>();
        _tentacleTemplate.gameObject.SetActive(false);

        Extensions = Resources.LoadAll<TentacleExtension>("TentacleExtensions");

        Debug.Log("Tentacle extensions: " + Extensions.Length.ToString());

        OnDamageTaken += OnDamageEffects;
    }

    public Vector2Int GetRandomPlayerPos()
    {
        var bulbChance = 1f / tentacles.Count;

        if (Random.value > bulbChance && tentacles.Count > 0)
        {
            return tentacles[Random.Range(0, tentacles.Count)].EndPosition.ToVector2Int();
        }
        else
        {
            return _position.ToVector2Int();
        }
    }

    void OnDamageEffects(float damage)
    {
        CameraController.Current.ScreenShake.Shake(Mathf.Min(damage / 20f, 1f), Mathf.Min(damage / 20f, 1f) * 0.15f, 3f);
    }

    public override ActionButton.Definition[] GetActionButtons()
    {
        if (level >= _stages.Length) return null;

        var buttons = new List<ActionButton.Definition>();
        var cost = new ResourceClass[] { new ResourceClass("Biomass", level * 250) };

        buttons.Add(new ActionButton.Definition()
        {
            sprite = _growIcon,
            title = "Grow",
            resourceList = cost,
            description = "Grow in size, doubling your health pool. Gain an additional tentacle.",
            onClick = () => {
                if (ResourceControl.Current.TrySpend(cost) == false)
                    return;
                    
                var newHealth = MaxHealth * 2;
                SetHealth(newHealth, newHealth);
                level++;
                var tentacle = CreateTentacle(transform.position, Extensions.FirstOrDefault(x => x.Name == "Club"));
                tentacle.Move(((Vector2)transform.position + Vector2.down * 3).ToVector2Int());
                UIController.Current.UpdateHealthText();
                _spriteRenderer.sprite = _stages[level - 1];
                UnitInspectorUI.Current.Open(this);
            }
        });


        return buttons.ToArray();
    }

    public void Heal(float amount)
    {
        var tentaclePortion = amount / tentacles.Count;

        SetHealth(Health + amount);

        foreach (var tentacle in tentacles)
        {
            tentacle.SetHealth(tentacle.Health + tentaclePortion);
        }
    }

    public Tentacle CreateTentacle(Vector2 position, TentacleExtension extension = null)
    {
        var tentacle = Instantiate(_tentacleTemplate, transform);
        tentacle.transform.parent = transform;
        tentacle.transform.position = position;
        tentacle.gameObject.SetActive(true);
        tentacle.OnDamageTaken += (float dmg) => { OnDamageEffects(dmg / 1.5f); };

        tentacle.Init();
        tentacles.Add(tentacle);

        if (extension != null)
            tentacle.AddExtension(extension);

        return tentacle;
    }

    IEnumerator Start() {

        while(!World.Current.IsGenerated) {
            yield return null;
        }

        CreateTentacle(transform.position, Extensions.FirstOrDefault(x => x.Name == "Club"));
        CreateTentacle(transform.position, Extensions.FirstOrDefault(x => x.Name == "Sucker"));

        var dir = Vector2.up;
        var i = 0;
        var dist = 3;
        var a = (360 / tentacles.Count);

        foreach (var tentacle in tentacles)
        {
            var pos = ((Vector2)transform.position + (dir.RotateVector(i * a) * dist)).ToVector2Int();
            tentacle.Move(pos);
            i++;
        }
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            DealDamage(10000);
        }
    }

    public bool TestHit(Vector2 position)
    {
        return (Vector2.Distance(position, _position) < 1.5f);
    }
}
