using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehavior : MonoBehaviour {

    const float HEATING_RATE = 0.1f;
    const float HEATING_INTENSITY = 4f;
    const float DEFAULT_INTENSITY = 0.5f;
    const float GAZE_MULT = 4.0f;
    const float COOLING_RATE = 0.96f;
    const float MAX_HEAT = 1.0f;
    const float MIN_ELEVATION = 0.5f;
    const float MAX_VELOCITY = .07f;
    const float MIN_VELOCITY = -0.02f;
    const float ACCEL_DIVIDER = 8000.0f;

    private float temperature = 0.25f; // 0-1
    private float ground_heat_elev;

    private Material flame_material;
    private Material paper_material;
    private Transform flame;
    private Light light;
    private Color color;
    private float velocity = 0.0f;
    private bool showing_recently_heated = false;

    void Start () {
        // First child is interior flame
        this.flame = this.transform.GetChild(0).GetComponent<Transform>();
        this.flame_material = this.transform.GetChild(0).GetComponent<Renderer>().material;
        this.paper_material = this.transform.GetChild(2).GetComponent<Renderer>().material;
        this.light = this.transform.GetChild(3).GetComponent<Light>();
		this.ground_heat_elev = 2;
        float g = Random.RandomRange(0.0f, 0.6f);
        float b = Random.RandomRange(0.0f, 0.6f);
        this.color = new Color(1f, g, b, 0.8f);
        this.paper_material.SetColor("_Color", color);
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
            float mult = 0.12f * (ground_heat_elev - elev);
            this.Heat(mult, gaze_heating:false);
        }
    }

    private void Accelerate() {
        // Accelerate up when temp high, down when low
        this.velocity = this.velocity + ((this.temperature - 0.1f) / ACCEL_DIVIDER);
        if (this.velocity > MAX_VELOCITY) this.velocity = MAX_VELOCITY;
        if (this.velocity < MIN_VELOCITY) this.velocity = MIN_VELOCITY;
    }

    private void UpdateAppearance() {
        float luminance = (this.temperature + 0.2f) / 1.2f;
        this.paper_material.SetVector("_EmissionColor", this.color * luminance);
        float scaleFactor = .01f + this.temperature * .055f;
        this.flame.localScale = new Vector3(scaleFactor, 2*scaleFactor, scaleFactor);
        this.light.intensity = this.temperature * HEATING_INTENSITY + DEFAULT_INTENSITY;
    }

    public void Heat(float mult=GAZE_MULT, float jitter=0.05f, bool gaze_heating=true)
    {
        float jitter_offset = 1.0f + Random.RandomRange(-jitter, jitter);
        if (this.temperature < MAX_HEAT) {
            this.temperature += HEATING_RATE * mult * jitter_offset;
            if (this.temperature > MAX_HEAT) this.temperature = MAX_HEAT;
        }
    }

}
