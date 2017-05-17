using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour {

    [SerializeField]
    private MapInfo mapInfo; // information a propos de la map
    public Mapping map; // limite de la map, waypoints, etc.
    [SerializeField]
    private Playground playground; // Zone jouable

    [SerializeField]
    private List<GameObject> mapObjects;

    public LevelScript defaultLevelScript;

    void Start()
    {
        if(Scenes.SceneCount() == 1)
        {
            MasterManager.Sync(delegate ()
            {
                Scenes.Load("Framework", LoadSceneMode.Additive, DebugInit);
            });
        }
    }

    void DebugInit(Scene scene)
    {
        Framework framework = Scenes.FindRootObject<Framework>(scene);
        framework.Init(defaultLevelScript);
    }

    /// <summary>
    /// Initialise les settings de la map
    /// </summary>
	public void Init (float height, float width) {
        for(int i = 0; i < mapObjects.Count; i++)
        {
            Adjust(mapObjects[i]);
        }

        map.Init(height, width);

        // On va chercher le playground et on set ca dans mapping
        map.SetOffsets(playground.GetTopLimit(),
                       playground.GetBottomLimit(),
                       playground.GetRightLimit(),
                       playground.GetLeftLimit());

        // Ajustement de la map a faire en fonction du height et width
	}

    public void Adjust(GameObject obj)
    {
        if(obj != null)
            obj.transform.position = Game.instance.ConvertToRealPos(obj.transform.position);
    }
}
