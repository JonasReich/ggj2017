using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunCollisions : MonoBehaviour {

	public GameObject RadioStation;
	private StationCapture station;
    private ParticleCollisions ParticleCollision;
    bool hit = false;
    float timer = 0;

    private GameManager game;

    // Use this for initialization
    void Start () {
		station = RadioStation.GetComponent<StationCapture>();
        ParticleCollision = station.GetComponent<ParticleCollisions>();
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (game.gameFinished)
            return;

        timer += Time.deltaTime;
        if (timer >= 30)
        {
            if (hit == false)
            {
                station.IncLvl(1);
                ParticleCollision.DecStunWaveDelay(station.Level / 2);
            }
            timer = 0;
        }
		
	}

    void OnParticleCollision(GameObject other) {
		if (other == RadioStation || game.gameFinished)
			return;

		PlayerController pc = (PlayerController)
				other.GetComponent<PlayerController>();
		if (pc == null)
			return;

		//Debug.Log("inhere");

		if (station.GetOwner() != pc.GetId() && !hit)
        {
            if(hit == false) { 
            station.IncLvl(1);
            ParticleCollision.DecStunWaveDelay(station.Level / 2);
            hit = true;
            }
            
            if (!IsInvoking())
                Invoke("ResetHit", 2);
        }
			
    }

    void ResetHit()
    {
        hit = false;
    }

}
