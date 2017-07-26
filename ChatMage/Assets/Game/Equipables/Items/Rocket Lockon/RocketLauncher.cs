using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    private const string LOCKON_KEY = "rLo"; // short pour Rocket LockOn
    private struct LockOn
    {
        public Unit target;
        public double startTime;
        public RocketLauncher_LockOnAnim anim;
        public double Elapsed(double internalClock)
        {
            return internalClock - startTime;
        }
    }

    [Header("Rocket Launcher")]
    public Rocket rocketPrefab;
    public int maxLockOnCount = 1000;
    public float lockOnDuration = 0.75f;
    [Forward]
    public Targets targets;
    public float fov;
    public RocketLauncher_LockOnAnim animItem;

    private List<RocketLauncher_LockOnAnim> anims = new List<RocketLauncher_LockOnAnim>();
    private LinkedList<LockOn> lockons = new LinkedList<LockOn>();
    private PlayerVehicle player;
    private Transform tr;
    private double internalClock;

    void Start()
    {
        tr = transform;
        player = Game.instance.Player.vehicle;
        internalClock = 0;

        animItem.SetInactive();
        anims.Add(animItem);
    }

    void Update()
    {
        //On process tous les lockons
        LinkedListNode<LockOn> node = lockons.First;
        while (node != null)
        {
            LockOn val = node.Value;
            LinkedListNode<LockOn> next = node.Next;

            //Prossess le lockon
            if (!ProssessLockOn(val))
            {
                //La cible est sortie du fov, on annule ce lockon
                lockons.Remove(node);
                val.target.marks.Remove(LOCKON_KEY);
            }

            node = next;
        }

        //Pouvons nous chercher de nouvelle cible ?
        if (lockons.Count < maxLockOnCount)
        {
            SearchForTarget();
        }

        internalClock += player.DeltaTime();
    }

    bool ProssessLockOn(LockOn lockon)
    {
        //La cible est-elle dans le fov ?
        float relativeAngle;
        if (IsUnitWithinFOV(lockon.target, out relativeAngle))
        {
            //La cible est dans le fov !
            double elapsed = lockon.Elapsed(internalClock);

            if (elapsed >= lockOnDuration)
            {
                //Launch Rocket !
                Game.instance.SpawnUnit(rocketPrefab, tr.position).Init(lockon.target.transform);

                //Terminate animation
                lockon.anim.SetInactive();
                return false;
            }
            else
            {
                //Update animation
                lockon.anim.UpdateAnimation(relativeAngle, (float)elapsed / lockOnDuration);
                return true;
            }
        }
        else
        {
            lockon.anim.SetInactive();
            return false;
        }
    }

    void SearchForTarget()
    {
        Vector2 myPos = tr.position;

        LinkedListNode<Unit> node = Game.instance.attackableUnits.First;
        while (node != null)
        {
            Unit val = node.Value;

            if (targets.IsValidTarget(val))
            {
                if (IsUnitWithinFOV(val) && !val.marks.Contains(LOCKON_KEY))
                {
                    //Nouvelle cible !
                    LockOn newLockOn = new LockOn {
                        target = val,
                        startTime = internalClock,
                        anim = GetNewAnim()
                    };
                    lockons.AddLast(newLockOn);
                    val.marks.Add(LOCKON_KEY);

                    return;
                }
            }

            node = node.Next;
        }
    }

    private bool IsUnitWithinFOV(Unit unit)
    {
        float bidon;
        return IsUnitWithinFOV(unit, out bidon);
    }
    private bool IsUnitWithinFOV(Unit unit, out float relativeAngle)
    {
        if (unit == null || unit.IsDead)
        {
            relativeAngle = 0;
            return false;
        }

        Vector2 myPos = tr.position;
        Vector2 deltaPos = unit.Position - myPos;
        float angle = deltaPos.ToAngle();
        relativeAngle = Mathf.DeltaAngle(player.Rotation, angle);

        return relativeAngle.Abs() <= fov;
    }

    private RocketLauncher_LockOnAnim GetNewAnim()
    {
        for (int i = 0; i < anims.Count; i++)
        {
            if (!anims[i].IsActive())
            {
                anims[i].SetActive();
                return anims[i];
            }
        }
        RocketLauncher_LockOnAnim newAnim = Instantiate(animItem.gameObject, tr).GetComponent<RocketLauncher_LockOnAnim>();
        anims.Add(newAnim);
        return newAnim;
    }
}
