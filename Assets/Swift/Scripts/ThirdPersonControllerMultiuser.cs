using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Swift
{
    public class ThirdPersonControllerMultiuser : NetworkBehaviour
    {
        public Material PlayerLocalMat;
        /// <summary>
        /// Represents the GameObject on which to change the color for the local player
        /// </summary>
        public GameObject GameObjectLocalPlayerColor;

        /// <summary>
        /// The FrreeLookCameraRig GameObject to configure for the UserMe
        /// </summary>
        GameObject goFreeLookCameraRig = null;


        /// <summary>
        /// The Transform from which the snow ball is spawned
        /// </summary>
        [SerializeField]
        Transform snowballSpawner;
        /// <summary>
        /// The prefab to create when spawning
        /// </summary>
        [SerializeField]
        GameObject SnowballPrefab;

        // Use to configure the throw ball feature
        [Range(0.2f, 100.0f)]
        public float MinSpeed;
        [Range(0.2f, 100.0f)]
        public float MaxSpeed;
        [Range(0.2f, 100.0f)]
        public float MaxPressDuration;
        private float pressDuration = 0;



        // Use this for initialization
        void Start()
        {
            Debug.Log("isLocalPlayer:" + isLocalPlayer);
            updateGoFreeLookCameraRig();
            followLocalPlayer();
            activateLocalPlayer();
        }

        /// <summary>
        /// Get the GameObject of the CameraRig
        /// </summary>
        protected void updateGoFreeLookCameraRig()
        {
            if (!isLocalPlayer) return;
            try
            {
                // Get the Camera to set as the followed camera
                goFreeLookCameraRig = transform.Find("/FreeLookCameraRig").gameObject;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
            }
        }

        /// <summary>
        /// Make the CameraRig following the LocalPlayer only.
        /// </summary>
        protected void followLocalPlayer()
        {
            if (isLocalPlayer)
            {
                if (goFreeLookCameraRig != null)
                {
                    // find Avatar EthanHips
                    Transform transformFollow = transform.Find("EthanSkeleton/EthanHips") != null ? transform.Find("EthanSkeleton/EthanHips") : transform;
                    // call the SetTarget on the FreeLookCam attached to the FreeLookCameraRig
                    goFreeLookCameraRig.GetComponent<FreeLookCam>().SetTarget(transformFollow);
                    Debug.Log("ThirdPersonControllerMultiuser follow:" + transformFollow);
                }
            }
        }

        protected void activateLocalPlayer()
        {
            // enable the ThirdPersonUserControl if it is a Loacl player = UserMe
            // disable the ThirdPersonUserControl if it is not a Loacl player = UserOther
            GetComponent<ThirdPersonUserControl>().enabled = isLocalPlayer;
            if (isLocalPlayer)
            {
                try
                {
                    // Change the material of the Ethan Glasses
                    GameObjectLocalPlayerColor.GetComponent<Renderer>().material = PlayerLocalMat;
                }
                catch (System.Exception)
                {

                }
            }
        }





        #region Snwoball Spawn
        // Update is called once per frame
        void Update()
        {
            // Don't do anything if we are not the UserMe isLocalPlayer
            if (!isLocalPlayer) return;

            if (Input.GetButtonDown("Fire1"))
            {
                // Start Loading time when fire is pressed
                pressDuration = 0.0f;
            }
            else if (Input.GetButton("Fire1"))
            {
                // count the time the Fire1 is pressed
                //pressDuration += ???; 
                pressDuration += Time.deltaTime;
            }

            else if (Input.GetButtonUp("Fire1"))
            {
                // When releasing Fire1, spawn the ball
                // Define the initial speed of the Snowball between MinSpeed and MaxSpeed according to the duration the button is pressed
                var speed = Mathf.Clamp(MinSpeed + pressDuration / MaxPressDuration * (MaxSpeed - MinSpeed), MinSpeed, MaxSpeed); // update with the right value
                Debug.Log(string.Format("time {0:F2} <  {1} => speed {2} < {3} < {4}", pressDuration, MaxPressDuration, MinSpeed, speed, MaxSpeed));
                CmdThrowBall(speed);
            }
        }

        [Command]
        void CmdThrowBall(float speed)
        {
            // Create the Snowball from the Snowball Prefab
            GameObject snowball = Instantiate(
                SnowballPrefab,
                snowballSpawner.position,
                Quaternion.identity);


            // Add velocity to the Snowball
            snowball.GetComponent<Rigidbody>().velocity = snowballSpawner.transform.forward * speed;

            // Spawn the Snowball on the Clients
            NetworkServer.Spawn(snowball);

            // Destroy the Snowball after 2 seconds
            Destroy(snowball, 5.0f);
        }
        #endregion
    }
}
