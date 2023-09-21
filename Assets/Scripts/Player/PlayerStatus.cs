using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    private float rayDistance = 1.25f;
    public float RayDistance
    {
        get
        {
            return rayDistance;
        }
        set
        {
            if (value > 0)
            {
                rayDistance = value;
            }
        }
    }

    [SerializeField]
    private float jumpFoece = 420f;
    public float JumpFoce
    {
        get
        {
            return jumpFoece;
        }
        set
        {
            if (value > 0)
            {
                jumpFoece = value;
            }
        }
    }

    [SerializeField]
    private float slideCoolTime = 3f;
    public float SlideCoolTime
    {
        get
        {
            return slideCoolTime;
        }
        set
        {
            if (value > 0)
            {
                slideCoolTime = value;
            }
        }
    }

    [SerializeField]
    private int maxLife = 5;
    public int MaxLife
    {
        get
        {
            return maxLife;
        }
        set
        {
            if (value > 0)
            {
                maxLife = value;
            }
        }
    }

    [SerializeField]
    private float knockbackForce = 250f;
    public float KnockbackForce
    {
        get
        {
            return knockbackForce;
        }
        set
        {
            if (value > 0)
            {
                knockbackForce = value;
            }
        }
    }

    [SerializeField]
    private float hitTime = 2f;
    public float HitTime
    {
        get
        {
            return hitTime;
        }
        set
        {
            if (value > 0)
            {
                hitTime = value;
            }
        }
    }

    [SerializeField]
    private float invincibleTime = 3f;
    public float InvincibleTime
    {
        get
        {
            return invincibleTime;
        }
        set
        {
            if (value > 0)
            {
                invincibleTime = value;
            }
        }
    }
}