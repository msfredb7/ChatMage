using GameIntroOutro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using CCC.Utility;

namespace GameIntroOutro
{
    public class WO_PortalFromParticles : StdWinOutro
    {
        public string remoteTag;
        public PlayerBounds.FourBoundStates boundStates;

        [Header("Particles")]
        public float particlesDelay;

        [Header("Wind zone")]
        public float windZoneDelay;

        [Header("Portal")]
        public float portalDelay;
        public float portalOnJesusOffset;

        private ParticleSystem ps;
        private WindZone windZone;
        private PortalVFX portal;
        private JesusV2Vehicle jesus;
        private GameObject topCollider;
        private bool playerHasExited = false;
        private GameObject topWallVisuals;
        Sequence sq;

        public override void Play()
        {
            LoadWinScene();

            FetchReferences();

            topWallVisuals.gameObject.SetActive(true);

            sq = DOTween.Sequence();

            //Particles
            ps.transform.position = jesus.Position + jesus.WorldDirection2D() * portalOnJesusOffset;

            //Delay
            sq.AppendInterval(particlesDelay);
            //Wind Zone
            sq.AppendCallback(ps.Play);

            //Delay
            sq.AppendInterval(windZoneDelay);
            //Wind Zone
            sq.AppendCallback(() => windZone.gameObject.SetActive(true));

            //Delay
            sq.AppendInterval(portalDelay);
            //Portal open
            sq.AppendCallback(portal.Open);

            //Delay
            sq.AppendInterval(1f);
            //Top Wall disable
            sq.AppendCallback(() =>
            {
                Game.instance.playerBounds.SetStates(boundStates);
                topCollider.SetActive(false);
            });


            AddTimescaleListener();
        }

        private void FetchReferences()
        {
            Mapping mp = Game.instance.map.mapping;

            PortalFromParticlesRemote remote = mp.GetTaggedObject(remoteTag).GetComponent<PortalFromParticlesRemote>();
            jesus = remote.jesus;
            portal = remote.portalVFX;
            windZone = remote.windZone;
            ps = remote.particleSystem;
            topCollider = remote.topCollider;
            topWallVisuals = remote.topWallVisuals;
            remote.winCollider.onTriggerEnter += WinCollider_onTriggerEnter;
        }

        private void WinCollider_onTriggerEnter(ColliderInfo other, ColliderListener listener)
        {
            if (other.parentUnit == Game.instance.Player.vehicle)
            {
                playerHasExited = true;
                CheckEnd();
            }
        }

        protected void UpdateTimescale(float worldTimescale)
        {
            if (sq != null)
                sq.timeScale = worldTimescale;
            ParticleSystem.MainModule psMain = ps.main;
            psMain.simulationSpeed = worldTimescale;
        }

        protected void AddTimescaleListener()
        {
            if (Game.instance == null)
            {
                Debug.LogError(name + " tried to a add listener to worldTimescale but Game.instance == null");
                return;
            }

            StatFloat worldTimescale = Game.instance.worldTimeScale;
            worldTimescale.onSet.AddListener(UpdateTimescale);
            UpdateTimescale(worldTimescale);
        }

        protected void RemoveTimescaleListener()
        {
            StatFloat worldTimescale = Game.instance.worldTimeScale;
            worldTimescale.onSet.RemoveListener(UpdateTimescale);
        }

        protected override bool CanEnd()
        {
            return playerHasExited;
        }
    }
}
