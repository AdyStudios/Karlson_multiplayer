using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200000C RID: 12
public class Game : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x0600002B RID: 43 RVA: 0x00002CB7 File Offset: 0x00000EB7
	// (set) Token: 0x0600002C RID: 44 RVA: 0x00002CBE File Offset: 0x00000EBE
	public static Game Instance { get; private set; }

	// Token: 0x0600002D RID: 45 RVA: 0x00002CC6 File Offset: 0x00000EC6
	private void Start()
	{
		Game.Instance = this;
		this.playing = false;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002CD5 File Offset: 0x00000ED5
	public void StartGame()
	{
		this.playing = true;
		this.done = false;
		Time.timeScale = 1f;
		UIManger.Instance.StartGame();
		Timer.Instance.StartTimer();
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002D04 File Offset: 0x00000F04
	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Time.timeScale = 1f;
		this.StartGame();
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002D33 File Offset: 0x00000F33
	public void EndGame()
	{
		this.playing = false;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002D3C File Offset: 0x00000F3C
	public void NextMap()
	{
		Time.timeScale = 1f;
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		if (buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
		{
			this.MainMenu();
			return;
		}
		SceneManager.LoadScene(buildIndex + 1);
		this.StartGame();
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002D80 File Offset: 0x00000F80
	public void MainMenu()
	{
		this.playing = false;
		SceneManager.LoadScene("MainMenu");
		UIManger.Instance.GameUI(false);
		Time.timeScale = 1f;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002DA8 File Offset: 0x00000FA8
	public void Win()
	{
		this.playing = false;
		Timer.Instance.Stop();
		Time.timeScale = 0.05f;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		UIManger.Instance.WinUI(true);
		float timer = Timer.Instance.GetTimer();
		int num = int.Parse(SceneManager.GetActiveScene().name[0].ToString() ?? "");
		int num2;
		if (int.TryParse(SceneManager.GetActiveScene().name.Substring(0, 2) ?? "", out num2))
		{
			num = num2;
		}
		float num3 = SaveManager.Instance.state.times[num];
		if (timer < num3 || num3 == 0f)
		{
			SaveManager.Instance.state.times[num] = timer;
			SaveManager.Instance.Save();
		}
		MonoBehaviour.print("time has been saved as: " + Timer.Instance.GetFormattedTime(timer));
		this.done = true;
	}

	// Token: 0x04000014 RID: 20
	public bool playing;

	// Token: 0x04000015 RID: 21
	public bool done;
}
