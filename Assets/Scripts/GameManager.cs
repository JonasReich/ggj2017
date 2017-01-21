using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public int NumPlayers;

    [SerializeField]
    private PlayerController[] PlayerArray;
    [SerializeField]
    private Sprite[] PlayerSprites;
    [SerializeField]
    private PlayerController PlayerPrefab;

	public Color[] PlayerColor;

	// Use this for initialization
	void Awake () {
		//Input.GetJoystickNames().Length
        PlayerArray = new PlayerController[NumPlayers];
		//PlayerColor = new Color[NumPlayers];
		string[] sJoysticks = Input.GetJoystickNames();

        for (int i = 0; i < NumPlayers; i++) {
			Debug.Log("Created Player " + i);
			PlayerController PlayerTMP =
					Instantiate(PlayerPrefab, GameObject.Find("Players").transform);
			PlayerTMP.Initialize(i);
			PlayerTMP.name = "Player" + i;
			PlayerTMP.SetGameManager(this);
			PlayerTMP.SetSprite(PlayerSprites[i]);
			//PlayerTMP.SetId(i);
			PlayerTMP.SetPosition(new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f)));
			PlayerArray[i] = PlayerTMP;

			/*
			switch (i) {
				case 0: PlayerColor[i] = Color.blue; break;
				case 1: PlayerColor[i] = Color.green; break;
				case 2: PlayerColor[i] = Color.red; break;
				case 3: PlayerColor[i] = Color.yellow; break;
			}
			*/
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

	public Color GetPlayerColor(int id) {
		return PlayerColor[id];
	}

    public Vector2 GetPlayerPos(int PlayerID)
    {
        return PlayerArray[PlayerID].transform.position;
    }

}
