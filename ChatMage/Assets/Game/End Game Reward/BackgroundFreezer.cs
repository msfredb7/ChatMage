using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndGameReward
{
    [RequireComponent(typeof(Camera))]
    public class BackgroundFreezer : MonoBehaviour
    {
        public MeshRenderer background;
        public ScreenShooter screenShooter;
        public UnityStandardAssets.ImageEffects.BlurOptimized blur;

        private Camera cameraToCopy; 
        Action onComplete;

        void Awake()
        {
            background.enabled = false;
            screenShooter.enabled = false;
            blur.enabled = false;
        }

        public void FreezeBackground(Camera cameraToCopy, Action onComplete)
        {
            this.onComplete = onComplete;
            this.cameraToCopy = cameraToCopy;

            screenShooter = cameraToCopy.gameObject.AddComponent<ScreenShooter>();

            //Shoot screen !
            screenShooter.Shoot(OnScreenShot);
        }

        void OnScreenShot(RenderTexture texture)
        {
            print("shot taken");
            Camera myCam = GetComponent<Camera>();

            //Copy parameters
            myCam.transform.position = cameraToCopy.transform.position;
            myCam.orthographicSize = cameraToCopy.orthographicSize;
            myCam.transform.rotation = cameraToCopy.transform.rotation;

            //Enable mine, disable old
            cameraToCopy.gameObject.SetActive(false);
            myCam.gameObject.SetActive(true);


            //On met le background a la bonne grosseur / position
            background.transform.localScale = new Vector3(myCam.orthographicSize * myCam.aspect * 2, myCam.orthographicSize * 2, 1);
            background.transform.position = myCam.transform.position + (Vector3.forward * 2);


            //New Texture
            RenderTexture newTexture = new RenderTexture(texture.width, texture.height, texture.depth);
            
            //Blur Texture
            blur.RemoteRenderImage(texture, newTexture);

            //Set Texture
            background.material.mainTexture = newTexture;

            //Enable background
            background.enabled = true;

            if (onComplete != null)
                onComplete();
        }
    }
}
