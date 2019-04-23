﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PupilLabs
{
    public class GazeVisualizer : MonoBehaviour
    {
        public SubscriptionsController subscriptionsController;
        public Transform cameraTransform;

        [Header("Settings")]
        [Range(0f, 1f)]
        public float confidenceThreshold = 0.6f;

        [Header("Projected Visualization")]
        public Transform projectionMarker;
        [Range(0.01f, 0.1f)]
        public float sphereCastRadius = 0.05f;

        GazeListener gazeListener = null;
        Vector3 localGazeDirection;
        float gazeDistance;
        bool isGazing = false;
        public bool showGazeRay = true;

        void OnEnable()
        {
            StartVisualizing();
        }

        void OnDisable()
        {
            StopVisualizing();
        }

        void Update()
        {
            if (!isGazing)
            {
                return;
            }

            ShowProjected();
        }

        public void StartVisualizing()
        {
            Debug.Log("Start Visualizing Gaze");

            if (subscriptionsController == null)
            {
                Debug.LogError("SubscriptionController missing");
                return;
            }

            if (projectionMarker == null)
            {
                Debug.LogError("Marker reference missing");
                return;
            }

            if (cameraTransform == null)
            {
                Debug.LogError("Camera reference missing");
                enabled = false;
                return;
            }

            if (gazeListener == null)
            {
                gazeListener = new GazeListener(subscriptionsController);
            }

            gazeListener.OnReceive3dGaze += ReceiveGaze;
            projectionMarker.gameObject.SetActive(true);
            isGazing = true;
        }

        public void StopVisualizing()
        {
            isGazing = false;

            if (gazeListener != null)
            {
                gazeListener.OnReceive3dGaze -= ReceiveGaze;
            }

            if (projectionMarker != null)
            {
                projectionMarker.gameObject.SetActive(false);
            }
        }

        void ReceiveGaze(GazeData gazeData)
        {
            if (gazeData.Confidence >= confidenceThreshold)
            {
                localGazeDirection = gazeData.GazeDirection;
                gazeDistance = gazeData.GazeDistance;
            }
        }

        void ShowProjected()
        {
            if (projectionMarker == null)
            {
                Debug.LogWarning("Marker missing");
                return;
            }

            projectionMarker.gameObject.SetActive(true);

            Vector3 origin = cameraTransform.position;
            origin += Vector3.down * 0.1f;
            
            Vector3 direction = cameraTransform.TransformDirection(localGazeDirection);

            if (Physics.SphereCast(origin, sphereCastRadius, direction, out RaycastHit hit, Mathf.Infinity))
            {
                Debug.DrawRay(origin, direction * hit.distance, Color.yellow);
                projectionMarker.gameObject.SetActive(true);
                if (showGazeRay) {
                    projectionMarker.position = origin;
                    float dist = Vector3.Distance(origin, hit.point);
                    projectionMarker.localScale = new Vector3(.02f, dist, .02f);
                    projectionMarker.up = hit.point - origin;
                } else {
                    projectionMarker.position = hit.point;
                    projectionMarker.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                }
            }
            else
            {
                projectionMarker.gameObject.SetActive(false);
                Debug.DrawRay(origin, direction * 10, Color.white);
            }
        }
    }
}
