
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


namespace Valve.VR.InteractionSystem.Sample
{
    public class ShootLantern : MonoBehaviour
    {
        public SteamVR_Action_Boolean squeezeAction;

        private Hand hand;

        public GameObject prefabToShoot;
        
        const float CROSSHAIR_DIST = 2.3f;
        const SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.RightHand;

        void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

            if (squeezeAction == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No plant action assigned");
                return;
            }

            squeezeAction.AddOnChangeListener(OnFireActionChange, inputSource);
        }

        private void OnDisable()
        {
            if (squeezeAction != null)
                squeezeAction.RemoveOnChangeListener(OnFireActionChange, inputSource);
        }

        private void OnFireActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newVal)
        {
            Debug.Log("OnFireActionChange: " + newVal.ToString());
            if (newVal) Shoot();
        }

        public void Shoot()
        {
            StartCoroutine(DoShoot());
        }

        private IEnumerator DoShoot()
        {
            Vector3 lanternDestinationPos;
            float velocity = 0.3f;
            float deceleration = 0.017f;

            lanternDestinationPos = getControllerPosition() + this.hand.transform.forward * CROSSHAIR_DIST;
            Debug.Log("Destination: " + lanternDestinationPos.ToString());

            GameObject lantern = GameObject.Instantiate<GameObject>(prefabToShoot);
            LanternBehavior behavior = lantern.AddComponent<LanternBehavior>();
            lantern.transform.position = hand.transform.position;

            float startTime = Time.time;
            float overTime = 2.5f;
            float endTime = startTime + overTime;

            while (velocity > 0)
            {
                lantern.transform.Translate(velocity * (lanternDestinationPos - lantern.transform.position));
                velocity -= deceleration;
                yield return null;
            }


        }

        public Vector3 getControllerPosition()
        {
            return this.hand.transform.position;
        }

        public Quaternion getControllerRotation()
        {
            return this.hand.transform.rotation;
        }
    }
}
