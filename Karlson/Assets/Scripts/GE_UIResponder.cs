using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000F RID: 15
public class GE_UIResponder : MonoBehaviour
{
	// Token: 0x0600004D RID: 77 RVA: 0x00003450 File Offset: 0x00001650
	private void Start()
	{
		GameObject gameObject = GameObject.Find("Text Package Title");
		if (gameObject != null)
		{
			gameObject.GetComponent<Text>().text = this.m_PackageTitle;
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003381 File Offset: 0x00001581
	private void Update()
	{
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003482 File Offset: 0x00001682
	public void OnButton_AssetName()
	{
		Application.OpenURL(this.m_TargetURL);
	}

	// Token: 0x04000027 RID: 39
	public string m_PackageTitle = "-";

	// Token: 0x04000028 RID: 40
	public string m_TargetURL = "www.unity3d.com";
}
