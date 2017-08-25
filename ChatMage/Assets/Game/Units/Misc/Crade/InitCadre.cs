using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCadre : MonoBehaviour {

    public GameObject cadrePrefab; 

	// Use this for initialization
	void Start () {
        Game.instance.onGameStarted += delegate ()
        {
            TaggedObject cadreTagged = Instantiate(cadrePrefab).GetComponent<TaggedObject>();
            List<TaggedObject> ourTaggedObject = new List<TaggedObject>();
            ourTaggedObject.Add(cadreTagged);
            Game.instance.map.mapping.taggedObjects.Add(cadreTagged.tags[0], ourTaggedObject);
        };
	}
}
