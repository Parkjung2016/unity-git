using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public int Damage;
    public Collider Col;
    private void Awake()
    {
        Col = GetComponent<Collider>();
    }
    private void Start()
    {
        Col.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            Col.enabled = false;
            if (BossHPManager.Instance.Death)
                return;
            GameManager.instace.StartCoroutine(GameManager.CameraShake(GetComponentInParent<Animator>(), other.GetComponent<Animator>(),GameManager.instace.ShakePower));
            BossHPManager.ApplyDamage(Damage);
        }

    }
    
}
