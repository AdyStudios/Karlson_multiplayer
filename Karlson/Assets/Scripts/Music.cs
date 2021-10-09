using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class Music : MonoBehaviour
{
	// Token: 0x17000007 RID: 7
	// (get) Token: 0x060000BA RID: 186 RVA: 0x00006002 File Offset: 0x00004202
	// (set) Token: 0x060000BB RID: 187 RVA: 0x00006009 File Offset: 0x00004209
	public static Music Instance { get; private set; }

	// Token: 0x060000BC RID: 188 RVA: 0x00006011 File Offset: 0x00004211
	private void Awake()
	{
		Music.Instance = this;
		this.music = base.GetComponent<AudioSource>();
		this.music.volume = 0.04f;
		this.multiplier = 1f;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00006040 File Offset: 0x00004240
	private void Update()
	{
		this.desiredVolume = 0.016f * this.multiplier;
		if (Game.Instance.playing)
		{
			this.desiredVolume = 0.6f * this.multiplier;
		}
		this.music.volume = Mathf.SmoothDamp(this.music.volume, this.desiredVolume, ref this.vel, 0.6f);
	}

	// Token: 0x060000BE RID: 190 RVA: 0x000060A9 File Offset: 0x000042A9
	public void SetMusicVolume(float f)
	{
		this.multiplier = f;
	}

	// Token: 0x04000096 RID: 150
	private AudioSource music;

	// Token: 0x04000097 RID: 151
	private float multiplier;

	// Token: 0x04000099 RID: 153
	private float desiredVolume;

	// Token: 0x0400009A RID: 154
	private float vel;
}
