using UnityEngine;

public class ParticleCollisions : MonoBehaviour {
    //public UnityEvent TestEvent;

	public float StunWaveDelay = 3f;
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

    void OnParticleCollision(GameObject other)
    {
		if (other == this.gameObject)
			return;

		PlayerController pc = (PlayerController)
				other.GetComponent<PlayerController>();
		if (pc == null)
			return;

		if (station.GetOwner() != pc.GetId())
			pc.Stun(station.GetOwner());
    }

	public void SetStunColor(Color color) {
		var main = stunParticles.main;
		main.startColor = color;
	}

	public void SetIndicationColor(Color color) {
		var main = ownerParticles.main;
		main.startColor = color;
	}

}
