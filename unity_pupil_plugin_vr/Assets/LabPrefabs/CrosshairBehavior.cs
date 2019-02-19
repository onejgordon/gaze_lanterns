using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairBehavior : MonoBehaviour {

    private GameObject selected;

    // Use this for initialization
    void Start () {
		
	}

    void OnTriggerEnter(Collider c)
    {
        NodeBehavior nb = c.gameObject.GetComponent<NodeBehavior>();
        nb.StartHover();
        selected = c.gameObject;
    }

    void OnTriggerExit(Collider c)
    {
        NodeBehavior nb = c.gameObject.GetComponent<NodeBehavior>();
        nb.StopHover();
        selected = null;
    }

    public bool HasSelection()
    {
        return selected != null;
    }

    public GameObject GetSelectedNode()
    {
        return selected;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
