using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMarkerBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c)
    {
        LanternBehavior lb = c.gameObject.GetComponent<LanternBehavior>();
        lb.StartFocus();
    }

    void OnTriggerExit(Collider c) {
        LanternBehavior lb = c.gameObject.GetComponent<LanternBehavior>();
        lb.StopFocus();
    }
}
