using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class PrefabManager : MonoBehaviour
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000124 RID: 292 RVA: 0x00008819 File Offset: 0x00006A19
	// (set) Token: 0x06000125 RID: 293 RVA: 0x00008820 File Offset: 0x00006A20
	public static PrefabManager Instance { get; private set; }

	// Token: 0x06000126 RID: 294 RVA: 0x00008828 File Offset: 0x00006A28
	private void Awake()
	{
		PrefabManager.Instance = this;
	}

	// Token: 0x0400010B RID: 267
	public GameObject blood;

	// Token: 0x0400010C RID: 268
	public GameObject bulletDestroy;

	// Token: 0x0400010D RID: 269
	public GameObject muzzle;

	// Token: 0x0400010E RID: 270
	public GameObject explosion;

	// Token: 0x0400010F RID: 271
	public GameObject bulletHitAudio;

	// Token: 0x04000110 RID: 272
	public GameObject enemyHitAudio;

	// Token: 0x04000111 RID: 273
	public GameObject gunShotAudio;

	// Token: 0x04000112 RID: 274
	public GameObject objectImpactAudio;

	// Token: 0x04000113 RID: 275
	public GameObject thumpAudio;

	// Token: 0x04000114 RID: 276
	public GameObject destructionAudio;
}
