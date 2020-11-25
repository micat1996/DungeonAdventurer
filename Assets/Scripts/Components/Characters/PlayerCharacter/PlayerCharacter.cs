using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCharacterMovement))]
public sealed class PlayerCharacter : MonoBehaviour
{
	[SerializeField] private ZoomableSpringArm _SpringArm;
	[SerializeField] private PlayerInteraction _Interaction;
	[SerializeField] private PlayerInventory _PlayerInventory;

	public PlayerCharacterMovement playerCharacterMovement { get; private set; }

	public ZoomableSpringArm springArm => _SpringArm;
	public PlayerInteraction interaction => _Interaction;
	public PlayerInventory inventory => _PlayerInventory;


	private void Awake()
	{
		playerCharacterMovement = GetComponent<PlayerCharacterMovement>();
	}
}
