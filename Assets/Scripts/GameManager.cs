using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public int NumPlayers;

    [SerializeField]
    private PlayerController[] PlayerArray;
    [SerializeField]
    private PlayerController PlayerPrefab;
	[SerializeField]
	private Color[] PlayerColor;

	// Use this for initialization
	void Awake () {
		//Input.GetJoystickNames().Length
        PlayerArray = new PlayerController[NumPlayers];
		PlayerColor = new Color[NumPlayers];

        for (int i = 0; i < NumPlayers; i++)
        {
			/*
            PlayerController PlayerTMP = Instantiate(PlayerPrefab, GameObject.Find("Players").transform);
            PlayerTMP.name = "Player" + i;
            PlayerTMP.SetId(i);
            PlayerArray[i] = PlayerTMP;
			*/

			switch (i) {
				case 0: PlayerColor[i] = Color.blue; break;
				case 1: PlayerColor[i] = Color.green; break;
				case 2: PlayerColor[i] = Color.red; break;
				case 3: PlayerColor[i] = Color.yellow; break;
			}
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public Color GetPlayerColor(int id) {
		return PlayerColor[id];
	}
}
