using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Animator anim;
    [HideInInspector]
    public bool dead;

    public virtual void Death()
    {
        dead = true;
    }
}
