using System;
using UnityEngine;

namespace EZCameraShake
{
	// Token: 0x0200003F RID: 63
	public class CameraUtilities
	{
		// Token: 0x060001FD RID: 509 RVA: 0x0000BD04 File Offset: 0x00009F04
		public static Vector3 SmoothDampEuler(Vector3 current, Vector3 target, ref Vector3 velocity, float smoothTime)
		{
			Vector3 result;
			result.x = Mathf.SmoothDampAngle(current.x, target.x, ref velocity.x, smoothTime);
			result.y = Mathf.SmoothDampAngle(current.y, target.y, ref velocity.y, smoothTime);
			result.z = Mathf.SmoothDampAngle(current.z, target.z, ref velocity.z, smoothTime);
			return result;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000BD6F File Offset: 0x00009F6F
		public static Vector3 MultiplyVectors(Vector3 v, Vector3 w)
		{
			v.x *= w.x;
			v.y *= w.y;
			v.z *= w.z;
			return v;
		}
	}
}
