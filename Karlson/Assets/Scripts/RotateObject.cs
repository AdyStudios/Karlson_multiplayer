using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class RotateObject : MonoBehaviour
{
	// Token: 0x06000120 RID: 288 RVA: 0x00008790 File Offset: 0x00006990
	private void Update()
	{
		base.transform.Rotate(Vector3.right, 40f * Time.deltaTime);
	}
}
