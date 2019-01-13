using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

namespace Swift
{
    /// <summary>
    /// Make the Steam VR available in multiplayer by deactivating script for UserOther
    /// Support version # SteamVR Unity Plugin - v2.0.1
    /// </summary>
    public class VR_CameraRigMultiuser : NetworkBehaviour
    {

        // reference to SteamController
        public GameObject SteamVRLeft, SteamVRRight, SteamVRCamera;
        public GameObject UserOtherLeftHandModel, UserOtherRightHandModel;
        private GameObject goFreeLookCameraRig;

        // Use this for initialization
        void Start()
        {
            updateGoFreeLookCameraRig();
            steamVRactivation();
        }

        /// <summary>
        /// deactivate the FreeLookCameraRig since we are using the HTC version
        /// Execute only in client side
        /// </summary>
        protected void updateGoFreeLookCameraRig()
        {
            // Client execution ONLY LOCAL
            if (!isLocalPlayer) return;

            goFreeLookCameraRig = null;

            try
            {
                // Get the Camera to set as the follow camera
                goFreeLookCameraRig = transform.Find("/FreeLookCameraRig").gameObject;
                // Deactivate the FreeLookCameraRig since we are using the SteamVR camera
                goFreeLookCameraRig.SetActive(false);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
            }
        }


        /// <summary>
        /// If we are the client who is using the HTC, activate component of SteamVR in the client using it
        /// If we are not the client using this specific HTC, deactivate some scripts.
        /// </summary>
        protected void steamVRactivation()
        {
            // client execution for ALL

            // Left activation if UserMe, deactivation if UserOther
            SteamVRLeft.GetComponent<SteamVR_Behaviour_Pose>().enabled = isLocalPlayer;

            // Left SteamVR_RenderModel activation if UserMe, deactivation if UserOther
            //SteamVRLeft.GetComponentInChildren<SteamVR_RenderModel>().enabled = isLocalPlayer;
            SteamVRLeft.transform.Find("Model").gameObject.SetActive(isLocalPlayer);

            // Right activation if UserMe, deactivation if UserOther
            SteamVRRight.GetComponent<SteamVR_Behaviour_Pose>().enabled = isLocalPlayer;

            // Left SteamVR_RenderModel activation if UserMe, deactivation if UserOther
            //SteamVRRight.GetComponentInChildren<SteamVR_RenderModel>().enabled = isLocalPlayer;
            SteamVRRight.transform.Find("Model").gameObject.SetActive(isLocalPlayer);

            // Camera activation if UserMe, deactivation if UserOther
            SteamVRCamera.GetComponent<Camera>().enabled = isLocalPlayer;

            if (!isLocalPlayer)
            {
                // ONLY for player OTHER

                // Create the model of the LEFT Hand for the UserOther, use a SteamVR model  Assets/SteamVR/Models/vr_glove_left_model_slim.fbx
                var modelLeft = Instantiate(UserOtherLeftHandModel);
                // Put it as a child of the SteamVRLeft Game Object
                modelLeft.transform.parent = SteamVRLeft.transform;

                // Create the model of the RIGHT Hand for the UserOther Assets/SteamVR/Models/vr_glove_right_model_slim.fbx
                var modelRight = Instantiate(UserOtherRightHandModel);
                // Put it as a child of the SteamVRRight Game Object
                modelRight.transform.parent = SteamVRRight.transform;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}