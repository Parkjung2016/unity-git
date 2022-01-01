using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
	public Transform sword, sword_ueq, sword_eq;
	public bool sword_is_equipped;
	PlayerAttack playerAttack_;
	Animator anim;

	void Awake()
	{
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

			anim.SetTrigger("sword_i");
			anim.SetBool("Sword",!anim.GetBool("Sword"));
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
	}
	public void Sword_Unequiped()
	{
		sword_is_equipped = false;
	}
}
