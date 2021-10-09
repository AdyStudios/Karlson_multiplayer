using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000016 RID: 22
public class Managers : MonoBehaviour
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000073 RID: 115 RVA: 0x000040D1 File Offset: 0x000022D1
	// (set) Token: 0x06000074 RID: 116 RVA: 0x000040D8 File Offset: 0x000022D8
	public static Managers Instance { get; private set; }

	// Token: 0x06000075 RID: 117 RVA: 0x000040E0 File Offset: 0x000022E0
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1f;
		Application.targetFrameRate = 240;
		QualitySettings.vSyncCount = 0;
	}
}
