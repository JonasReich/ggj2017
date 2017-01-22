using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIndication : MonoBehaviour {

	public GameObject RadioTower;
	public Sprite[] LevelPoints;

	private StationCapture station;
	private SpriteRenderer spriteRenderer;

	void Awake() {
		station = RadioTower.GetComponent<StationCapture>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		Debug.Log("start");
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void SetLevelIndication(int level) {
		Debug.Log(spriteRenderer);
		if (level == 0) {
			spriteRenderer.enabled = false;
		} else {
			spriteRenderer.enabled = true;
			spriteRenderer.sprite = LevelPoints[level-1];
		}
		Debug.Log("Level Indication is " + level);
	}

	public void SetLevelIndicationColor(Color color) {
		spriteRenderer.color = color;
	}

}
