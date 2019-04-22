using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PupilLabs
{
    public class LookHandler : MonoBehaviour
    {
        public SubscriptionsController subscriptionsController;
        public Transform cameraTransform;

        [Header("Settings")]
        [Range(0f, 1f)]
        public float confidenceThreshold = 0.6f;

        [Header("Projected Visualization")]
        public float sphereCastRadius = 0.05f;

        GazeListener gazeListener = null;
        Vector3 localGazeDirection;
        float gazeDistance;
        bool isGazing = false;

        void OnEnable()
        {
            StartLooking();
        }

        void OnDisable()
        {
            StopLooking();
        }

        void Update()
        {
            if (!isGazing)
            {
                return;
            }

            ProjectGaze();
        }

        public void StartLooking()
        {
            Debug.Log("Start looking");

            if (subscriptionsController == null)
            {
                Debug.LogError("SubscriptionController missing");
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
            isGazing = true;
        }

        public void StopLooking()
        {
            isGazing = false;

            if (gazeListener != null)
            {
                gazeListener.OnReceive3dGaze -= ReceiveGaze;
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

        void ProjectGaze()
        {

            Vector3 origin = cameraTransform.position;

            Vector3 direction = cameraTransform.TransformDirection(localGazeDirection);

            if (Physics.SphereCast(origin, sphereCastRadius, direction, out RaycastHit hit, Mathf.Infinity))
            {
                Debug.DrawRay(origin, direction * hit.distance, Color.yellow);
                GameObject objectHit = hit.transform.gameObject;
                LanternBehavior beh = objectHit.GetComponent<LanternBehavior>();
                if (beh != null) {
                    // Hit a lantern
                    Debug.Log("Heating a lantern...");
                    beh.Heat();
                }
            }
            else
            {
                Debug.DrawRay(origin, direction * 10, Color.white);
            }
        }
    }
}
