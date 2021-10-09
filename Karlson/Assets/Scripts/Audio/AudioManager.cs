using System;
using UnityEngine;

namespace Audio
{
	// Token: 0x02000039 RID: 57
	public class AudioManager : MonoBehaviour
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000AF30 File Offset: 0x00009130
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000AF37 File Offset: 0x00009137
		public static AudioManager Instance { get; set; }

		// Token: 0x060001CC RID: 460 RVA: 0x0000AF40 File Offset: 0x00009140
		private void Awake()
		{
			AudioManager.Instance = this;
			foreach (Sound sound in this.sounds)
			{
				sound.source = base.gameObject.AddComponent<AudioSource>();
				sound.source.clip = sound.clip;
				sound.source.loop = sound.loop;
				sound.source.volume = sound.volume;
				sound.source.pitch = sound.pitch;
				sound.source.bypassListenerEffects = sound.bypass;
			}
			foreach (Sound sound2 in this.footsteps)
			{
				sound2.source = base.gameObject.AddComponent<AudioSource>();
				sound2.source.clip = sound2.clip;
				sound2.source.loop = sound2.loop;
				sound2.source.volume = sound2.volume;
				sound2.source.pitch = sound2.pitch;
				sound2.source.bypassListenerEffects = sound2.bypass;
			}
			foreach (Sound sound3 in this.wallrun)
			{
				sound3.source = base.gameObject.AddComponent<AudioSource>();
				sound3.source.clip = sound3.clip;
				sound3.source.loop = sound3.loop;
				sound3.source.volume = sound3.volume;
				sound3.source.pitch = sound3.pitch;
				sound3.source.bypassListenerEffects = sound3.bypass;
			}
			foreach (Sound sound4 in this.jumps)
			{
				sound4.source = base.gameObject.AddComponent<AudioSource>();
				sound4.source.clip = sound4.clip;
				sound4.source.loop = sound4.loop;
				sound4.source.volume = sound4.volume;
				sound4.source.pitch = sound4.pitch;
				sound4.source.bypassListenerEffects = sound4.bypass;
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00003381 File Offset: 0x00001581
		private void Update()
		{
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000B167 File Offset: 0x00009367
		public void MuteSounds(bool b)
		{
			if (b)
			{
				AudioListener.volume = 0f;
			}
			else
			{
				AudioListener.volume = 1f;
			}
			this.muted = b;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000B18C File Offset: 0x0000938C
		public void PlayButton()
		{
			if (this.muted)
			{
				return;
			}
			foreach (Sound sound in this.sounds)
			{
				if (sound.name == "Button")
				{
					sound.source.pitch = 0.8f + UnityEngine.Random.Range(-0.03f, 0.03f);
					break;
				}
			}
			this.Play("Button");
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000B1FC File Offset: 0x000093FC
		public void PlayPitched(string n, float v)
		{
			if (this.muted)
			{
				return;
			}
			foreach (Sound sound in this.sounds)
			{
				if (sound.name == n)
				{
					sound.source.pitch = 1f + UnityEngine.Random.Range(-v, v);
					break;
				}
			}
			this.Play(n);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000B25C File Offset: 0x0000945C
		public void MuteMusic()
		{
			foreach (Sound sound in this.sounds)
			{
				if (sound.name == "Song")
				{
					sound.source.volume = 0f;
					return;
				}
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000B2A8 File Offset: 0x000094A8
		public void SetVolume(float v)
		{
			foreach (Sound sound in this.sounds)
			{
				if (sound.name == "Song")
				{
					sound.source.volume = v;
					return;
				}
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000B2F0 File Offset: 0x000094F0
		public void UnmuteMusic()
		{
			foreach (Sound sound in this.sounds)
			{
				if (sound.name == "Song")
				{
					sound.source.volume = 1.15f;
					return;
				}
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000B33C File Offset: 0x0000953C
		public void Play(string n)
		{
			if (this.muted && n != "Song")
			{
				return;
			}
			foreach (Sound sound in this.sounds)
			{
				if (sound.name == n)
				{
					sound.source.Play();
					return;
				}
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000B394 File Offset: 0x00009594
		public void PlayFootStep()
		{
			if (this.muted)
			{
				return;
			}
			int num = UnityEngine.Random.Range(0, this.footsteps.Length - 1);
			this.footsteps[num].source.Play();
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000B3D0 File Offset: 0x000095D0
		public void PlayLanding()
		{
			if (this.muted)
			{
				return;
			}
			int num = UnityEngine.Random.Range(0, this.wallrun.Length - 1);
			this.wallrun[num].source.Play();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000B40C File Offset: 0x0000960C
		public void PlayJump()
		{
			if (this.muted)
			{
				return;
			}
			int num = UnityEngine.Random.Range(0, this.jumps.Length - 1);
			Sound sound = this.jumps[num];
			if (sound.source)
			{
				sound.source.Play();
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000B454 File Offset: 0x00009654
		public void Stop(string n)
		{
			foreach (Sound sound in this.sounds)
			{
				if (sound.name == n)
				{
					sound.source.Stop();
					return;
				}
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000B494 File Offset: 0x00009694
		public void SetFreq(float freq)
		{
			this.desiredFreq = freq;
		}

		// Token: 0x0400018F RID: 399
		public Sound[] sounds;

		// Token: 0x04000190 RID: 400
		public Sound[] footsteps;

		// Token: 0x04000191 RID: 401
		public Sound[] wallrun;

		// Token: 0x04000192 RID: 402
		public Sound[] jumps;

		// Token: 0x04000193 RID: 403
		public AudioLowPassFilter filter;

		// Token: 0x04000194 RID: 404
		private float desiredFreq = 500f;

		// Token: 0x04000195 RID: 405
		private float velFreq;

		// Token: 0x04000196 RID: 406
		private float freqSpeed = 0.2f;

		// Token: 0x04000197 RID: 407
		public bool muted;
	}
}
