using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIndication : MonoBehaviour {

	public Sprite[] LevelPoints;

	private SpriteRenderer spriteRenderer;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
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
	}

	public void SetLevelIndicationColor(Color color) {
		spriteRenderer.color = color;
	}

}
