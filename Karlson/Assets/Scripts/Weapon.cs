using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
public abstract class Weapon : Pickup
{
	// Token: 0x17000017 RID: 23
	// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000AAB6 File Offset: 0x00008CB6
	// (set) Token: 0x060001A9 RID: 425 RVA: 0x0000AABE File Offset: 0x00008CBE
	public float MultiplierDamage { get; set; }

	// Token: 0x060001AA RID: 426 RVA: 0x0000AAC7 File Offset: 0x00008CC7
	public void Start()
	{
		this.MultiplierDamage = 1f;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0000AA24 File Offset: 0x00008C24
	protected void Cooldown()
	{
		base.readyToUse = true;
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000AAD4 File Offset: 0x00008CD4
	public float GetAttackSpeed()
	{
		return this.attackSpeed;
	}

	// Token: 0x04000179 RID: 377
	public float attackSpeed;

	// Token: 0x0400017A RID: 378
	public float damage;

	// Token: 0x0400017C RID: 380
	public TrailRenderer trailRenderer;
}
