using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFromParticlesRemote : TaggedObject
{
    public PortalVFX portalVFX;
    public new ParticleSystem particleSystem;
    public WindZone windZone;
    public JesusV2Vehicle jesus;
    public GameObject topCollider;
    public SimpleColliderListener winCollider;
    public GameObject topWallVisuals;
}
