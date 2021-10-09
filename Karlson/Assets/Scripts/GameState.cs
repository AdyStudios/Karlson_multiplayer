using System;
using Audio;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200000D RID: 13
public class GameState : MonoBehaviour
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000035 RID: 53 RVA: 0x00002EA2 File Offset: 0x000010A2
	// (set) Token: 0x06000036 RID: 54 RVA: 0x00002EA9 File Offset: 0x000010A9
	public static GameState Instance { get; private set; }

	// Token: 0x06000037 RID: 55 RVA: 0x00002EB4 File Offset: 0x000010B4
	private void Start()
	{
		GameState.Instance = this;
		this.ppBlur = this.pp.GetSetting<MotionBlur>();
		this.graphics = SaveManager.Instance.state.graphics;
		this.shake = SaveManager.Instance.state.cameraShake;
		this.blur = SaveManager.Instance.state.motionBlur;
		this.slowmo = SaveManager.Instance.state.slowmo;
		this.muted = SaveManager.Instance.state.muted;
		this.sensitivity = SaveManager.Instance.state.sensitivity;
		this.music = SaveManager.Instance.state.music;
		this.volume = SaveManager.Instance.state.volume;
		this.fov = SaveManager.Instance.state.fov;
		this.UpdateSettings();
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002F9B File Offset: 0x0000119B
	public void SetGraphics(bool b)
	{
		this.graphics = b;
		this.ppVolume.SetActive(b);
		SaveManager.Instance.state.graphics = b;
		SaveManager.Instance.Save();
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002FCC File Offset: 0x000011CC
	public void SetBlur(bool b)
	{
		this.blur = b;
		if (b)
		{
			this.ppBlur.shutterAngle.value = 160f;
		}
		else
		{
			this.ppBlur.shutterAngle.value = 0f;
		}
		SaveManager.Instance.state.motionBlur = b;
		SaveManager.Instance.Save();
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00003029 File Offset: 0x00001229
	public void SetShake(bool b)
	{
		this.shake = b;
		if (b)
		{
			this.cameraShake = 1f;
		}
		else
		{
			this.cameraShake = 0f;
		}
		SaveManager.Instance.state.cameraShake = b;
		SaveManager.Instance.Save();
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003067 File Offset: 0x00001267
	public void SetSlowmo(bool b)
	{
		this.slowmo = b;
		SaveManager.Instance.state.slowmo = b;
		SaveManager.Instance.Save();
	}

	// Token: 0x0600003C RID: 60 RVA: 0x0000308C File Offset: 0x0000128C
	public void SetSensitivity(float s)
	{
		float num = Mathf.Clamp(s, 0f, 5f);
		this.sensitivity = num;
		if (PlayerMovement.Instance)
		{
			PlayerMovement.Instance.UpdateSensitivity();
		}
		SaveManager.Instance.state.sensitivity = num;
		SaveManager.Instance.Save();
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000030E4 File Offset: 0x000012E4
	public void SetMusic(float s)
	{
		float musicVolume = Mathf.Clamp(s, 0f, 1f);
		this.music = musicVolume;
		if (Music.Instance)
		{
			Music.Instance.SetMusicVolume(musicVolume);
		}
		SaveManager.Instance.state.music = musicVolume;
		SaveManager.Instance.Save();
		MonoBehaviour.print("music saved as: " + this.music);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003154 File Offset: 0x00001354
	public void SetVolume(float s)
	{
		float num = Mathf.Clamp(s, 0f, 1f);
		this.volume = num;
		AudioListener.volume = num;
		SaveManager.Instance.state.volume = num;
		SaveManager.Instance.Save();
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0000319C File Offset: 0x0000139C
	public void SetFov(float f)
	{
		float num = Mathf.Clamp(f, 50f, 150f);
		this.fov = num;
		if (MoveCamera.Instance)
		{
			MoveCamera.Instance.UpdateFov();
		}
		SaveManager.Instance.state.fov = num;
		SaveManager.Instance.Save();
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000031F1 File Offset: 0x000013F1
	public void SetMuted(bool b)
	{
		AudioManager.Instance.MuteSounds(b);
		this.muted = b;
		SaveManager.Instance.state.muted = b;
		SaveManager.Instance.Save();
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003220 File Offset: 0x00001420
	private void UpdateSettings()
	{
		this.SetGraphics(this.graphics);
		this.SetBlur(this.blur);
		this.SetSensitivity(this.sensitivity);
		this.SetMusic(this.music);
		this.SetVolume(this.volume);
		this.SetFov(this.fov);
		this.SetShake(this.shake);
		this.SetSlowmo(this.slowmo);
		this.SetMuted(this.muted);
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003299 File Offset: 0x00001499
	public bool GetGraphics()
	{
		return this.graphics;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000032A1 File Offset: 0x000014A1
	public float GetSensitivity()
	{
		return this.sensitivity;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000032A9 File Offset: 0x000014A9
	public float GetVolume()
	{
		return this.volume;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000032B1 File Offset: 0x000014B1
	public float GetMusic()
	{
		return this.music;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000032B9 File Offset: 0x000014B9
	public float GetFov()
	{
		return this.fov;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000032C1 File Offset: 0x000014C1
	public bool GetMuted()
	{
		return this.muted;
	}

	// Token: 0x04000017 RID: 23
	public GameObject ppVolume;

	// Token: 0x04000018 RID: 24
	public PostProcessProfile pp;

	// Token: 0x04000019 RID: 25
	private MotionBlur ppBlur;

	// Token: 0x0400001A RID: 26
	public bool graphics = true;

	// Token: 0x0400001B RID: 27
	public bool muted;

	// Token: 0x0400001C RID: 28
	public bool blur = true;

	// Token: 0x0400001D RID: 29
	public bool shake = true;

	// Token: 0x0400001E RID: 30
	public bool slowmo = true;

	// Token: 0x0400001F RID: 31
	private float sensitivity = 1f;

	// Token: 0x04000020 RID: 32
	private float volume;

	// Token: 0x04000021 RID: 33
	private float music;

	// Token: 0x04000022 RID: 34
	public float fov = 1f;

	// Token: 0x04000023 RID: 35
	public float cameraShake = 1f;
}
