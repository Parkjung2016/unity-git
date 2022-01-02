using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRifleEquip : MonoBehaviour
{
    public Transform Rifle, Rifle_ueq, Rifle_eq;
    public bool Rifle_is_equipped;
    PlayerAttack playerAttack_;
    Animator anim;
	void Update()
	{
		if (PlayerHPManager.Instance.death)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) && !playerAttack_.attacking)
		{
			anim.SetTrigger("Rifle_i");
		}

		if (Rifle_is_equipped)
		{
			Rifle.position = Rifle_eq.position;
			Rifle.rotation = Rifle_eq.rotation;
		}
		else
		{
			Rifle.position = Rifle_ueq.position;
			Rifle.rotation = Rifle_ueq.rotation;
		}
	}
	public void Rifle_Equip()
	{
		Rifle_is_equipped = true;
	}
	public void Rifle_Unequiped()
	{
		Rifle_is_equipped = false;
	}
}
