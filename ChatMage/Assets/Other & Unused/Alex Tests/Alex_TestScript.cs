using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alex_TestScript : MonoBehaviour {

    public int number = 15;

	void Start ()
    {
        FindIncreasingPartSum(number);
    }

    private List<int> FindIncreasingPartSum(int number)
    {
        if (number <= 0)
            return null;

        List<int> result = new List<int>();

        // Trouver une liste qui fonctionne
        int sum = 1; // minimum de 1
        result.Add(1);
        for (int i = 1; sum != number; i++)
        {
            sum += i;
            if (sum > number)
            {
                sum -= i;
                i = 0;
                continue;
            }
            else
            {
                result.Add(i);
            }
        }

        // Optimiser la liste
        for (int i = 0; i+1 < result.Count; i++)
        {
            // Si l'element courrant est plus grand que le suivant, on doit optimiser
            if (result[i] > result[i+1]) {
                // On est deja la fin de la liste
                if(i+2 > result.Count)
                {
                    // on ajoute le dernier terme a l'avant dernier et on quitte
                    result[i] += result[i + 1];
                    result.RemoveAt(i + 1);
                    break;
                } else
                {
                    // Sinon, on optimise tout les elements suivant
                    for (int j = i + 1; j + 1 < result.Count; j++)
                    {
                        // Si le terme suivant de celui qu'on optimise est plus petit que le terme courrant, on optimise
                        if (result[i] > result[j + 1])
                        {
                            // on concatene les deux termes suivant en un seul terme
                            result[i + 1] += result[j + 1];
                            // On l'elime
                            result.RemoveAt(j + 1);
                            // On continue tant qu'il n'y a plus d'autres suivants
                            j--;
                            continue;
                        }
                    }
                    // On a fini d'optimiser la fin de la liste en un seul terme
                    // Ce dernier terme a besoin d'etre optimiser egalement ?
                    if (result[i] > result[i + 1])
                    {
                        // on ajoute le dernier terme a l'avant dernier et on quitte
                        result[i] += result[i + 1];
                        result.RemoveAt(i + 1);
                        break;
                    }
                }
            }
        }
        //result.Print();
        return result;
    }
}
