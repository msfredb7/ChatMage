using CCC.Manager;
using UnityEngine.SceneManagement;

public class ToLoadoutMessage : SceneMessage
{
    private string levelScriptName;
    private LoadoutTab.LoadoutTab_Type startTab;

    public ToLoadoutMessage(string levelScriptName, LoadoutTab.LoadoutTab_Type startTab = LoadoutTab.LoadoutTab_Type.Car)
    {
        this.levelScriptName = levelScriptName;
        this.startTab = startTab;
    }

    public void OnLoaded(Scene scene)
    {
        Loadout loadOut = Scenes.FindRootObject<Loadout>(scene);
        loadOut.Init(levelScriptName, startTab);
    }

    public void OnOutroComplete()
    {

    }
}