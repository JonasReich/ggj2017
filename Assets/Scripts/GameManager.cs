using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public bool OnlyUseJoysticks = true;
	public int NumPlayers = 2;
	public Color[] PlayerColor;

    [SerializeField]
    private PlayerController[] PlayerArray;
    [SerializeField]
    private Sprite[] PlayerSprites;
    [SerializeField]
    private PlayerController PlayerPrefab;


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
    }

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
