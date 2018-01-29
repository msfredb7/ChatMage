using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonVFX : MonoBehaviour
{
    //Nouveau
    [SerializeField]
    private GameParticleEffect WhiteHitPrefab;
    private List<GameParticleEffect> WhiteHits = new List<GameParticleEffect>();

    [SerializeField]
    private GameParticleEffect RedHit3Prefab;
    private List<GameParticleEffect> RedHits3 = new List<GameParticleEffect>();

    [SerializeField]
    private GameParticleEffect RedHit1Prefab;
    private List<GameParticleEffect> RedHits1 = new List<GameParticleEffect>();

    public void HitRed3(Vector2 position, MultiSize.Size size = MultiSize.Size.Small)
    {
        StandardVFX(position, size, RedHits3, RedHit3Prefab);
    }
    public void HitRed1(Vector2 position, MultiSize.Size size = MultiSize.Size.Small)
    {
        StandardVFX(position, size, RedHits1, RedHit1Prefab);
    }

    public void HitWhite(Vector2 position, MultiSize.Size size = MultiSize.Size.Small)
    {
        StandardVFX(position, size, WhiteHits, WhiteHitPrefab);
    }

    private void StandardVFX(Vector2 position, MultiSize.Size size, List<GameParticleEffect> list, GameParticleEffect prefab)
    {
        GameParticleEffect vfx = GetVFX(list, prefab);

        var multiSize = vfx.GetComponent<MultiSize>();
        if (multiSize != null)
            multiSize.SetSize(size);

        vfx.MoveTo(position);
        vfx.Play();
    }

    private GameParticleEffect GetVFX(List<GameParticleEffect> list, GameParticleEffect prefabReference)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].IsPlaying)
                return list[i];
        }

        //Spawn new
        var newInstance = prefabReference.DuplicateGO(Game.Instance.unitsContainer);
        list.Add(newInstance);

        return newInstance;
    }
}
