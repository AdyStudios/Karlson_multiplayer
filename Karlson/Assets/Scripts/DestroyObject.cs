using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class DestroyObject : MonoBehaviour
{
	// Token: 0x06000018 RID: 24 RVA: 0x0000285E File Offset: 0x00000A5E
	private void Start()
	{
		base.Invoke("DestroySelf", this.time);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002871 File Offset: 0x00000A71
	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400000A RID: 10
	public float time;
}
