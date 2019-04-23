
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


namespace Valve.VR.InteractionSystem.Sample
{
    public class ShootLantern : MonoBehaviour
    {
        public SteamVR_Action_Boolean squeezeAction;
        
        public GameObject prefabToShoot;
        
        private Transform transform;

        const float MIN_DEST_DIST = 2.3f;
        const SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.RightHand;

        void OnEnable()
        {
            if (transform == null) transform =  GetComponent<Transform>();


            if (squeezeAction == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No action assigned");
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

            float dist_to_send = Random.Range(MIN_DEST_DIST, MIN_DEST_DIST * 5);
            lanternDestinationPos = getControllerPosition() + this.transform.forward * dist_to_send;

            GameObject lantern = GameObject.Instantiate<GameObject>(prefabToShoot);
            LanternBehavior behavior = lantern.AddComponent<LanternBehavior>();
            lantern.transform.position = getControllerPosition();

            while (velocity > 0)
            {
                lantern.transform.Translate(velocity * (lanternDestinationPos - lantern.transform.position));
                velocity -= deceleration;
                yield return null;
            }


        }

        public Vector3 getControllerPosition()
        {
            return this.transform.position;
        }

        public Quaternion getControllerRotation()
        {
            return this.transform.rotation;
        }
    }
}
