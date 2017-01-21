using UnityEngine;

public class StationCapture : MonoBehaviour {

	public float CaptureTime = 3.0f;

	private bool[] inBounds;
	private float[] timeInBounds;
	private int playersInBounds;

    public int Level;

	private bool captured = false;
	// Player ID
	private int owner = -1;
	private bool playerCapturing = false;

	public GameObject GameManager;

	private GameManager game;
	private ParticleCollisions particles;


	// Use this for initialization
	void Start () {
		timeInBounds = new float[4];
		inBounds = new bool[4];
		particles = (ParticleCollisions) GetComponent<ParticleCollisions>();
		game = (GameManager) GameManager.GetComponent<GameManager>();
	}

	// Update is called once per frame
	void Update () {
		if (!playerCapturing) {
			return;
		}
        

        if (Level == 5)
            Debug.LogError("Winning!!!!!!!!!!!!");

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
		particles.SetStunColor(game.GetPlayerColor(playerId));
		particles.SetIndicationColor(game.GetPlayerColor(playerId));
		playerCapturing = false;

		Debug.Log("Captured by player " + playerId);
	}

	private void resetProgress() {
		for (int i = 0; i < 4; i++) {
			timeInBounds[i] = 0f;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		//if (collider.GetType() != typeof(BoxCollider2D))
		//	return;

		PlayerController pc = (PlayerController)
			collider.gameObject.GetComponent<PlayerController>();
		if (pc == null)
			return;

        inBounds[pc.GetId()] = true;
		playersInBounds++;
		if (playersInBounds == 1 && pc.GetId() != owner) {
			playerCapturing = true;
		} else if (playersInBounds != 0) {
			playerCapturing = false;
		}
		if (pc.GetId() != owner)
			particles.SetIndicationColor(Color.clear);

		Debug.Log("Entered by player " + pc.GetId());
	}

	void OnTriggerExit2D(Collider2D collider) {
		//if (collider.GetType() != typeof(BoxCollider2D))
		//	return;

		PlayerController pc = (PlayerController)
			collider.gameObject.GetComponent(typeof(PlayerController));
		if (pc == null)
			return;

		inBounds[pc.GetId()] = false;
		timeInBounds[pc.GetId()] = 0f;
		playersInBounds--;

		playerCapturing = false;
		if (playersInBounds == 1) {
			for (int i = 0; i < game.NumPlayers; i++) {
				if (inBounds[i] && owner != i)
					playerCapturing = true;
			}
		}
		if (playerCapturing == false) {
			if (!captured)
				particles.SetIndicationColor(Color.grey);
			else
				particles.SetIndicationColor(game.PlayerColor[owner]);
		}

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

	public void Reset() {
		owner = -1;
		captured = false;
		for (int i = 0; i < inBounds.Length; i++)
			inBounds[i] = false;
		particles.SetStunColor(Color.clear);
		particles.SetIndicationColor(Color.grey);

		playerCapturing = false;
	}

}
