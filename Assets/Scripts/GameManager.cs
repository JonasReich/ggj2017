using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private PlayerController[] PlayerArray;
    [SerializeField]
    private PlayerController PlayerPrefab;

	// Use this for initialization
	void Awake () {
        PlayerArray = new PlayerController[Input.GetJoystickNames().Length];

        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            PlayerController PlayerTMP = Instantiate(PlayerPrefab, GameObject.Find("Players").transform);
            PlayerTMP.name = "Player" + i;
            PlayerTMP.SetId(i);
            PlayerArray[i] = PlayerTMP;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
