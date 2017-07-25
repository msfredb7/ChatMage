using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher_LockOnAnim : MonoBehaviour
{
    [Header("Left Lazer")]
    public Transform leftLazer;
    public Transform rightLazer;
    public float fov;

    //-----------Interface-----------//

    public void UpdateAnimation(float relativeAngle, float progress)
    {
        SetLazersRelative(relativeAngle, progress);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }



    //-----------Animation-----------//

    private void SetLazersRelative(float angle, float progress)
    {
        float leftRot = fov.Lerpped(angle, progress);
        float rightRot = (-fov).Lerpped(angle, progress);

        leftLazer.localRotation = Quaternion.Euler(Vector3.forward * leftRot);
        rightLazer.localRotation = Quaternion.Euler(Vector3.forward * rightRot);
    }
}
