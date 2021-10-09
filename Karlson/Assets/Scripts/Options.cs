using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200001E RID: 30
public class Options : MonoBehaviour
{
	// Token: 0x060000C6 RID: 198 RVA: 0x000062E0 File Offset: 0x000044E0
	private void OnEnable()
	{
		this.UpdateList(this.graphics, GameState.Instance.GetGraphics());
		this.UpdateList(this.shake, GameState.Instance.shake);
		this.UpdateList(this.slowmo, GameState.Instance.slowmo);
		this.UpdateList(this.blur, GameState.Instance.blur);
		this.sensS.value = GameState.Instance.GetSensitivity();
		this.volumeS.value = GameState.Instance.GetVolume();
		this.musicS.value = GameState.Instance.GetMusic();
		this.fovS.value = GameState.Instance.GetFov();
		MonoBehaviour.print(GameState.Instance.GetMusic());
		this.UpdateSensitivity();
		this.UpdateFov();
		this.UpdateVolume();
		this.UpdateMusic();
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x000063C5 File Offset: 0x000045C5
	public void ChangeGraphics(bool b)
	{
		GameState.Instance.SetGraphics(b);
		this.UpdateList(this.graphics, b);
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x000063DF File Offset: 0x000045DF
	public void ChangeBlur(bool b)
	{
		GameState.Instance.SetBlur(b);
		this.UpdateList(this.blur, b);
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x000063F9 File Offset: 0x000045F9
	public void ChangeShake(bool b)
	{
		GameState.Instance.SetShake(b);
		this.UpdateList(this.shake, b);
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00006413 File Offset: 0x00004613
	public void ChangeSlowmo(bool b)
	{
		GameState.Instance.SetSlowmo(b);
		this.UpdateList(this.slowmo, b);
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00006430 File Offset: 0x00004630
	public void UpdateSensitivity()
	{
		float value = this.sensS.value;
		GameState.Instance.SetSensitivity(value);
		this.sens.text = string.Format("{0:F2}", value);
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00006470 File Offset: 0x00004670
	public void UpdateVolume()
	{
		float value = this.volumeS.value;
		AudioListener.volume = value;
		GameState.Instance.SetVolume(value);
		this.volume.text = string.Format("{0:F2}", value);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x000064B8 File Offset: 0x000046B8
	public void UpdateMusic()
	{
		float value = this.musicS.value;
		GameState.Instance.SetMusic(value);
		this.music.text = string.Format("{0:F2}", value);
	}

	// Token: 0x060000CE RID: 206 RVA: 0x000064F8 File Offset: 0x000046F8
	public void UpdateFov()
	{
		float value = this.fovS.value;
		GameState.Instance.SetFov(value);
		this.fov.text = string.Concat(value);
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00006534 File Offset: 0x00004734
	private void UpdateList(TextMeshProUGUI[] list, bool b)
	{
		if (!b)
		{
			list[1].color = Color.white;
			list[0].color = (Color.clear + Color.white) / 2f;
			return;
		}
		list[1].color = (Color.clear + Color.white) / 2f;
		list[0].color = Color.white;
	}

	// Token: 0x0400009E RID: 158
	public TextMeshProUGUI sens;

	// Token: 0x0400009F RID: 159
	public TextMeshProUGUI volume;

	// Token: 0x040000A0 RID: 160
	public TextMeshProUGUI music;

	// Token: 0x040000A1 RID: 161
	public TextMeshProUGUI fov;

	// Token: 0x040000A2 RID: 162
	public TextMeshProUGUI[] sounds;

	// Token: 0x040000A3 RID: 163
	public TextMeshProUGUI[] graphics;

	// Token: 0x040000A4 RID: 164
	public TextMeshProUGUI[] shake;

	// Token: 0x040000A5 RID: 165
	public TextMeshProUGUI[] slowmo;

	// Token: 0x040000A6 RID: 166
	public TextMeshProUGUI[] blur;

	// Token: 0x040000A7 RID: 167
	public Slider sensS;

	// Token: 0x040000A8 RID: 168
	public Slider volumeS;

	// Token: 0x040000A9 RID: 169
	public Slider musicS;

	// Token: 0x040000AA RID: 170
	public Slider fovS;
}
