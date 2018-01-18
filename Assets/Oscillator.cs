using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [SerializeField] float period = 2f;

    float durationLoop;
    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (period <= Mathf.Epsilon) { 
            return;
        }

        float cycles = Time.time / period; //grows continually

        const float tau = Mathf.PI * 2f;
        float rawSineWave = Mathf.Sin(cycles * tau);

        durationLoop = rawSineWave / 2f + 0.5f;
        Vector3 offset = movementVector * durationLoop;
        transform.position = startingPos + offset;
	}
}
