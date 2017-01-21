using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunCollisions : MonoBehaviour {

	public GameObject RadioStation;
	private StationCapture station;
    bool hit = false;

    // Use this for initialization
    void Start () {
		station = RadioStation.GetComponent<StationCapture>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnParticleCollision(GameObject other) {
		if (other == RadioStation)
			return;

		PlayerController pc = (PlayerController)
				other.GetComponent<PlayerController>();
		if (pc == null)
			return;

		Debug.Log("inhere");

		if (station.GetOwner() != pc.GetId() && !hit)
        {
            station.GetComponent<StationCapture>().Level++;
            station.GetComponent<ParticleCollisions>().StunWaveDelay -= station.GetComponent<StationCapture>().Level++;
            hit = true;
            if (!IsInvoking())
                Invoke("ResetHit", 1.0f);
        }
			
    }

    void ResetHit()
    {
        hit = false;
    }

}
