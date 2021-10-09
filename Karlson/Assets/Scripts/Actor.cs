using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
public abstract class Actor : MonoBehaviour
{
	// Token: 0x06000128 RID: 296 RVA: 0x00008830 File Offset: 0x00006A30
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.OnStart();
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00008844 File Offset: 0x00006A44
	private void FixedUpdate()
	{
		this.Movement();
		this.RotateBody();
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00008852 File Offset: 0x00006A52
	private void LateUpdate()
	{
		this.Look();
	}

	// Token: 0x0600012B RID: 299 RVA: 0x0000885A File Offset: 0x00006A5A
	private void Update()
	{
		this.Logic();
	}

	// Token: 0x0600012C RID: 300
	protected abstract void OnStart();

	// Token: 0x0600012D RID: 301
	protected abstract void Logic();

	// Token: 0x0600012E RID: 302
	protected abstract void RotateBody();

	// Token: 0x0600012F RID: 303
	protected abstract void Look();

	// Token: 0x06000130 RID: 304 RVA: 0x00008864 File Offset: 0x00006A64
	private void Movement()
	{
		this.grounded = (Physics.OverlapSphere(this.groundChecker.position, 0.1f, this.whatIsGround).Length != 0);
		Vector2 vector = this.FindVelRelativeToLook();
		float num = vector.x;
		float num2 = vector.y;
		this.CounterMovement(this.x, this.y, vector);
		if (this.readyToJump && this.jumping)
		{
			this.Jump();
		}
		if (this.crouching && this.grounded && this.readyToJump)
		{
			this.rb.AddForce(Vector3.down * Time.deltaTime * 2000f);
			return;
		}
		if (this.x > 0f && num > this.maxSpeed)
		{
			this.x = 0f;
		}
		if (this.x < 0f && num < -this.maxSpeed)
		{
			this.x = 0f;
		}
		if (this.y > 0f && num2 > this.maxSpeed)
		{
			this.y = 0f;
		}
		if (this.y < 0f && num2 < -this.maxSpeed)
		{
			this.y = 0f;
		}
		this.rb.AddForce(Time.deltaTime * this.y * this.accelerationSpeed * this.orientation.transform.forward);
		this.rb.AddForce(Time.deltaTime * this.x * this.accelerationSpeed * this.orientation.transform.right);
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00008A04 File Offset: 0x00006C04
	private void Jump()
	{
		if (this.grounded || this.wallRunning)
		{
			Vector3 velocity = this.rb.velocity;
			this.rb.velocity = new Vector3(velocity.x, 0f, velocity.z);
			this.readyToJump = false;
			this.rb.AddForce(Vector2.up * this.jumpForce * 1.5f);
			this.rb.AddForce(this.wallNormalVector * this.jumpForce * 0.5f);
			base.Invoke("ResetJump", this.jumpCooldown);
			if (this.wallRunning)
			{
				this.wallRunning = false;
			}
		}
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00008AC8 File Offset: 0x00006CC8
	private void ResetJump()
	{
		this.readyToJump = true;
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00008AD4 File Offset: 0x00006CD4
	protected void CounterMovement(float x, float y, Vector2 mag)
	{
		if (!this.grounded || this.crouching)
		{
			return;
		}
		float num = 0.2f;
		if (x == 0f || (mag.x < 0f && x > 0f) || (mag.x > 0f && x < 0f))
		{
			this.rb.AddForce(this.accelerationSpeed * num * Time.deltaTime * -mag.x * this.orientation.transform.right);
		}
		if (y == 0f || (mag.y < 0f && y > 0f) || (mag.y > 0f && y < 0f))
		{
			this.rb.AddForce(this.accelerationSpeed * num * Time.deltaTime * -mag.y * this.orientation.transform.forward);
		}
		if (Mathf.Sqrt(Mathf.Pow(this.rb.velocity.x, 2f) + Mathf.Pow(this.rb.velocity.z, 2f)) > 20f)
		{
			float num2 = this.rb.velocity.y;
			Vector3 vector = this.rb.velocity.normalized * 20f;
			this.rb.velocity = new Vector3(vector.x, num2, vector.z);
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00008C58 File Offset: 0x00006E58
	protected Vector2 FindVelRelativeToLook()
	{
		float current = this.orientation.transform.eulerAngles.y;
		Vector3 velocity = this.rb.velocity;
		float target = Mathf.Atan2(velocity.x, velocity.z) * 57.29578f;
		float num = Mathf.DeltaAngle(current, target);
		float num2 = 90f - num;
		float magnitude = this.rb.velocity.magnitude;
		float num3 = magnitude * Mathf.Cos(num * 0.017453292f);
		return new Vector2(magnitude * Mathf.Cos(num2 * 0.017453292f), num3);
	}

	// Token: 0x04000116 RID: 278
	public Transform gunPosition;

	// Token: 0x04000117 RID: 279
	public Transform orientation;

	// Token: 0x04000118 RID: 280
	private float xRotation;

	// Token: 0x04000119 RID: 281
	private Rigidbody rb;

	// Token: 0x0400011A RID: 282
	private float accelerationSpeed = 4500f;

	// Token: 0x0400011B RID: 283
	private float maxSpeed = 20f;

	// Token: 0x0400011C RID: 284
	private bool crouching;

	// Token: 0x0400011D RID: 285
	private bool jumping;

	// Token: 0x0400011E RID: 286
	private bool wallRunning;

	// Token: 0x0400011F RID: 287
	protected float x;

	// Token: 0x04000120 RID: 288
	protected float y;

	// Token: 0x04000121 RID: 289
	private Vector3 wallNormalVector = Vector3.up;

	// Token: 0x04000122 RID: 290
	private bool grounded;

	// Token: 0x04000123 RID: 291
	public Transform groundChecker;

	// Token: 0x04000124 RID: 292
	public LayerMask whatIsGround;

	// Token: 0x04000125 RID: 293
	private bool readyToJump;

	// Token: 0x04000126 RID: 294
	private float jumpCooldown = 0.2f;

	// Token: 0x04000127 RID: 295
	private float jumpForce = 500f;
}
