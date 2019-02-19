using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWorld : MonoBehaviour
{
    public Camera steamCamera;

    private LineRenderer heading;
    private Vector3 standardViewportPoint = new Vector3(0.5f, 0.5f, 10);

    private Vector2 gazePointLeft;
    private Vector2 gazePointRight;
    private Vector2 gazePointCenter;

    public Material shaderMaterial;

    void Start()
    {
        PupilData.calculateMovingAverage = false;

        //steamCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        heading = gameObject.GetComponent<LineRenderer>();
    }

    void OnEnable()
    {
        if (PupilTools.IsConnected)
        {
            print("TestWorld.we're gazing");

            PupilSettings.Instance.currentCamera = steamCamera;
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
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            heading.enabled = !heading.enabled;
            print("Heading enabled: " + heading.enabled.ToString());
        }
            
        if (heading.enabled)
        {
            heading.SetPosition(0, steamCamera.transform.position - steamCamera.transform.up);

            Ray ray = steamCamera.ViewportPointToRay(viewportPoint);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                heading.SetPosition(1, hit.point);
            }
            else
            {
                heading.SetPosition(1, ray.origin + ray.direction * 50f);
            }
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
