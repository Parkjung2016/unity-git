using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
	public Transform sword, sword_ueq, sword_eq;
	public bool sword_is_equipped;
	PlayerAttack playerAttack_;
	Animator anim;
	PlayerRifleEquip playerRifleEquip;

	void Awake()
	{
		playerRifleEquip = GetComponent<PlayerRifleEquip>();
		playerAttack_ = GetComponent<PlayerAttack>();
		anim = GetComponent<Animator>();
	}
	void Update()
	{
		if(PlayerHPManager.Instance.death)
        {
			return;
        }
		if (Input.GetKeyDown(KeyCode.Alpha1) &&!playerAttack_.attacking)
        {
			if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Wait") || anim.GetCurrentAnimatorStateInfo(1).IsTag("Wait")
				|| anim.GetCurrentAnimatorStateInfo(2).IsTag("Wait"))
				return;
			anim.SetBool("Sword",!anim.GetBool("Sword"));
			if(anim.GetBool("Shield"))
            {
			anim.SetTrigger("sword_i");
            }
			else
            {
				playerRifleEquip.Aim = false;
				playerRifleEquip.CrossHair.SetActive(false);
				playerRifleEquip.Cam.SetActive(true);
				playerRifleEquip.Cam_Rifle.SetActive(false);
				playerRifleEquip.rigbuilder.layers[1].active = false;
				anim.SetTrigger("Rifle_i");
            }
        }

		if (sword_is_equipped)
		{
			sword.position = sword_eq.position;
			sword.rotation = sword_eq.rotation;
		}
		else
		{
			sword.position = sword_ueq.position;
			sword.rotation = sword_ueq.rotation;
		}
	}
	public void Sword_Equip()
	{
		sword_is_equipped = true;
		anim.ResetTrigger("sword_i");
	}
	public void Sword_Unequiped()
	{
		anim.ResetTrigger("sword_i");
		sword_is_equipped = false;
	}
}
