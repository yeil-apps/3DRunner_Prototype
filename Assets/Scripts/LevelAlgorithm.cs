using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Algorithm", menuName = "Algorithm")]
public class LevelAlgorithm : ScriptableObject
{
    public List<GameObject> LevelPrefabs; // блоки для спавна на уровне 
    public GameObject FirstBlock; // первый блок уровня
}

