using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnParticleCollisions : MonoBehaviour {
    public UnityEvent TestEvent;

    void OnParticleCollision(GameObject other)
    {
        DoSomething();
    }
    // Update is called once per frame
    void DoSomething() {
        gameObject.SetActive(false);
        TestEvent.Invoke();

    }
}
