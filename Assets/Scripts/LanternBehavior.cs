using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehavior : MonoBehaviour {

    const float HEATING_RATE = 0.04f;
    const float HEATING_INTENSITY = 6f;
    const float DEFAULT_INTENSITY = 0.5f;
    const float COOLING_RATE = 0.96f;
    const float MAX_HEAT = 1.0f;
    const float MIN_ELEVATION = 1.0f;
    const float MAX_VELOCITY = .07f;
    const float MIN_VELOCITY = -0.02f;
    const float ACCEL_DIVIDER = 8000.0f;

    private float temperature = 0.25f; // 0-1
    private float ground_heat_elev;

    private Material light_material;
    private Material paper_material;
    private Light light;
    private float velocity = 0.0f;
    private int last_heat_timestamp = 0;
    private bool showing_recently_heated = false;

    void Start () {
        // First child is Light
        // this.light_material = this.transform.GetChild(0).GetComponent<Renderer>().material;
        this.paper_material = this.transform.GetChild(2).GetComponent<Renderer>().material;
        this.light = this.transform.GetChild(3).GetComponent<Light>();
		this.ground_heat_elev = Random.Range(4f, 8f);
	}
	
	void Update () {
        this.Accelerate();

        // Move lantern (rising or falling)
        this.Move();
    
        // Cool
        if (this.temperature > 0.0f) {
            this.temperature = this.temperature * COOLING_RATE;
        }

        this.UpdateAppearance();

	}

    private void Move() {
        float elev = this.transform.position.y;
        if (elev > MIN_ELEVATION || velocity > 0) this.transform.Translate(velocity * Vector3.up);   
        if (elev < ground_heat_elev) {
            // Heat slightly when near ground
            float mult = 0.09f * (ground_heat_elev - elev);
            this.Heat(mult, gaze_heating:false);
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
        bool recently_heated = Time.frameCount - this.last_heat_timestamp < 200;
        if (recently_heated != this.showing_recently_heated) {
            this.light.intensity = recently_heated ? HEATING_INTENSITY : DEFAULT_INTENSITY;
            this.showing_recently_heated = recently_heated;
        }
    }

    public void Heat(float mult=2.0f, float jitter=0.05f, bool gaze_heating=true)
    {
        float jitter_offset = 1.0f + Random.RandomRange(-jitter, jitter);
        if (this.temperature < MAX_HEAT) {
            this.temperature += HEATING_RATE * mult * jitter_offset;
            if (this.temperature > MAX_HEAT) this.temperature = MAX_HEAT;
        }
        if (gaze_heating) this.last_heat_timestamp = Time.frameCount;
    }

}
