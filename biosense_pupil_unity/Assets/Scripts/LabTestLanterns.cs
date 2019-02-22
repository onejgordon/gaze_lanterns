using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabTestLanterns : MonoBehaviour
{
    private Camera steamCamera;

    private LineRenderer heading;
    private Vector3 standardViewportPoint = new Vector3(0.5f, 0.5f, 10);

    private Vector2 gazePointLeft;
    private Vector2 gazePointRight;
    private Vector2 gazePointCenter;

    public Transform marker;
    public bool showGazeLaser;
    public bool showEyeMarkers;

    public Material shaderMaterial;

    void Start()
    {
        PupilData.calculateMovingAverage = true;

        steamCamera = gameObject.GetComponent<Camera> ();
        heading = gameObject.GetComponent<LineRenderer>();
        heading.enabled = showGazeLaser;
    }

    void OnEnable()
    {
        if (PupilTools.IsConnected)
        {
            PupilGazeTracker.Instance.StartVisualizingGaze();
            PupilTools.IsGazing = true;
            PupilTools.SubscribeTo("gaze");
        }
    }

    void Update()
    {
        Vector3 viewportPoint = standardViewportPoint;

        if (PupilTools.IsConnected && PupilTools.IsGazing)
        {
            gazePointLeft = PupilData._2D.GetEyePosition(steamCamera, PupilData.leftEyeID);
            gazePointRight = PupilData._2D.GetEyePosition(steamCamera, PupilData.rightEyeID);
            gazePointCenter = PupilData._2D.GazePosition;
            viewportPoint = new Vector3(gazePointCenter.x, gazePointCenter.y, 1f);

            if (showEyeMarkers) marker.localPosition = PupilData._2D.GazePosition;
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            heading.enabled = !heading.enabled;
            print("Heading enabled: " + heading.enabled.ToString());
        }
        
        heading.SetPosition(0, steamCamera.transform.position - steamCamera.transform.up);

        float thickness = 20f;
        Vector3 origin = steamCamera.transform.position;
        Vector3 direction = viewportPoint - origin;
        RaycastHit hit;
        if (Physics.SphereCast(origin, thickness, direction, out hit))
        {
            heading.SetPosition(1, hit.point);

            // Check if hit object is lantern
            GameObject hitObject = hit.transform.gameObject;
            LanternBehavior lb = hitObject.GetComponent<LanternBehavior>();
            if (lb != null) {
                // Heat this lantern
                Debug.Log("Hit a lantern");
                lb.Heat();
            }
        }
        else
        {
            Ray ray = steamCamera.ViewportPointToRay(viewportPoint);
            heading.SetPosition(1, ray.origin + ray.direction * 50f);
        }
    
    }
    
    void OnDisable()
    {
        print("Disable");

        if (PupilTools.IsConnected)
            PupilTools.UnSubscribeFrom("gaze");

        if (PupilTools.IsConnected && PupilTools.IsGazing)
        {
            PupilGazeTracker.Instance.StopVisualizingGaze();
            print("We stopped gazing.");
        }
    }
}
