using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainChomp_ChainSpawner : MonoBehaviour
{
    [Header("Chains")]
    public Transform chainContainer;
    public ChainChomp_Link chain_Prefab;

    [Header("Other")]
    public Rigidbody2D anchor;
    public ChainChomp_Link ball;

    List<ChainChomp_Link> chains = new List<ChainChomp_Link>();

    void ClearVelocities()
    {
        for (int i = 0; i < chains.Count; i++)
        {
            chains[i].KillVelocity();
        }
    }

    public int ChainCount
    {
        get { return chains.Count; }
    }

    public void SpawnChains(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnChain();
        }
    }
    public void SpawnChain()
    {
        ChainChomp_Link newLink = chain_Prefab.DuplicateGO(chainContainer);
        newLink.SetVisuals(chains.Count.IsEvenNumber() ? 0 : 1);
        chains.Add(newLink);
        ConfigureChain(chains.Count - 1);
    }

    public ChainChomp_Link GetChain(int index)
    {
        if (index < 0 || index >= chains.Count)
            return null;
        return chains[index];
    }

    public void RemoveChain()
    {
        if (chains.Count == 0)
            return;

        chains.Last().DestroyGO();
        chains.RemoveLast();
        if (chains.Count > 0)
        {
            ConfigureChain(chains.Count - 1);
        }
        else
        {
            ball.SetNextJoint(anchor, null);
        }
    }
    public void BreakOffChains(int amount, Vector2 inherentVelocity)
    {
        amount = amount.Capped(chains.Count);
        if (amount == 0)
            return;


        for (int i = chains.Count - amount; i < chains.Count; i++)
        {
            chains[i].BreakOff(inherentVelocity);
        }
        chains.RemoveRange(chains.Count - amount, amount);

        if (chains.Count > 0)
        {
            anchor.transform.position = chains.Last().TransformedNextAnchor();
            ConfigureChain(chains.Count - 1);
        }
        else
        {
            anchor.transform.position = ball.TransformedNextAnchor();
            ball.SetNextJoint(anchor, null);
        }
    }
    public void CutChainsAt(int index)
    {
        index = index.Capped(chains.Count - 2);

        var chainA = GetChain(index);
        var chainB = GetChain(index + 1);
        chainA.SetNextJoint(null);
        chainB.SetPreviousJoint(null);

        //Add twist force
        var force = (chainA.rb.position - chainB.rb.position).Rotate(90).normalized * chainA.rb.mass;
        chainA.rb.AddForce(force * 4, ForceMode2D.Impulse);
        chainB.rb.AddForce(force * -4, ForceMode2D.Impulse);
    }

    private void ConfigureChain(int at)
    {
        ChainChomp_Link next = GetNextLink(at);
        ChainChomp_Link previous = GetPreviousLink(at);

        if (previous == null)
            chains[at].SetPreviousJoint(ball.rb, ball);
        else
        {
            chains[at].SetPreviousJoint(previous.rb, previous);
            previous.SetNextJoint(chains[at].rb, chains[at]);
        }

        if (next == null)
            chains[at].SetNextJoint(anchor, null);
        else
        {
            chains[at].SetNextJoint(next.rb, next);
            next.SetPreviousJoint(chains[at].rb, chains[at]);
        }

        chains[at].RemoveStrain();
    }

    private ChainChomp_Link GetPreviousLink(int from)
    {
        if (from == 0)
            return ball;

        return chains[from - 1];
    }

    private ChainChomp_Link GetNextLink(int from)
    {
        if (from >= chains.Count - 1)
            return null;

        return chains[from + 1];
    }
}
