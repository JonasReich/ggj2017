﻿using UnityEngine;

public class ParticleCollisions : MonoBehaviour {
    //public UnityEvent TestEvent;
    [SerializeField]
	private float StunWaveDelay = 5f;
	public GameObject StunParticles, OwnerParticles;

	private ParticleSystem stunParticles, ownerParticles;
	private StationCapture station;
	private float delay;

	void Awake() {
		station = GetComponent<StationCapture>();
		stunParticles =
			StunParticles.GetComponent<ParticleSystem>();
		ownerParticles =
			OwnerParticles.GetComponent<ParticleSystem>();
	}

	void Update() {
		if (!station.IsCaptured() || station.PlayerCapturing()) {
			delay = 0f;
			return;
		}

		delay += Time.deltaTime;
		if (delay >= StunWaveDelay) {
			stunParticles.Clear();
			stunParticles.Play();
			delay -= StunWaveDelay;
		}
	}

	public void SetStunColor(Color color) {
		var main = stunParticles.main;
		main.startColor = color;
	}

	public void SetIndicationColor(Color color) {
		var main = ownerParticles.main;
		main.startColor = color;
	}

    public void DecStunWaveDelay(float delay)
    {
        StunWaveDelay -= delay;
    }
    public void DecStunWaveDelay()
    {
        StunWaveDelay = 5f;
    }
}
