using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsSpawn : MonoBehaviour
{
    // проигрывание эффектов при подборе
    [SerializeField] private GameObject _destroyEffect;
    public void SpawnEffect()
    {
        GameObject _myParent = transform.parent.gameObject; // определение платформы на которой был алмаз,   
        Instantiate(_destroyEffect, transform.position, Quaternion.identity, _myParent.transform); // генерация эффекта на блоке, чтобы эффект двигался вместе с ним
    }
}
