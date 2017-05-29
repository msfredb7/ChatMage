using CCC.Manager;
using UnityEngine.SceneManagement;

public class ToLoadoutMessage : SceneMessage
{
    private string levelScriptName;

    public ToLoadoutMessage(string levelScriptName)
    {
        this.levelScriptName = levelScriptName;
    }

    public void OnLoaded(Scene scene)
    {
        Loadout loadOut = Scenes.FindRootObject<Loadout>(scene);
        loadOut.Init(levelScriptName);
    }

    public void OnOutroComplete()
    {

    }
}