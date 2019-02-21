using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehavior : MonoBehaviour {

    private float temperature = 0.25f; // 0-1
    const float heating_rate = 0.02f;
    const float max_heat = 1.0f;
    const float cooling_rate = 0.999f;
    public Vector3 direction;
    private Vector3 destination;
    
    private Renderer rend;
    private bool focusing = false;


    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
		
	}
	
	void Update () {
        float velocity = this.getVelocity();

        // Move lantern (rising or falling)
        transform.Translate(velocity * Vector3.up);
    
        // Cool
        if (this.temperature > 0) {
            this.temperature = this.temperature * cooling_rate;
            this.UpdateAppearance();
        }
	}

    private float getVelocity() {
        // Fall when temperature low, rise when high
        return this.temperature - 0.2f;
    }

    private void UpdateAppearance() {
        Color c = Color.red;
        c.a = (this.temperature + 0.2f) / 1.2f;
        rend.material.SetColor("_Color", c);
    }

    public void Heat()
    {
        if (this.temperature < max_heat) {
            this.temperature += heating_rate;
            this.UpdateAppearance();
        }
    }

}
