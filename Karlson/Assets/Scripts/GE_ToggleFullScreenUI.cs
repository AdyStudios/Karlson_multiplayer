using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000E RID: 14
public class GE_ToggleFullScreenUI : MonoBehaviour
{
	// Token: 0x06000049 RID: 73 RVA: 0x0000331C File Offset: 0x0000151C
	private void Start()
	{
		this.m_DefWidth = Screen.width;
		this.m_DefHeight = Screen.height;
		if (!Application.isEditor)
		{
			if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
			{
				base.gameObject.SetActive(true);
				return;
			}
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00003381 File Offset: 0x00001581
	private void Update()
	{
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00003384 File Offset: 0x00001584
	public void OnButton_ToggleFullScreen()
	{
		if (Application.isEditor)
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			base.gameObject.GetComponent<Button>().interactable = false;
			/*using (IEnumerator enumerator = base.transform.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					((Transform)obj).gameObject.SetActive(true);
				}
				return;
			}
			*/
		}
		Screen.fullScreen = !Screen.fullScreen;
		if (!Screen.fullScreen)
		{
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
			return;
		}
		Screen.SetResolution(this.m_DefWidth, this.m_DefHeight, false);
	}

	// Token: 0x04000025 RID: 37
	private int m_DefWidth;

	// Token: 0x04000026 RID: 38
	private int m_DefHeight;
}
