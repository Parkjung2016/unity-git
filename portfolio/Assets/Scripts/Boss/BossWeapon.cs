using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public int Damage;
    public Collider col;
    private void Awake()
    {
        col = GetComponent<Collider>();
    }
    private void Start()
    {
        col.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHPManager.ApplyDamage(Damage,false);
            col.enabled = false;
        }
    }
}
