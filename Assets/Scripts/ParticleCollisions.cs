using UnityEngine;

public class ParticleCollisions : MonoBehaviour {
    //public UnityEvent TestEvent;

	public float StunWaveDelay = 3f;

	private ParticleSystem stunParticles, ownerParticles;
	private StationCapture station;
	private float delay;

	void Awake() {
		station = GetComponent<StationCapture>();
		stunParticles =
			transform.Find("StunWave").GetComponent<ParticleSystem>();
		ownerParticles =
			transform.Find("OwnerIndication").GetComponent<ParticleSystem>();
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
		PlayerController pc = (PlayerController)
				other.GetComponent<PlayerController>();
		if (pc == null)
			return;

		if (station.GetOwner() != pc.GetId())
			pc.Stun(station.GetOwner());
    }

	public void SetColor(Color color) {
		var main = stunParticles.main;
		main.startColor = color;
	}

}
