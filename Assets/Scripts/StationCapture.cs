using UnityEngine;

public class StationCapture : MonoBehaviour {

	public float CaptureTime = 3.0f;

	private bool[] inBounds;
	private float[] timeInBounds;
	private int playersInBounds;

	// Use this for initialization
	void Start () {
		timeInBounds = new float[4];
		inBounds = new bool[4];
	}

	// Update is called once per frame
	void Update () {
		if (playersInBounds != 1)
			return;

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
		Debug.Log("Captured by player " + playerId);
	}

	private void resetProgress() {
		for (int i = 0; i < 4; i++) {
			timeInBounds[i] = 0f;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log("lel");
		PlayerController pc = (PlayerController)
			collider.gameObject.GetComponent<PlayerController>();
		if (pc == null)
			return;
		inBounds[pc.GetId()] = true;
		playersInBounds++;
		Debug.Log("Entered by player " + pc.GetId());
	}

	void OnTriggerExit2D(Collider2D collider) {
		PlayerController pc = (PlayerController)
			collider.gameObject.GetComponent(typeof(PlayerController));
		if (pc == null)
			return;
		inBounds[pc.GetId()] = false;
		timeInBounds[pc.GetId()] = 0f;
		playersInBounds--;
		Debug.Log("Exited by player " + pc.GetId());
	}
}
