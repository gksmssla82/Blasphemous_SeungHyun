using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Colider : MonoBehaviour
{
    protected Enemy Enemy;
    protected virtual void Start()
    {
        Enemy = GetComponentInParent<Enemy>();
    }

    protected abstract void OnTriggerEnter2D(Collider2D collision);
    protected abstract void OnTriggerStay2D(Collider2D collision);
    protected abstract void OnTriggerExit2D(Collider2D collision);




}
