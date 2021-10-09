using System;
using TMPro;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class PlayUI : MonoBehaviour
{
	// Token: 0x06000114 RID: 276 RVA: 0x00008410 File Offset: 0x00006610
	private void Start()
	{
		float[] times = SaveManager.Instance.state.times;
		for (int i = 0; i < this.maps.Length; i++)
		{
			MonoBehaviour.print("i: " + times[i]);
			this.maps[i].text = Timer.Instance.GetFormattedTime(times[i]);
		}
	}

	// Token: 0x040000FB RID: 251
	public TextMeshProUGUI[] maps;
}
