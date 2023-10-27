using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : MonoBehaviour
{
    // Este método será implementado por las clases derivadas (como Sword)
    public abstract void PerformAttack();
}
