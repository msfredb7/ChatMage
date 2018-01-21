using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameParticleEffect))]
public class GameParticleEffectEditor : Editor
{
    GameParticleEffect gameParticleEffect;

    void OnEnable()
    {
        gameParticleEffect = target as GameParticleEffect;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUI.enabled = !gameParticleEffect.IsPlaying && Application.isPlaying;
        if (GUILayout.Button("Activate"))
        {
            gameParticleEffect.Play();
        }

        GUI.enabled = gameParticleEffect.IsPlaying && Application.isPlaying;

        if (GUILayout.Button("Deactivate"))
        {
            gameParticleEffect.StopPlaying();
        }

        GUI.enabled = true;
    }
}
