using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerAttack : MonoBehaviour
{
	private PlayerInventory _Inventory;
	private PlayerCharacterAnimator _AnimatorController;

	private void Awake()
	{
		_Inventory = GetComponent<PlayerInventory>();
		_AnimatorController = PlayerManager.Instance.playerCharacter.animatorController;
	}

	private void Update()
	{
		if (_Inventory.equipItems[ItemType.Sword].isEmpty) 

		if (InputManager.Instance.gameInputRegularAttack)
			RegularAttackStart();
	}

	private void RegularAttackStart()
	{

	}



}
