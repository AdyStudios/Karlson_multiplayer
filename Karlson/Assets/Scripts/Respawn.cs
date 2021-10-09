using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class Respawn : MonoBehaviour
{
	// Token: 0x06000122 RID: 290 RVA: 0x000087B0 File Offset: 0x000069B0
	private void OnTriggerEnter(Collider other)
	{
		MonoBehaviour.print(other.gameObject.layer);
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			Transform root = other.transform.root;
			root.transform.position = this.respawnPoint.position;
			root.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	// Token: 0x0400010A RID: 266
	public Transform respawnPoint;
}
