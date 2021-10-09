using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class MainCamera : MonoBehaviour
{
	// Token: 0x06000071 RID: 113 RVA: 0x000040AC File Offset: 0x000022AC
	private void Awake()
	{
		if (!SlowmoEffect.Instance)
		{
			return;
		}
		SlowmoEffect.Instance.NewScene(base.GetComponent<AudioLowPassFilter>(), base.GetComponent<AudioDistortionFilter>());
	}
}
