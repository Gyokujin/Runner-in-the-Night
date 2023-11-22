using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private float rayDistance = 0.4f;
    public float RayDistance
    {
        get
        {
            return rayDistance;
        }
        set
        {
            rayDistance = value;
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
            jumpFoece = value;
        }
    }

    private float jumpTime = 0.35f;
    public float JumpTime
    {
        get
        {
            return jumpTime;
        }
        set
        {
            jumpTime = value;
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
            jumpCoolTime = value;
        }
    }

    private float minGravity = 0.75f;
    public float MinGravity
    {
        get
        {
            return minGravity;
        }
        set
        {
            minGravity = value;
        }
    }

    private float maxGravity = 1.1f;
    public float MaxGravity
    {
        get
        {
            return maxGravity;
        }
        set
        {
            maxGravity = value;
        }
    }

    private float slideTime = 0.55f;
    public float SlideTime
    {
        get
        {
            return slideTime;
        }
        set
        {
            slideTime = value;
        }
    }

    private float slideCoolTime = 2.95f;
    public float SlideCoolTime
    {
        get
        {
            return slideCoolTime;
        }
        set
        {
            slideCoolTime = value;
        }
    }

    private float attackCoolTime = 0.24f;
    public float AttackCoolTime
    {
        get
        {
            return attackCoolTime;
        }
        set
        {
            AttackCoolTime = value;
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
            HitTime = value;
        }
    }

    private float putGunTime = 0.6f;
    public float PutGunTime
    {
        get
        {
            return putGunTime;
        }
        set
        {
            PutGunTime = value;
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
            maxLife = value;
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
            knockbackForce = value;
        }
    }

    private float bounceForce = 850f;
    public float BounceForce
    {
        get
        {
            return bounceForce;
        }
        set
        {
            bounceForce = value;
        }
    }

    private float invincibleTime = 3.5f;
    public float InvincibleTime
    {
        get
        {
            return invincibleTime;
        }
        set
        {
            invincibleTime = value;
        }
    }

    private float respawnTime = 0.1f;
    public float RespawnTime
    {
        get
        {
            return respawnTime;
        }
        set
        {
            respawnTime = value;
        }
    }
}