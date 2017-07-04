using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BouclierTournant : MonoBehaviour {

    public SimpleColliderListener shieldCollider;
    public float animationDuration = 3;

    private enum RotationPosition { up = 0, right = 1, bottom = 2, left = 3 }

    private RotationPosition currentRotationPosition;
    private bool curentlyRotating;

	void Start ()
    {
        curentlyRotating = false;
        RotateTo(RotationPosition.up);
        shieldCollider.onTriggerEnter += ShieldCollider_onTriggerEnter;
    }

    private void ShieldCollider_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if(other.GetComponent<IAttackable>() != null)
        {
            other.GetComponent<IAttackable>().Attacked(other, 1, null);
            UnitKilled();
        } else
        {
            if (other.parentUnit.allegiance == Allegiance.Enemy)
            {
                Destroy(other.gameObject);
                UnitKilled();
            }
        }
    }

    void UnitKilled()
    {
        if (!curentlyRotating)
        {
            curentlyRotating = true;
            switch (currentRotationPosition)
            {
                case RotationPosition.up:
                    RotateTo(RotationPosition.right);
                    break;
                case RotationPosition.right:
                    RotateTo(RotationPosition.bottom);
                    break;
                case RotationPosition.bottom:
                    RotateTo(RotationPosition.left);
                    break;
                case RotationPosition.left:
                    RotateTo(RotationPosition.up);
                    break;
                default:
                    break;
            }
        }
    }

    void RotateTo(RotationPosition position)
    {
        switch (position)
        {
            case RotationPosition.up:
                currentRotationPosition = RotationPosition.up;
                transform.DOLocalRotate(new Vector3(0, 0, -90), animationDuration).OnComplete(delegate ()
                {
                    curentlyRotating = false;
                });
                break;
            case RotationPosition.right:
                currentRotationPosition = RotationPosition.right;
                transform.DOLocalRotate(new Vector3(0, 0, -180), animationDuration).OnComplete(delegate ()
                {
                    curentlyRotating = false;
                });
                break;
            case RotationPosition.bottom:
                currentRotationPosition = RotationPosition.bottom;
                transform.DOLocalRotate(new Vector3(0, 0, 90), animationDuration).OnComplete(delegate ()
                {
                    curentlyRotating = false;
                });
                break;
            case RotationPosition.left:
                currentRotationPosition = RotationPosition.left;
                transform.DOLocalRotate(new Vector3(0, 0, 0), animationDuration).OnComplete(delegate ()
                {
                    curentlyRotating = false;
                });
                break;
            default:
                break;
        }
    }
}
