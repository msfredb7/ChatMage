using CCC.Manager;
using UnityEngine.SceneManagement;

public class ToLoadoutMessage : SceneMessage
{
    private LevelScript chosenLevel;

    public ToLoadoutMessage(LevelScript level)
    {
        chosenLevel = level;
    }

    public void OnLoaded(Scene scene)
    {
        Loadout loadOut = Scenes.FindRootObject<Loadout>(scene);
        loadOut.Init(chosenLevel);
    }

    public void OnOutroComplete()
    {

    }
}