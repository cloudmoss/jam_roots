using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorePile : MonoBehaviour
{
    public event System.Action OnConsumed;

    public int resourceCount = 10;

    public void Consume()
    {
        resourceCount--;
        
        transform.localScale = (Vector3.one * 0.2f) + (Vector3.one * 0.8f) * resourceCount / 10f;

        if (resourceCount <= 0)
        {
            OnConsumed?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        
    }
}
