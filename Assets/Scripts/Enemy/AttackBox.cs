using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    public enum AttackType
    {
        Obstacle, Bullet
    }

    public AttackType attackType;
}