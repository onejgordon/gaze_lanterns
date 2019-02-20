using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehavior : MonoBehaviour {

    private float velocity = 0.4f;
    public Vector3 direction;
    private Vector3 destination;
    const float deceleration = 0.02f;
    private Renderer rend;

    public static NodeBehavior CreateComponent(GameObject node, Vector3 _direction, Vector3 _destination)
    {
        NodeBehavior behavior = node.AddComponent<NodeBehavior>();
        behavior.direction = _direction;
        behavior.destination = _destination;
        Debug.Log("Intialized new behavior...");
        return behavior;
    }

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
		
	}
	
	// Update is called once per frame
	void Update () {
        if (velocity > 0)
        {
            velocity -= deceleration;
            transform.Translate(velocity * (destination - transform.position));
        }
        else velocity = 0;
	}

    public void StartHover()
    {
        rend.material.SetColor("_Color", Color.red);
    }

    public void StopHover()
    {
        rend.material.SetColor("_Color", Color.gray);
    }
}
