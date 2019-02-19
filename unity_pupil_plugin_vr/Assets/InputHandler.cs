using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public GameObject newNode;
    public GameObject crosshair;
    private CrosshairBehavior cb;
    const float CROSSHAIR_DIST = 1.3f;
    private bool grabbing = false;

    // 1
    private SteamVR_TrackedObject trackedObj;
    // 2
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void Start()
    {
        cb = crosshair.GetComponent<CrosshairBehavior>();
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update () {
        // Update crosshair
        crosshair.transform.position = Controller.transform.pos + transform.forward * CROSSHAIR_DIST;
        if (grabbing)
        {
            if (cb.HasSelection())
            {
                // Should be true if grabbing
                cb.GetSelectedNode().transform.position = crosshair.transform.position;
            }
        }

        if (Controller.GetAxis() != Vector2.zero)
        {
            Debug.Log(gameObject.name + Controller.GetAxis());
        }

        // 2
        if (Controller.GetHairTriggerDown())
        {
            if (cb.HasSelection())
            {
                DeleteNode(cb.GetSelectedNode());
            } else
            {
                AddNewNode();
            }
            
        }

        // 3
        if (Controller.GetHairTriggerUp())
        {
            Debug.Log(gameObject.name + " Trigger Release");
        }

        // 4
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Press");
            if (cb.HasSelection())
            {
                grabbing = true;
            }
        }

        // 5
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Release");
            if (cb.HasSelection())
            {
                grabbing = false;
            }
        }
    }

    void AddNewNode()
    {
        GameObject nodeInstance = Instantiate(newNode, Controller.transform.pos, Quaternion.identity) as GameObject;
        NodeBehavior.CreateComponent(nodeInstance, transform.forward, Controller.transform.pos + transform.forward * CROSSHAIR_DIST);
        nodeInstance.AddComponent<SphereCollider>();
        Controller.TriggerHapticPulse(300);
    }

    void DeleteNode(GameObject node)
    {
        Destroy(node);
    }
}
