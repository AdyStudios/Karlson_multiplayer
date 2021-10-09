using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class Lava : MonoBehaviour
{
	// Token: 0x06000063 RID: 99 RVA: 0x00003FFC File Offset: 0x000021FC
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			PlayerMovement.Instance.KillPlayer();
		}
	}
}
