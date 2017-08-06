using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class SlimeBrain : EnemyBrainV2<SlimeVehicle>
    {
        public SlimeVehicle slimeClone;
        public float reproduceEvery;
        public Vector2 reproduceSpawnDelta = new Vector2(0, -0.6f);

        private float reproduceTimer;
        private bool reproducing = false;

        void Start()
        {
            RemoveAllGoals();
            AddGoal(new Goal_Wander(veh));
            reproduceTimer = reproduceEvery;
        }

        protected override void Update()
        {
            base.Update();

            if (!reproducing)
            {
                reproduceTimer -= veh.DeltaTime();
                if (reproduceTimer < 0)
                    Reproduce();
            }
        }

        void Reproduce()
        {
            reproducing = true;

            Goal g = new Goal_Manual();
            veh.animator.ReproduceAnimation(OnReproduceMoment, g.ForceCompletion);
            g.onRemoved = (Goal goal) =>
             {
                 reproducing = false;
                 reproduceTimer = reproduceEvery;
             };
            AddGoal(g);
        }

        void OnReproduceMoment()
        {
            //Spawn other slime
            Vector2 spawnDelta = transform.localToWorldMatrix.MultiplyVector(reproduceSpawnDelta);
            SlimeVehicle newSlime = Game.instance.SpawnUnit(slimeClone, Game.instance.aiArea.ClampToArea(veh.Position + spawnDelta));
            newSlime.transform.rotation = transform.rotation;

            SlimeBrain brain = newSlime.GetComponent<SlimeBrain>();
            newSlime.enabled = false;
            brain.enabled = false;
            newSlime.visuals.gameObject.SetActive(false);
            newSlime.animator.BirthAnimation(
                () =>
                {
                    newSlime.visuals.gameObject.SetActive(true);
                },
                () =>
                {
                    brain.enabled = true;
                    newSlime.enabled = true;
                });
        }
    }
}
