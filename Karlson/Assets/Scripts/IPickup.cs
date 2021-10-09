using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
public interface IPickup
{
	// Token: 0x0600018E RID: 398
	void Use(Vector3 attackDirection);

	// Token: 0x0600018F RID: 399
	bool IsPickedUp();
}
