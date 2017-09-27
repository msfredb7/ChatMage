using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.Manager;

public class RoadMapPoint : MonoBehaviour {

    // Canvas pour spawn
    public Canvas canvas;

    // Structure de la map
    public RoadMapPoint pointSuivant;

    // Animation de la map
    public GameObject dotSpritePrefab;
    [HideInInspector]
    public List<GameObject> dotList = new List<GameObject>(); // Utile pour apres faire avancer le bonhomme entre les points
    public float roadIntensity = 5;
    public AnimationCurve curve;
    public float timeBetweenDots = 1;
    public float dotDensity = 50;

    // Info Anim
    private int dotCount = 0;
    private Vector3 vectorDeplacement;
    private float pathLenght;
    private Vector3 vectorBetween;

    // Use this for initialization
    void Start ()
    {
        MasterManager.Sync();

        roadIntensity = (roadIntensity * Screen.width) / 1920;
    }

    public void StartRoad()
    {
        // Si ya pas de points suivant, ben pourquoi on y va ?
        if (pointSuivant == null)
            return;

        Vector2 adjustFactor;

        adjustFactor.x = Screen.width / 1920;
        adjustFactor.y = Screen.height / 1080;

        // Calculate data for dots anims
        vectorBetween = pointSuivant.transform.position - transform.position;
        pathLenght = vectorBetween.magnitude;
        vectorDeplacement = vectorBetween / Mathf.Floor(pathLenght / dotDensity);

        // Add first dot
        dotList.Add(Instantiate(dotSpritePrefab, transform.position, Quaternion.identity, canvas.transform));
        dotCount++;
        DelayManager.LocalCallTo(MakeRoad, 1, this);
    }
	
	void MakeRoad()
    {
        dotList.Add(Instantiate(dotSpritePrefab, ApplyCurveOnVecPos((transform.position + (vectorDeplacement * dotCount))), Quaternion.identity, canvas.transform));
        dotCount++;
        if (dotList[dotList.Count - 1].transform.position == pointSuivant.transform.position)
        {
            Debug.Log("Finito");
            return;
        } 
        DelayManager.LocalCallTo(MakeRoad, 1, this);
    }

    Vector3 ApplyCurveOnVecPos(Vector2 currentPos)
    {
        Vector3 currentPosV3 = new Vector3(currentPos.x, currentPos.y, 0); // Position actuel
        float lenghtFromStart = (currentPosV3 - transform.position).magnitude; 
        Vector3 perpendicularVec = Vector3.Cross(vectorBetween, Vector3.forward).normalized;
        Vector3 modifyingFactor = - 1 *(perpendicularVec * (curve.Evaluate(lenghtFromStart / pathLenght) * roadIntensity));
        return currentPosV3 + modifyingFactor;
    }
}
