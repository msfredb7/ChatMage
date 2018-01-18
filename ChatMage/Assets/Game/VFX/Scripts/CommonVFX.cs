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
    private GameParticleEffect RedHitPrefab;
    private List<GameParticleEffect> RedHits = new List<GameParticleEffect>();

    public void HitRed(Vector2 position, MultiSize.Size size = MultiSize.Size.Small)
    {
        StandardVFX(position, size, RedHits, RedHitPrefab);
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
        vfx.Activate();
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
