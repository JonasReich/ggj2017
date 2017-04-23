using UnityEngine;

public class StationCapture : MonoBehaviour {

	public float CaptureTime = 3.0f;

	public bool[] inBounds;
	public float[] timeInBounds;
	public int playersInBounds;

    public int Level;

	public GameObject LevelIndicator;

	public bool captured = false;
	// Player ID
	public int owner = -1;
	private bool playerCapturing = false;

	private GameManager game;
	private ParticleCollisions particles;
	private LevelIndication levelIndication;

    // Use this for initialization
    void Start () {
		timeInBounds = new float[4];
		inBounds = new bool[4];
        
		particles = (ParticleCollisions) GetComponent<ParticleCollisions>();
		game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		levelIndication = LevelIndicator.GetComponent<LevelIndication>();
		levelIndication.SetLevelIndication(0);
	}

	// Update is called once per frame
	void Update () {


        if(game.gameFinished)
        {
            owner = -1;
            Level = 0;
            particles.DecStunWaveDelay();
            if (!IsInvoking())
                Invoke("Reset", 3.0f);

            return;
        }
        int x = 0;
        for (int i = 0; i < inBounds.Length; i++)
        {
            
            if (inBounds[i])
                x++;
        }
        playersInBounds = x;

        if (!playerCapturing) {
            if (!captured)
            {
                owner = -1;
                Level = 0;
                particles.DecStunWaveDelay();
            } 
			return;
		}
        

		for (int i = 0; i < 4; i++) {
			if (inBounds[i]) {
				timeInBounds[i] += Time.deltaTime;
				//sDebug.Log("Progress for player " + i + ": " + timeInBounds[i]);
				if (timeInBounds[i] >= CaptureTime) {
					captureStation(i);
					resetProgress(i);
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
        particles.DecStunWaveDelay();
        playerCapturing = false;
		levelIndication.SetLevelIndicationColor(game.GetPlayerColor(playerId));
        Level = 0;
		IncLvl(1);

		//Debug.Log("Captured by player " + playerId);
	}

	private void resetProgress(int i) {
		
			timeInBounds[i] = 0f;

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
		} else if (playersInBounds != 1) {
			playerCapturing = false;
		}
		if (pc.GetId() != owner)
			particles.SetIndicationColor(Color.clear);

		//Debug.Log("Entered by player " + pc.GetId());
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

		//Debug.Log("Exited by player " + pc.GetId());
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
        Level = 0;
		particles.SetStunColor(Color.clear);
		particles.SetIndicationColor(Color.grey);
		playerCapturing = false;
        playersInBounds = 0;
		levelIndication.SetLevelIndication(0);
	}


    public void IncLvl(int i)
    {
        if(captured)
        {
            Level += i;
			levelIndication.SetLevelIndication(Level);
            GetComponent<AudioSource>().Play();
            GetComponentInChildren<StunCollisions>().NotSetAlready = true;
        }
            

    }

}
