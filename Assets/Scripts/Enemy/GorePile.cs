using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorePile : MonoBehaviour
{
    public event System.Action OnConsumed;

    [SerializeField] private AnimationCurve _popInAnim;

    public int resourceCount = 10;
    
    private void Awake() {
        EnemyController.Current.RegisterGore(this);
    }

    IEnumerator Start() {
        var t = 0f;
        var startScale = Vector3.one * 0.25f;

        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            var e = _popInAnim.Evaluate(t);
            transform.localScale = Vector3.Lerp(startScale, Vector3.one, e);
            yield return null;
        }
    }

    public void Consume()
    {
        resourceCount--;
        ResourceControl.Current.AddResources("Biomass", 1);
        
        transform.localScale = (Vector3.one * 0.2f) + (Vector3.one * 0.8f) * resourceCount / 10f;

        if (resourceCount <= 0)
        {
            OnConsumed?.Invoke();
            Destroy(gameObject);
        }
    }

}
