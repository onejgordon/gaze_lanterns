using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehavior : MonoBehaviour {

    private float temperature = 0.2f; // 0-1
    const float heating_rate = 0.02f;
    const float cooling_rate = 0.7f;
    public Vector3 direction;
    private Vector3 destination;
    
    private Renderer rend;
    private bool focusing = false;


    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
		
	}
	
	void Update () {
        float velocity = temperature;

        // Move lantern (rising)
        if (velocity > 0) transform.Translate(velocity * Vector3.up);
        
        bool change = false;

        // Heat if gazing
        if (focusing) {
            temperature += heating_rate;
            change = true;
        }
        
        // Cool
        if (this.temperature > 0) {
            this.temperature = this.temperature * cooling_rate;
            change = true;
            if (this.temperature < 0) this.temperature = 0.0f;
        }

        if (change) {
            // Adjust color
            Color c = Color.red;
            c.a = this.temperature;
            rend.material.SetColor("_Color", c);
        }
	}

    public void StartFocus()
    {
        this.focusing = true;
    }

    public void StopFocus()
    {
        this.focusing = false;
    }
}
