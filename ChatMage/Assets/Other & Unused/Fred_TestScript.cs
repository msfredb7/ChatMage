using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using Dialoguing;
using CCC.Manager;

public class Fred_TestScript : BaseBehavior
{
    public Dialog permanentSkip;
    public List<Dialog> clips;

    void Start()
    {
        MasterManager.Sync();
        Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Loadout
            LoadoutResult loadoutResult = new LoadoutResult();

            //Scene message à donner au framework
            ToGameMessage gameMessage = new ToGameMessage("", loadoutResult, false);

            //Cinematic Settings
            CinematicSettings cinematicSettings = new CinematicSettings
            {
                skipOnDoubleTap = false,
                nextSceneName = Framework.SCENENAME,
                nextSceneMessage = gameMessage
            };

            //Launch
            CinematicScene.LaunchCinematic("Cinematic Demo", cinematicSettings);
        }
    }
}