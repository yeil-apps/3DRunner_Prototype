using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Gem : MonoBehaviour
{
    public int GemsCountInObj
    {
        get { return _gemsCountInObj; }
    }
    [SerializeField] private int _gemsCountInObj = 1; // количество алмазов в одном объекте
}
