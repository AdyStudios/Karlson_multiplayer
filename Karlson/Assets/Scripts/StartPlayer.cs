using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class StartPlayer : MonoBehaviour
{
	// Token: 0x060001B4 RID: 436 RVA: 0x0000ACB0 File Offset: 0x00008EB0
	private void Awake()
	{
		for (int i = base.transform.childCount - 1; i >= 0; i--)
		{
			MonoBehaviour.print("removing child: " + i);
			base.transform.GetChild(i).parent = null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
