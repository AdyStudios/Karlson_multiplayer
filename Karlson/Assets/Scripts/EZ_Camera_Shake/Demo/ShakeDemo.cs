using System;
using EZCameraShake;
using UnityEngine;

namespace EZ_Camera_Shake.Demo
{
	// Token: 0x02000040 RID: 64
	public class ShakeDemo : MonoBehaviour
	{
		// Token: 0x06000200 RID: 512 RVA: 0x0000BDA8 File Offset: 0x00009FA8
		private void OnGUI()
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				Application.LoadLevel(Application.loadedLevel);
			}
			ShakeDemo.Slider slider = delegate(float val, string prefix, float min, float max, int pad)
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label(prefix, new GUILayoutOption[]
				{
					GUILayout.MaxWidth((float)pad)
				});
				val = GUILayout.HorizontalSlider(val, min, max, Array.Empty<GUILayoutOption>());
				GUILayout.Label(val.ToString("F2"), new GUILayoutOption[]
				{
					GUILayout.MaxWidth(50f)
				});
				GUILayout.EndHorizontal();
				return val;
			};
			GUI.Box(new Rect(10f, 10f, 250f, (float)(Screen.height - 15)), "Make-A-Shake");
			GUILayout.BeginArea(new Rect(29f, 40f, 215f, (float)(Screen.height - 40)));
			GUILayout.Label("--Position Infleunce--", Array.Empty<GUILayoutOption>());
			this.posInf.x = slider(this.posInf.x, "X", 0f, 4f, 25);
			this.posInf.y = slider(this.posInf.y, "Y", 0f, 4f, 25);
			this.posInf.z = slider(this.posInf.z, "Z", 0f, 4f, 25);
			GUILayout.Label("--Rotation Infleunce--", Array.Empty<GUILayoutOption>());
			this.rotInf.x = slider(this.rotInf.x, "X", 0f, 4f, 25);
			this.rotInf.y = slider(this.rotInf.y, "Y", 0f, 4f, 25);
			this.rotInf.z = slider(this.rotInf.z, "Z", 0f, 4f, 25);
			GUILayout.Label("--Other Properties--", Array.Empty<GUILayoutOption>());
			this.magn = slider(this.magn, "Magnitude:", 0f, 10f, 75);
			this.rough = slider(this.rough, "Roughness:", 0f, 20f, 75);
			this.fadeIn = slider(this.fadeIn, "Fade In:", 0f, 10f, 75);
			this.fadeOut = slider(this.fadeOut, "Fade Out:", 0f, 10f, 75);
			GUILayout.Label("--Saved Shake Instance--", Array.Empty<GUILayoutOption>());
			GUILayout.Label("You can save shake instances and modify their properties at runtime.", Array.Empty<GUILayoutOption>());
			if (this.shake == null && GUILayout.Button("Create Shake Instance", Array.Empty<GUILayoutOption>()))
			{
				this.shake = CameraShaker.Instance.StartShake(this.magn, this.rough, this.fadeIn);
				this.shake.DeleteOnInactive = false;
			}
			if (this.shake != null)
			{
				if (GUILayout.Button("Delete Shake Instance", Array.Empty<GUILayoutOption>()))
				{
					this.shake.DeleteOnInactive = true;
					this.shake.StartFadeOut(this.fadeOut);
					this.shake = null;
				}
				if (this.shake != null)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Fade Out", Array.Empty<GUILayoutOption>()))
					{
						this.shake.StartFadeOut(this.fadeOut);
					}
					if (GUILayout.Button("Fade In", Array.Empty<GUILayoutOption>()))
					{
						this.shake.StartFadeIn(this.fadeIn);
					}
					GUILayout.EndHorizontal();
					this.modValues = GUILayout.Toggle(this.modValues, "Modify Instance Values", Array.Empty<GUILayoutOption>());
					if (this.modValues)
					{
						this.shake.ScaleMagnitude = this.magn;
						this.shake.ScaleRoughness = this.rough;
						this.shake.PositionInfluence = this.posInf;
						this.shake.RotationInfluence = this.rotInf;
					}
				}
			}
			GUILayout.Label("--Shake Once--", Array.Empty<GUILayoutOption>());
			GUILayout.Label("You can simply have a shake that automatically starts and stops too.", Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Shake!", Array.Empty<GUILayoutOption>()))
			{
				CameraShakeInstance cameraShakeInstance = CameraShaker.Instance.ShakeOnce(this.magn, this.rough, this.fadeIn, this.fadeOut);
				cameraShakeInstance.PositionInfluence = this.posInf;
				cameraShakeInstance.RotationInfluence = this.rotInf;
			}
			GUILayout.EndArea();
			float height;
			if (!this.showList)
			{
				height = 120f;
			}
			else
			{
				height = 120f + (float)CameraShaker.Instance.ShakeInstances.Count * 130f;
			}
			GUI.Box(new Rect((float)(Screen.width - 310), 10f, 300f, height), "Shake Instance List");
			GUILayout.BeginArea(new Rect((float)(Screen.width - 285), 40f, 255f, (float)(Screen.height - 40)));
			GUILayout.Label("All shake instances are saved and stacked as long as they are active.", Array.Empty<GUILayoutOption>());
			this.showList = GUILayout.Toggle(this.showList, "Show List", Array.Empty<GUILayoutOption>());
			if (this.showList)
			{
				int num = 1;
				foreach (CameraShakeInstance cameraShakeInstance2 in CameraShaker.Instance.ShakeInstances)
				{
					string str = cameraShakeInstance2.CurrentState.ToString();
					GUILayout.Label(string.Concat(new object[]
					{
						"#",
						num,
						": Magnitude: ",
						cameraShakeInstance2.Magnitude.ToString("F2"),
						", Roughness: ",
						cameraShakeInstance2.Roughness.ToString("F2")
					}), Array.Empty<GUILayoutOption>());
					GUILayout.Label("      Position Influence: " + cameraShakeInstance2.PositionInfluence, Array.Empty<GUILayoutOption>());
					GUILayout.Label("      Rotation Influence: " + cameraShakeInstance2.RotationInfluence, Array.Empty<GUILayoutOption>());
					GUILayout.Label("      State: " + str, Array.Empty<GUILayoutOption>());
					GUILayout.Label("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", Array.Empty<GUILayoutOption>());
					num++;
				}
			}
			GUILayout.EndArea();
		}

		// Token: 0x040001B9 RID: 441
		private Vector3 posInf = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x040001BA RID: 442
		private Vector3 rotInf = new Vector3(1f, 1f, 1f);

		// Token: 0x040001BB RID: 443
		private float magn = 1f;

		// Token: 0x040001BC RID: 444
		private float rough = 1f;

		// Token: 0x040001BD RID: 445
		private float fadeIn = 0.1f;

		// Token: 0x040001BE RID: 446
		private float fadeOut = 2f;

		// Token: 0x040001BF RID: 447
		private bool modValues;

		// Token: 0x040001C0 RID: 448
		private bool showList;

		// Token: 0x040001C1 RID: 449
		private CameraShakeInstance shake;

		// Token: 0x02000041 RID: 65
		// (Invoke) Token: 0x06000203 RID: 515
		private delegate float Slider(float val, string prefix, float min, float max, int pad);
	}
}
