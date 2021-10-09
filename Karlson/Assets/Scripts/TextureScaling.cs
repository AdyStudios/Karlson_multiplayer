using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
[ExecuteInEditMode]
public class TextureScaling : MonoBehaviour
{
	// Token: 0x06000176 RID: 374 RVA: 0x00009ABA File Offset: 0x00007CBA
	private void Start()
	{
		this.Calculate();
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00009ABA File Offset: 0x00007CBA
	private void Update()
	{
		this.Calculate();
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00009AC4 File Offset: 0x00007CC4
	public void Calculate()
	{
		if (this._currentScale == base.transform.localScale)
		{
			return;
		}
		if (this.CheckForDefaultSize())
		{
			return;
		}
		this._currentScale = base.transform.localScale;
		Mesh mesh = this.GetMesh();
		mesh.uv = this.SetupUvMap(mesh.uv);
		mesh.name = "Cube Instance";
		if (base.GetComponent<Renderer>().sharedMaterial.mainTexture.wrapMode != TextureWrapMode.Repeat)
		{
			base.GetComponent<Renderer>().sharedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
		}
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00009B55 File Offset: 0x00007D55
	private Mesh GetMesh()
	{
		return base.GetComponent<MeshFilter>().mesh;
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00009B64 File Offset: 0x00007D64
	private Vector2[] SetupUvMap(Vector2[] meshUVs)
	{
		float x = this._currentScale.x * this.size;
		float num = this._currentScale.z * this.size;
		float y = this._currentScale.y * this.size;
		meshUVs[2] = new Vector2(0f, y);
		meshUVs[3] = new Vector2(x, y);
		meshUVs[0] = new Vector2(0f, 0f);
		meshUVs[1] = new Vector2(x, 0f);
		meshUVs[7] = new Vector2(0f, 0f);
		meshUVs[6] = new Vector2(x, 0f);
		meshUVs[11] = new Vector2(0f, y);
		meshUVs[10] = new Vector2(x, y);
		meshUVs[19] = new Vector2(num, 0f);
		meshUVs[17] = new Vector2(0f, y);
		meshUVs[16] = new Vector2(0f, 0f);
		meshUVs[18] = new Vector2(num, y);
		meshUVs[23] = new Vector2(num, 0f);
		meshUVs[21] = new Vector2(0f, y);
		meshUVs[20] = new Vector2(0f, 0f);
		meshUVs[22] = new Vector2(num, y);
		meshUVs[4] = new Vector2(x, 0f);
		meshUVs[5] = new Vector2(0f, 0f);
		meshUVs[8] = new Vector2(x, num);
		meshUVs[9] = new Vector2(0f, num);
		meshUVs[13] = new Vector2(x, 0f);
		meshUVs[14] = new Vector2(0f, 0f);
		meshUVs[12] = new Vector2(x, num);
		meshUVs[15] = new Vector2(0f, num);
		return meshUVs;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00009D6C File Offset: 0x00007F6C
	private bool CheckForDefaultSize()
	{
		if (this._currentScale != Vector3.one)
		{
			return false;
		}
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		UnityEngine.Object.DestroyImmediate(base.GetComponent<MeshFilter>());
		base.gameObject.AddComponent<MeshFilter>();
		base.GetComponent<MeshFilter>().sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
		UnityEngine.Object.DestroyImmediate(gameObject);
		return true;
	}

	// Token: 0x04000153 RID: 339
	private Vector3 _currentScale;

	// Token: 0x04000154 RID: 340
	public float size = 1f;
}
