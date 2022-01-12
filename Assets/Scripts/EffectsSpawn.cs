using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsSpawn : MonoBehaviour
{
    // ������������ �������� ��� �������
    [SerializeField] private GameObject _destroyEffect;
    public void SpawnEffect()
    {
        GameObject _myParent = transform.parent.gameObject; // ����������� ��������� �� ������� ��� �����,   
        Instantiate(_destroyEffect, transform.position, Quaternion.identity, _myParent.transform); // ��������� ������� �� �����, ����� ������ �������� ������ � ���
    }
}
