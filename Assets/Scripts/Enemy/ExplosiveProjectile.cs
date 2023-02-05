using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    [SerializeField] private AudioClip _sfx;

    private void OnDestroy() {
        AudioController.PlaySfx(_sfx, transform.position, 0.5f);
    }
}
