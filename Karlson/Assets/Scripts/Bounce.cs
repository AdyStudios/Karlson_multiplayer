using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class Bounce : MonoBehaviour
{
	// Token: 0x06000005 RID: 5 RVA: 0x00002160 File Offset: 0x00000360
	private void OnCollisionEnter(Collision other)
	{
		MonoBehaviour.print("yeet");
		other.gameObject.GetComponent<Rigidbody>();
	}
}
