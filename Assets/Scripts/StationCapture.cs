using UnityEngine;

public class StationCapture : MonoBehaviour {

	public float CaptureTime = 3.0f;

	private bool[] inBounds;
	private float[] timeInBounds;
	private int playersInBounds;

	private bool captured = false;
	// Player ID
	private int owner;
	private bool playerCapturing = false;

	public GameObject GameManager;
	private ParticleSystem particles;
	private GameManager game;
	private OnParticleCollisions part;

	// Use this for initialization
	void Start () {
		timeInBounds = new float[4];
		inBounds = new bool[4];
		particles = (ParticleSystem) GetComponent<ParticleSystem>();
		part = (OnParticleCollisions) GetComponent<OnParticleCollisions>();
		//game = (GameManager) GetComponent<GameManager>();
		game = (GameManager) GameManager.GetComponent<GameManager>();
	}

	// Update is called once per frame
	void Update () {
		if (!playerCapturing) {
			//Debug.Log("stop");
			return;
		}

		for (int i = 0; i < 4; i++) {
			if (inBounds[i]) {
				timeInBounds[i] += Time.deltaTime;
				Debug.Log("Progress for player " + i + ": " + timeInBounds[i]);
				if (timeInBounds[i] >= CaptureTime) {
					captureStation(i);
					resetProgress();
				}
			}
		}
	}

	private void captureStation(int playerId) {
		inBounds[playerId] = false;
		captured = true;
		owner = playerId;
		part.SetColor(game.GetPlayerColor(playerId));
		playerCapturing = false;

		Debug.Log("Captured by player " + playerId);
	}

	private void resetProgress() {
		for (int i = 0; i < 4; i++) {
			timeInBounds[i] = 0f;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.GetType() != typeof(BoxCollider2D))
			return;
		PlayerController pc = (PlayerController)
			collider.gameObject.GetComponent<PlayerController>();
		if (pc == null)
			return;
		inBounds[pc.GetId()] = true;
		playersInBounds++;
		playerCapturing = (playersInBounds == 1);
		Debug.Log("inbounds: " + playersInBounds);
		Debug.Log("Entered by player " + pc.GetId());
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.GetType() != typeof(BoxCollider2D))
			return;
		PlayerController pc = (PlayerController)
			collider.gameObject.GetComponent(typeof(PlayerController));
		if (pc == null)
			return;
		inBounds[pc.GetId()] = false;
		timeInBounds[pc.GetId()] = 0f;
		playersInBounds--;
		playerCapturing = (playersInBounds == 1);
		Debug.Log("inbounds: " + playersInBounds);
		Debug.Log("Exited by player " + pc.GetId());
	}

	public bool PlayerCapturing() {
		return playerCapturing;
	}

	public bool IsCaptured() {
		return captured;
	}

	public int GetOwner() {
		return owner;
	}

}
