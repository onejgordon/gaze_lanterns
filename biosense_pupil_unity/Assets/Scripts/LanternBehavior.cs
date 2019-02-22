using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehavior : MonoBehaviour {

    const float HEATING_RATE = 0.025f;
    const float COOLING_RATE = 0.99f;
    const float MAX_HEAT = 1.0f;
    const float MIN_ELEVATION = 1.0f;
    const float MAX_VELOCITY = .1f;
    const float MIN_VELOCITY = -0.05f;
    const float ACCEL_DIVIDER = 8000.0f;

    private float temperature = 0.25f; // 0-1
    private float ground_heat_elev;

    private Material light_material;
    private float velocity = 0.0f;

    void Start () {
        // First child is Light
        this.light_material = this.transform.GetChild(0).GetComponent<Renderer>().material;
		this.ground_heat_elev = Random.RandomRange(2f, 5f);
	}
	
	void Update () {
        this.Accelerate();

        // Move lantern (rising or falling)
        this.Move();
    
        // Cool
        if (this.temperature > 0.0f) {
            this.temperature = this.temperature * COOLING_RATE;
            this.UpdateAppearance();
        }
	}

    private void Move() {
        float elev = this.transform.position.y;
        if (elev > MIN_ELEVATION || velocity > 0) this.transform.Translate(velocity * Vector3.up);   
        if (elev < ground_heat_elev) {
            // Heat slightly when near ground
            float mult = 0.3f * (ground_heat_elev - elev);
            this.Heat(mult);
        }
    }

    private void Accelerate() {
        // Accelerate up when temp high, down when low
        this.velocity = this.velocity + ((this.temperature - 0.2f) / ACCEL_DIVIDER);
        if (this.velocity > MAX_VELOCITY) this.velocity = MAX_VELOCITY;
        if (this.velocity < MIN_VELOCITY) this.velocity = MIN_VELOCITY;
    }

    private void UpdateAppearance() {
        float luminance = (this.temperature + 0.2f) / 1.2f;
        this.light_material.SetVector("_EmissionColor", Color.white * luminance);
    }

    public void Heat(float mult=2.0f, float jitter=0.05f)
    {
        float jitter_offset = 1.0f + Random.RandomRange(-jitter, jitter);
        if (this.temperature < MAX_HEAT) {
            this.temperature += HEATING_RATE * mult * jitter_offset;
            if (this.temperature > MAX_HEAT) this.temperature = MAX_HEAT;
            this.UpdateAppearance();
        }
    }

}
