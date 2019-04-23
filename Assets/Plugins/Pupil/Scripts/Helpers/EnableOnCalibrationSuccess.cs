using UnityEngine;

namespace PupilLabs
{
    public class EnableOnCalibrationSuccess : MonoBehaviour
    {
        public CalibrationController calibrationController;
        public GameObject objectToBeActivated;
        public GameObject eyeFrameViz;

        void OnEnable()
        {
            calibrationController.OnCalibrationSucceeded += EnableComponent;
        }

        void OnDisable()
        {
            calibrationController.OnCalibrationSucceeded -= EnableComponent;
        }

        void EnableComponent()
        {
            objectToBeActivated.SetActive(true);

            if (eyeFrameViz != null) eyeFrameViz.SetActive(false);
        }
    }
}
