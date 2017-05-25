using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    public Mapping mapping; // limite de la map, waypoints, etc.

    [SerializeField]
    private List<GameObject> mapObjectsToAjust;

    [Header("Optional")]
    public RubanPlayer rubanPlayer;
    public MapFollower mapFollower;

    [Header("Debug")]
    public LevelScript defaultLevelScript;

    void Start()
    {
        if (Scenes.SceneCount() == 1)
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
        framework.Init(defaultLevelScript, null);
    }

    /// <summary>
    /// Initialise les settings de la map
    /// </summary>
	public void Init(float height, float width)
    {
        for (int i = 0; i < mapObjectsToAjust.Count; i++)
        {
            Adjust(mapObjectsToAjust[i]);
        }

        mapping.Init(height, width);

        // Ajustement de la map a faire en fonction du height et width
    }

    public void Adjust(GameObject obj)
    {
        if (obj != null)
            obj.transform.position = Game.instance.ConvertToRealPos(obj.transform.position);
    }
}
