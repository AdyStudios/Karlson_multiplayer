using System;
using TMPro;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class Timer : MonoBehaviour
{
	// Token: 0x17000019 RID: 25
	// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000AD07 File Offset: 0x00008F07
	// (set) Token: 0x060001B7 RID: 439 RVA: 0x0000AD0E File Offset: 0x00008F0E
	public static Timer Instance { get; set; }

	// Token: 0x060001B8 RID: 440 RVA: 0x0000AD16 File Offset: 0x00008F16
	private void Awake()
	{
		Timer.Instance = this;
		this.text = base.GetComponent<TextMeshProUGUI>();
		this.stop = false;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000AD31 File Offset: 0x00008F31
	public void StartTimer()
	{
		this.stop = false;
		this.timer = 0f;
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000AD45 File Offset: 0x00008F45
	private void Update()
	{
		if (!Game.Instance.playing || this.stop)
		{
			return;
		}
		this.timer += Time.deltaTime;
		this.text.text = this.GetFormattedTime(this.timer);
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000AD88 File Offset: 0x00008F88
	public string GetFormattedTime(float f)
	{
		if (f == 0f)
		{
			return "nan";
		}
		string arg = Mathf.Floor(f / 60f).ToString("00");
		string arg2 = Mathf.Floor(f % 60f).ToString("00");
		string text = (f * 100f % 100f).ToString("00");
		if (text.Equals("100"))
		{
			text = "99";
		}
		return string.Format("{0}:{1}:{2}", arg, arg2, text);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000AE12 File Offset: 0x00009012
	public float GetTimer()
	{
		return this.timer;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000AE1C File Offset: 0x0000901C
	private string StatusText(float f)
	{
		if (f < 2f)
		{
			return "very easy";
		}
		if (f < 4f)
		{
			return "easy";
		}
		if (f < 8f)
		{
			return "medium";
		}
		if (f < 12f)
		{
			return "hard";
		}
		if (f < 16f)
		{
			return "very hard";
		}
		if (f < 20f)
		{
			return "impossible";
		}
		if (f < 25f)
		{
			return "oh shit";
		}
		if (f < 30f)
		{
			return "very oh shit";
		}
		return "f";
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000AE9E File Offset: 0x0000909E
	public void Stop()
	{
		this.stop = true;
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000AEA7 File Offset: 0x000090A7
	public int GetMinutes()
	{
		return (int)Mathf.Floor(this.timer / 60f);
	}

	// Token: 0x04000187 RID: 391
	private TextMeshProUGUI text;

	// Token: 0x04000188 RID: 392
	private float timer;

	// Token: 0x04000189 RID: 393
	private bool stop;
}
