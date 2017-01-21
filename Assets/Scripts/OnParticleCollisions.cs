using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnParticleCollisions : MonoBehaviour {
    //public UnityEvent TestEvent;

	public float EmissionDelay = 3f;

	private ParticleSystem particles;
	private StationCapture station;
	private float delay;

	void Awake() {
		particles = GetComponent<ParticleSystem>();
		station = GetComponent<StationCapture>();
	}

	void Update() {
		if (!station.IsCaptured() || station.PlayerCapturing()) {
			delay = 0f;
			return;
		}

		delay += Time.deltaTime;
		if (delay >= EmissionDelay) {
			particles.Emit(450);
			delay -= EmissionDelay;
		}
	}

    void OnParticleCollision(GameObject other)
    {
		PlayerController pc = (PlayerController)
				other.GetComponent<PlayerController>();
		if (pc == null)
			return;

		if (station.GetOwner() != pc.GetId())
			pc.Stun();
		
        //particles.animation.gameObject.SetActive(false);
        //TestEvent.Invoke();
    }

	public void SetColor(Color color) {
		var main = particles.main;
		main.startColor = color;
	}

}
