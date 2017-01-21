using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public bool OnlyUseJoysticks = true;
	public int NumPlayers = 2;
	public Color[] PlayerColor;
	public float RoundTime = 30f;

    [SerializeField]
    private PlayerController[] PlayerArray;
    [SerializeField]
    private Sprite[] PlayerSprites;
    [SerializeField]
    private PlayerController PlayerPrefab;

	private float countDown;
	private bool gameFinished = false;

	void Awake () {
        if (OnlyUseJoysticks) {
            NumPlayers = 0;
            foreach (string s in Input.GetJoystickNames())
            {
                Debug.Log(s);
                if (s != "")
                    NumPlayers++;
            }
        }

        PlayerArray = new PlayerController[NumPlayers];

        for (int i = 0; i < NumPlayers; i++) {
                Debug.Log("Created Player " + i);
                PlayerController PlayerTMP =
                        Instantiate(PlayerPrefab, GameObject.Find("Players").transform);
                PlayerTMP.Initialize(i);
                PlayerTMP.name = "Player" + i;
                PlayerTMP.SetGameManager(this);
                PlayerTMP.SetSprite(PlayerSprites[i]);
                //PlayerTMP.SetPosition(new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f)));
                PlayerArray[i] = PlayerTMP;

        }
		
		countDown = RoundTime;
    }

	void OnGUI() {
		if (gameFinished) {
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 400, 200), "End");
			if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 20, 400, 200), "Restart")) {
				gameFinished = false;
				countDown = RoundTime;
				unstunPlayers();
			}
		}

		int minutes = (int) (countDown / 60f);
		int seconds = ((int) countDown) - minutes * 60;
		string label;
		if (seconds < 10)
			label = minutes + ":0" + seconds;
		else
			label = minutes + ":" + seconds;
		GUI.Label(new Rect(0, 0, 400, 200), label);
	}

	void Update () {
		if (gameFinished) {
			stunPlayers();
			return;
		}

		countDown -= Time.deltaTime;
		if (countDown <= 0f) {
			stunPlayers();
			gameFinished = true;
		}
	}

	void stunPlayers() {
		foreach (PlayerController pc in PlayerArray) {
			pc.Stun(pc.GetId());
		}
	}
	
	void unstunPlayers() {
		foreach (PlayerController pc in PlayerArray) {
			pc.Reset();
		}
	}

	public Color GetPlayerColor(int id) {
		return PlayerColor[id];
	}

    public Vector2 GetPlayerPos(int PlayerID)
    {
        return PlayerArray[PlayerID].transform.position;
    }

}
