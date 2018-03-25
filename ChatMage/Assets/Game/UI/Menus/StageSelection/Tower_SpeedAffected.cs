using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_SpeedAffected : MonoBehaviour
{
    [SerializeField] Tower_SpeedRecorder recorder;
    [SerializeField, Range(0, 1)] float paralaxRatio = 1;
    [SerializeField] Transform tr;
    [SerializeField] SpriteOffset spriteOffset;
    [SerializeField] bool moveTransform;
    [SerializeField] bool moveSpriteOffset;

    private float yScale = 1;

    private void Start()
    {
        yScale = tr.lossyScale.y;
    }

    void Update()
    {
        var move = recorder.WorldVelocity * paralaxRatio * Time.deltaTime;

        if (tr && moveTransform)
            tr.position += Vector3.up * move;

        if (spriteOffset && moveSpriteOffset)
            spriteOffset.Offset += Vector2.up * (-move / yScale);
    }
}
