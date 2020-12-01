using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCharacterMovement))]
public sealed class PlayerCharacter : MonoBehaviour
{
	[SerializeField] private ZoomableSpringArm _SpringArm;
	[SerializeField] private PlayerInteraction _Interaction;
	[SerializeField] private PlayerInventory _PlayerInventory;
	[SerializeField] private PlayerCharacterEquipSockets _EquipSockets;
	[SerializeField] private PlayerAttack _PlayerAttack;
	[SerializeField] private PlayerCharacterAnimator _AnimatorController;

	public PlayerCharacterMovement playerCharacterMovement { get; private set; }

	public ZoomableSpringArm springArm => _SpringArm;
	public PlayerInteraction interaction => _Interaction;
	public PlayerInventory inventory => _PlayerInventory;
	public PlayerCharacterEquipSockets equipSockets => _EquipSockets;
	public PlayerAttack playerAttack => _PlayerAttack;
	public PlayerCharacterAnimator animatorController => _AnimatorController;


	private void Awake()
	{
		playerCharacterMovement = GetComponent<PlayerCharacterMovement>();
	}
}
