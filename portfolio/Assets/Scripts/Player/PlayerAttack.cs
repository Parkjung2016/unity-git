using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator anim;
    bool combopossible;
    int ComboStep;
    PlayerEquip playerEquip_;
    public bool attacking;
    PlayerWeapon playerWeapon_;
    public bool FinalAttackCol;
    private void Awake()
    {
        playerWeapon_ = GetComponentInChildren<PlayerWeapon>();
        playerEquip_ = GetComponent<PlayerEquip>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        FinalAttackCol = true;
    }
    private void Update()
    {
        if (PlayerHPManager.Instance.death)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && playerEquip_.sword_is_equipped)
        {
            Attack();
        }
    }
  
    public void Attack()
    {
        attacking = true;
        anim.applyRootMotion = true;
        if (ComboStep == 0)
        {
            anim.Play("AttackA");
            LookCameraDir();
            playerWeapon_.Col.enabled = true;
            ComboStep = 1;
            return;
        }
        if (ComboStep != 0)
        {
            if (combopossible)
            {
                combopossible = false;
                ComboStep++;
                LookCameraDir();
                playerWeapon_.Col.enabled = true;
            }
        }
    }
    public void ComboPossible()
    {
        combopossible = true;
    }
    public void Combo()
    {
        switch (ComboStep)
        {

            case 2:
                anim.Play("AttackB");
                break;
            case 3:
                anim.Play("AttackC");
                break;
            case 4:
                anim.Play("AttackD");
                break;
            case 5:
                anim.Play("AttackCombo1");
                break;

        }
    }
    public void LookCameraDir()
    {
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
    }
    public void ComboReset()
    {
        FinalAttackCol = true;
        playerWeapon_.Col.enabled = false;
        anim.applyRootMotion = false;
        combopossible = false;
        ComboStep = 0;
        attacking = false;
    }
}


