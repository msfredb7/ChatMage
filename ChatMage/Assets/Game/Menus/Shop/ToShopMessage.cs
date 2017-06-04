using CCC.Manager;
using UnityEngine.SceneManagement;

public class ToShopMessage : SceneMessage
{
    private string previousSceneName;
    private LoadoutTab.LoadoutTab_Type startTab;

    public ToShopMessage(string previousSceneName, LoadoutTab.LoadoutTab_Type startTab = 0)
    {
        this.previousSceneName = previousSceneName;
        this.startTab = startTab;
    }

    public void OnLoaded(Scene scene)
    {
        ShopMenu shop = Scenes.FindRootObject<ShopMenu>(scene);
        shop.SetPreviousContext(previousSceneName, startTab);
    }

    public void OnOutroComplete()
    {

    }
}
