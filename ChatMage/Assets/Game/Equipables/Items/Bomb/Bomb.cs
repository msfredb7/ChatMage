using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb
{
    public BombScript movingBomb;
    GameObject bomb;
    int amountOfBumps;
    
    float currentThrowSpeed;
    float currentBumpDuration;

    public Bomb(GameObject bomb, int amountOfBumps) // Les visuels peuvent etre different (meteor, boulet, rocket, etc.)
    {
        movingBomb = bomb.GetComponent<BombScript>();
        this.bomb = bomb;
        this.amountOfBumps = amountOfBumps;
        movingBomb.rb.position = Game.instance.Player.vehicle.Position;
    }

    public void Throw(Vector2 destination, Vector2 from, float throwSpeed, float explosionForce, float bumbDuration = 0.25f)
    {
        // Initialisation
        currentThrowSpeed = throwSpeed;
        currentBumpDuration = bumbDuration;

        // Physique
        Vector2 direction = destination - from;
        movingBomb.Bump(direction * currentThrowSpeed, currentBumpDuration, BumpMode.VelocityChange);

        // Explosion
        movingBomb.SetExplosionForce(explosionForce);

        // Effet de bond multiple

        // Prochain Bond
        currentThrowSpeed = throwSpeed / 2; // Vraiment moins vite, plus lent
        currentBumpDuration = bumbDuration * 2; // Pendant plus longtemps

        movingBomb.onBumpComplete.AddListener(delegate (Vehicle vehicle) { movingBomb.Explode(); });
    }
}
