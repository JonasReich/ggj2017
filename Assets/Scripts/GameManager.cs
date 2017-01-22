using UnityEngine;
using NONE;

public class GameManager : MonoBehaviour {

	public bool OnlyUseJoysticks = true;
	public int NumPlayers = 2;
	public Color[] PlayerColor;
	public float RoundTime = 30f;
	public TextureReader Level;
    public Sprite Panda;
    public Sprite Mouse;

	public GUIStyle style, winlabelstyle, winbuttonstyle;

    [SerializeField]
    private PlayerController[] PlayerArray;
    [SerializeField]
    private PlayerController PlayerPrefab;

	private float countDown = 1;
	public bool gameFinished = false;
    public bool CharSelection = false;

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
                //Debug.Log("Created Player " + i);
                PlayerController PlayerTMP =
                        Instantiate(PlayerPrefab, GameObject.Find("Players").transform);
                PlayerTMP.name = "Player" + i;
                PlayerTMP.SetGameManager(this);
				PlayerTMP.SetLevel(Level);
                if (i < 2)
                PlayerTMP.SetPosition(new Vector2(1+i,2+i));
                if (i==2)
                PlayerTMP.SetPosition(new Vector2(i,i));
                if (i == 3)
                PlayerTMP.SetPosition(new Vector2(1, i));
                PlayerArray[i] = PlayerTMP;
                PlayerTMP.Initialize(i);
        }
		
		countDown = RoundTime;
    }

	private string getWinningPlayerColor() {
		//todo unentschieden

		int[] stations = new int[NumPlayers];
		foreach (StationCapture station in Level.GetComponentsInChildren(typeof(StationCapture))) {
			if (station.GetOwner() != -1)
				stations[station.GetOwner()]++;
		}
        bool NoWinner = false;
		int winner = 0, maxPoints = 0;
		for (int i = 0; i < NumPlayers; i++) {
			if (stations[i] > maxPoints) {
				maxPoints = stations[i];
				winner = i;
                NoWinner = false;
			}
            else if(stations[i] == maxPoints)
            {
                NoWinner = true;
            }
		}

        if(NoWinner)
        {
            return "No";
        }
        else
        {
            switch (winner)
            {
                case 0: return "Red";
                case 1: return "Green";
                case 2: return "Blue";
                case 3: return "Yellow";
                default: return "No";
            }

		}
	}

	void OnGUI() {
		if (gameFinished) {
			string win = getWinningPlayerColor() + " Player Won!";
			GUI.Label(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 75, 250, 50),
					win, winlabelstyle);
			if (GUI.Button(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 25, 250, 100),
					"> Restart <", winbuttonstyle)) {
				gameFinished = false;
				countDown = RoundTime;
				resetStations();
				resetPlayers();
			}
		}
        if(CharSelection)
        {
            float offset = 375;
            for(int i = 0; i < NumPlayers; i++)
            {
                string PlayerChoose = "Player " + i + ":\n Choose your Charakter";
                GUI.Label(new Rect(Screen.width - offset, Screen.height / 2 - 75, 350, 250),
                        PlayerChoose, winlabelstyle);

                if (GUI.Button(new Rect(Screen.width - offset, Screen.height / 2 + 100, 275, 100), "Propaganda Panda", winbuttonstyle))
                {
                    PlayerArray[i].SetSprite(Panda);
                    CharSelection = false;
                    break;
                }
                if (GUI.Button(new Rect(Screen.width - offset, Screen.height / 2 + 200, 275, 100), "Mouse sullini", winbuttonstyle))
                {
                    PlayerArray[i].SetSprite(Mouse);
                    CharSelection = false;
                    break;
                }
                offset += 375;

            }

                
        }

		int minutes = (int) (countDown / 60f);
		int seconds = ((int) countDown) - minutes * 60;
		string label;
		if (seconds < 10)
			label = minutes + ":0" + seconds;
		else
			label = minutes + ":" + seconds;
		GUI.Label(new Rect(Screen.width - 80, 10, 50, 30), label, style);
	}

	void Update () {
		if (gameFinished) {
			stunPlayers();
			return;
		}

        if(countDown == RoundTime)
        {
            CharSelection = true;
            stunPlayers();
            return;
        }

		countDown -= Time.deltaTime;
		if (countDown <= 0f) {
			stunPlayers();
			gameFinished = true;
		}
	}

    public void SetGameFinished(bool finished)
    {
        gameFinished = finished;
    }

	private void resetPlayers() {
		foreach (PlayerController pc in PlayerArray) {
			pc.Reset();
			pc.MoveToSpawn();
		}
	}

	private void resetStations() {
		foreach (StationCapture station in Level.GetComponentsInChildren(typeof(StationCapture))) {
			station.Reset();
		}
	}

	void stunPlayers() {
		foreach (PlayerController pc in PlayerArray) {
			pc.Stun(pc.GetId());
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
