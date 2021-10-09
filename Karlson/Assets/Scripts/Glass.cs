using System;
using EZCameraShake;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class Glass : MonoBehaviour
{
	// Token: 0x06000051 RID: 81 RVA: 0x000034B0 File Offset: 0x000016B0
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
		{
			UnityEngine.Object.Instantiate<GameObject>(this.glassSfx, base.transform.position, Quaternion.identity);
			this.glass.SetActive(true);
			this.glass.transform.parent = null;
			this.glass.transform.localScale = Vector3.one;
			UnityEngine.Object.Destroy(base.gameObject);
			if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			{
				PlayerMovement.Instance.Slowmo(0.3f, 1f);
			}
			CameraShaker.Instance.ShakeOnce(5f, 3.5f, 0.3f, 0.2f);
		}
	}

	// Token: 0x04000029 RID: 41
	public GameObject glass;

	// Token: 0x0400002A RID: 42
	public GameObject glassSfx;
}
