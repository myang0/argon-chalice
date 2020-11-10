using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    private ParticleSystem _ps;

    void Start() {
        _ps = GetComponent<ParticleSystem>();
        
        float destroyTime = _ps.duration + _ps.startLifetime;
        Destroy(gameObject, destroyTime);
    }
}
