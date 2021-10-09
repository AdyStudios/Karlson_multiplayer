using System;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000014 RID: 20
public class Lobby : MonoBehaviour
{
	// Token: 0x06000065 RID: 101 RVA: 0x00003381 File Offset: 0x00001581
	private void Start()
	{
	}

	// Token: 0x06000066 RID: 102 RVA: 0x0000401F File Offset: 0x0000221F
	public void LoadMap(string s)
	{
		if (s.Equals(""))
		{
			return;
		}
		SceneManager.LoadScene(s);
		Game.Instance.StartGame();
	}

	// Token: 0x06000067 RID: 103 RVA: 0x0000403F File Offset: 0x0000223F
	public void Exit()
	{
		Application.Quit(0);
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00004047 File Offset: 0x00002247
	public void ButtonSound()
	{
		AudioManager.Instance.Play("Button");
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00004058 File Offset: 0x00002258
	public void Youtube()
	{
		Application.OpenURL("https://youtube.com/danidev");
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00004064 File Offset: 0x00002264
	public void Twitter()
	{
		Application.OpenURL("https://twitter.com/DaniDevYT");
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00004070 File Offset: 0x00002270
	public void Facebook()
	{
		Application.OpenURL("https://www.facebook.com/DWSgames");
	}

	// Token: 0x0600006C RID: 108 RVA: 0x0000407C File Offset: 0x0000227C
	public void Discord()
	{
		Application.OpenURL("https://discord.gg/P53pFtR");
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00004088 File Offset: 0x00002288
	public void Steam()
	{
		Application.OpenURL("https://store.steampowered.com/app/1228610/Karlson");
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00004094 File Offset: 0x00002294
	public void EvanYoutube()
	{
		Application.OpenURL("https://www.youtube.com/user/EvanKingAudio");
	}

	// Token: 0x0600006F RID: 111 RVA: 0x000040A0 File Offset: 0x000022A0
	public void EvanTwitter()
	{
		Application.OpenURL("https://twitter.com/EvanKingAudio");
	}
}
