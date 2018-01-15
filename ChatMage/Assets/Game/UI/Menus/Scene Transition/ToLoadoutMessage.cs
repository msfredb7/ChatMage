
using UnityEngine.SceneManagement;
using LoadoutMenu;

public class ToLoadoutMessage : SceneMessage
{
    private string levelScriptName;
    private LoadoutTab startTab;

    public ToLoadoutMessage(string levelScriptName, LoadoutTab startTab = LoadoutTab.Car)
    {
        this.levelScriptName = levelScriptName;
        this.startTab = startTab;
    }

    public void OnLoaded(Scene scene)
    {
        LoadoutUI loadOut = Scenes.FindRootObject<LoadoutUI>(scene);
        loadOut.Init(levelScriptName, startTab);
    }

    public void OnOutroComplete()
    {

    }
}