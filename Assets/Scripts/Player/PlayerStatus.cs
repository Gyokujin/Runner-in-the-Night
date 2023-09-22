using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private float rayDistance = 0.5f;
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

    private float jumpTime = 1f;
    public float JumpTime
    {
        get
        {
            return jumpTime;
        }
        set
        {
            if (value > 0)
            {
                jumpTime = value;
            }
        }
    }

    private float jumpCoolTime = 0.35f;
    public float JumpCoolTime
    {
        get
        {
            return jumpCoolTime;
        }
        set
        {
            if (value > 0)
            {
                jumpCoolTime = value;
            }
        }
    }

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