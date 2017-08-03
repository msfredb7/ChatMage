using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseCinematicSettings : SceneMessage
{
    public bool skipOnDoubleTap;

    private BaseCinematicScene cin;

    public void OnLoaded(Scene scene)
    {
        //Deliver cinematic settings
        cin = scene.FindRootObject<BaseCinematicScene>();

        if (cin != null)
            cin.ApplySettings(this);
        else
            Debug.LogError("No root gameobject of Type BaseCinematicScene in " + scene.name + " scene.");

    }

    public void OnOutroComplete()
    {
        cin.OnArrivalComplete();

        cin = null;
    }
}
