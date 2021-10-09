using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class Milk : MonoBehaviour
{
	// Token: 0x0600007E RID: 126 RVA: 0x00004310 File Offset: 0x00002510
	private void Update()
	{
		float z = Mathf.PingPong(Time.time, 1f);
		Vector3 axis = new Vector3(1f, 1f, z);
		base.transform.Rotate(axis, 0.5f);
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00004350 File Offset: 0x00002550
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (PlayerMovement.Instance.IsDead())
			{
				return;
			}
			Game.Instance.Win();
			MonoBehaviour.print("Player won");
		}
	}
}
