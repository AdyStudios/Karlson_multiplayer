using System;
using TMPro;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class Debug : MonoBehaviour
{
	// Token: 0x0600000A RID: 10 RVA: 0x000022D3 File Offset: 0x000004D3
	private void Start()
	{
		Application.targetFrameRate = 150;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000022DF File Offset: 0x000004DF
	private void Update()
	{
		this.Fps();
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (this.console.isActiveAndEnabled)
			{
				this.CloseConsole();
				return;
			}
			this.OpenConsole();
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x0000230C File Offset: 0x0000050C
	private void Fps()
	{
		if (this.fpsOn || this.speedOn)
		{
			if (!this.fps.gameObject.activeInHierarchy)
			{
				this.fps.gameObject.SetActive(true);
			}
			this.deltaTime += (Time.unscaledDeltaTime - this.deltaTime) * 0.1f;
			float num = this.deltaTime * 1000f;
			float num2 = 1f / this.deltaTime;
			string text = "";
			if (this.fpsOn)
			{
				text += string.Format("{0:0.0} ms ({1:0.} fps)", num, num2);
			}
			if (this.speedOn)
			{
				text = text + "\nm/s: " + string.Format("{0:F1}", PlayerMovement.Instance.rb.velocity.magnitude);
			}
			this.fps.text = text;
			return;
		}
		if (this.fps.enabled)
		{
			return;
		}
		this.fps.gameObject.SetActive(false);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002418 File Offset: 0x00000618
	private void OpenConsole()
	{
		this.console.gameObject.SetActive(true);
		this.console.Select();
		this.console.ActivateInputField();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		PlayerMovement.Instance.paused = true;
		Time.timeScale = 0f;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000246D File Offset: 0x0000066D
	private void CloseConsole()
	{
		this.console.gameObject.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		PlayerMovement.Instance.paused = false;
		Time.timeScale = 1f;
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000024A4 File Offset: 0x000006A4
	public void RunCommand()
	{
		string text = this.console.text;
		TextMeshProUGUI textMeshProUGUI = this.consoleLog;
		textMeshProUGUI.text = textMeshProUGUI.text + text + "\n";
		if (text.Length < 2 || text.Length > 30 || this.CountWords(text) != 2)
		{
			this.console.text = "";
			this.console.Select();
			this.console.ActivateInputField();
			return;
		}
		this.console.text = "";
		string s = text.Substring(text.IndexOf(' ') + 1);
		string a = text.Substring(0, text.IndexOf(' '));
		int n;
		if (!int.TryParse(s, out n))
		{
			TextMeshProUGUI textMeshProUGUI2 = this.consoleLog;
			textMeshProUGUI2.text += "Command not found\n";
			return;
		}
		if (!(a == "fps"))
		{
			if (!(a == "fpslimit"))
			{
				if (!(a == "fov"))
				{
					if (!(a == "sens"))
					{
						if (!(a == "speed"))
						{
							if (a == "help")
							{
								this.Help();
							}
						}
						else
						{
							this.OpenCloseSpeed(n);
						}
					}
					else
					{
						this.ChangeSens(n);
					}
				}
				else
				{
					this.ChangeFov(n);
				}
			}
			else
			{
				this.FpsLimit(n);
			}
		}
		else
		{
			this.OpenCloseFps(n);
		}
		this.console.Select();
		this.console.ActivateInputField();
	}

	// Token: 0x06000010 RID: 16 RVA: 0x0000260C File Offset: 0x0000080C
	private void Help()
	{
		string str = "The console can be used for simple commands.\nEvery command must be followed by number i (0 = false, 1 = true)\n<i><b>fps 1</b></i>            shows fps\n<i><b>speed 1</b></i>      shows speed\n<i><b>fov i</b></i>             sets fov to i\n<i><b>sens i</b></i>          sets sensitivity to i\n<i><b>fpslimit i</b></i>    sets max fps\n<i><b>TAB</b></i>              to open/close the console\n";
		TextMeshProUGUI textMeshProUGUI = this.consoleLog;
		textMeshProUGUI.text += str;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002638 File Offset: 0x00000838
	private void FpsLimit(int n)
	{
		Application.targetFrameRate = n;
		TextMeshProUGUI textMeshProUGUI = this.consoleLog;
		textMeshProUGUI.text = string.Concat(new object[]
		{
			textMeshProUGUI.text,
			"Max FPS set to ",
			n,
			"\n"
		});
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002688 File Offset: 0x00000888
	private void OpenCloseFps(int n)
	{
		this.fpsOn = (n == 1);
		TextMeshProUGUI textMeshProUGUI = this.consoleLog;
		textMeshProUGUI.text += ("FPS set to " + n == 1 + "\n").ToString();
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000026E4 File Offset: 0x000008E4
	private void OpenCloseSpeed(int n)
	{
		this.speedOn = (n == 1);
		TextMeshProUGUI textMeshProUGUI = this.consoleLog;
		textMeshProUGUI.text += ("Speedometer set to " + n == 1 + "\n").ToString();
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002740 File Offset: 0x00000940
	private void ChangeFov(int n)
	{
		GameState.Instance.SetFov((float)n);
		TextMeshProUGUI textMeshProUGUI = this.consoleLog;
		textMeshProUGUI.text = string.Concat(new object[]
		{
			textMeshProUGUI.text,
			"FOV set to ",
			n,
			"\n"
		});
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002794 File Offset: 0x00000994
	private void ChangeSens(int n)
	{
		GameState.Instance.SetSensitivity((float)n);
		TextMeshProUGUI textMeshProUGUI = this.consoleLog;
		textMeshProUGUI.text = string.Concat(new object[]
		{
			textMeshProUGUI.text,
			"Sensitivity set to ",
			n,
			"\n"
		});
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000027E8 File Offset: 0x000009E8
	private int CountWords(string text)
	{
		int num = 0;
		int i;
		for (i = 0; i < text.Length; i++)
		{
			if (!char.IsWhiteSpace(text[i]))
			{
				break;
			}
		}
		while (i < text.Length)
		{
			while (i < text.Length && !char.IsWhiteSpace(text[i]))
			{
				i++;
			}
			num++;
			while (i < text.Length && char.IsWhiteSpace(text[i]))
			{
				i++;
			}
		}
		return num;
	}

	// Token: 0x04000004 RID: 4
	public TextMeshProUGUI fps;

	// Token: 0x04000005 RID: 5
	public TMP_InputField console;

	// Token: 0x04000006 RID: 6
	public TextMeshProUGUI consoleLog;

	// Token: 0x04000007 RID: 7
	private bool fpsOn;

	// Token: 0x04000008 RID: 8
	private bool speedOn;

	// Token: 0x04000009 RID: 9
	private float deltaTime;
}
