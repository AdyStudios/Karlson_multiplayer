using System;
using Audio;
using EZCameraShake;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class Movement : MonoBehaviour
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000087 RID: 135 RVA: 0x00004435 File Offset: 0x00002635
	// (set) Token: 0x06000088 RID: 136 RVA: 0x0000443C File Offset: 0x0000263C
	public static Movement Instance { get; private set; }

	// Token: 0x06000089 RID: 137 RVA: 0x00004444 File Offset: 0x00002644
	private void Awake()
	{
		Movement.Instance = this;
		this.rb = base.GetComponent<Rigidbody>();
		MonoBehaviour.print("rb: " + this.rb);
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00004470 File Offset: 0x00002670
	private void Start()
	{
		this.psEmission = this.ps.emission;
		this.playerCollider = base.GetComponent<Collider>();
		this.detectWeapons = (DetectWeapons)base.GetComponentInChildren(typeof(DetectWeapons));
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		this.readyToJump = true;
		this.wallNormalVector = Vector3.up;
		this.CameraShake();
		if (this.spawnWeapon != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.spawnWeapon, base.transform.position, Quaternion.identity);
			this.detectWeapons.ForcePickup(gameObject);
		}
		if (GameState.Instance)
		{
			this.sensMultiplier = GameState.Instance.GetSensitivity();
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x0000452B File Offset: 0x0000272B
	private void LateUpdate()
	{
		if (this.dead || this.paused)
		{
			return;
		}
		this.DrawGrapple();
		this.DrawGrabbing();
		this.WallRunning();
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00004550 File Offset: 0x00002750
	private void FixedUpdate()
	{
		if (this.dead || Game.Instance.done || this.paused)
		{
			return;
		}
		this.Move();
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00004578 File Offset: 0x00002778
	private void Update()
	{
		this.MyInput();
		if (this.dead || Game.Instance.done || this.paused)
		{
			return;
		}
		this.Look();
		this.DrawGrabbing();
		this.UpdateTimescale();
		if (base.transform.position.y < -200f)
		{
			this.KillPlayer();
		}
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000045D8 File Offset: 0x000027D8
	private void MyInput()
	{
		if (this.dead || Game.Instance.done)
		{
			return;
		}
		this.x = Input.GetAxisRaw("Horizontal");
		this.y = Input.GetAxisRaw("Vertical");
		this.jumping = Input.GetButton("Jump");
		this.sprinting = Input.GetKey(KeyCode.LeftShift);
		this.crouching = Input.GetKey(KeyCode.LeftControl);
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			this.StartCrouch();
		}
		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			this.StopCrouch();
		}
		if (Input.GetKey(KeyCode.Mouse0))
		{
			if (this.detectWeapons.HasGun())
			{
				this.detectWeapons.Shoot(this.HitPoint());
			}
			else
			{
				this.GrabObject();
			}
		}
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			this.detectWeapons.StopUse();
			if (this.objectGrabbing)
			{
				this.StopGrab();
			}
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			this.detectWeapons.Pickup();
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			this.detectWeapons.Throw((this.HitPoint() - this.detectWeapons.weaponPos.position).normalized);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.Pause();
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00004724 File Offset: 0x00002924
	private void Pause()
	{
		if (this.dead)
		{
			return;
		}
		if (this.paused)
		{
			Time.timeScale = 1f;
			UIManger.Instance.DeadUI(false);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			this.paused = false;
			return;
		}
		this.paused = true;
		Time.timeScale = 0f;
		UIManger.Instance.DeadUI(true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00004793 File Offset: 0x00002993
	private void UpdateTimescale()
	{
		if (Game.Instance.done || this.paused || this.dead)
		{
			return;
		}
		Time.timeScale = Mathf.SmoothDamp(Time.timeScale, this.desiredTimeScale, ref this.timeScaleVel, 0.15f);
	}

	// Token: 0x06000091 RID: 145 RVA: 0x000047D2 File Offset: 0x000029D2
	private void GrabObject()
	{
		if (this.objectGrabbing == null)
		{
			this.StartGrab();
			return;
		}
		this.HoldGrab();
	}

	// Token: 0x06000092 RID: 146 RVA: 0x000047F0 File Offset: 0x000029F0
	private void DrawGrabbing()
	{
		if (!this.objectGrabbing)
		{
			return;
		}
		this.myGrabPoint = Vector3.Lerp(this.myGrabPoint, this.objectGrabbing.position, Time.deltaTime * 45f);
		this.myHandPoint = Vector3.Lerp(this.myHandPoint, this.grabJoint.connectedAnchor, Time.deltaTime * 45f);
		this.grabLr.SetPosition(0, this.myGrabPoint);
		this.grabLr.SetPosition(1, this.myHandPoint);
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00004880 File Offset: 0x00002A80
	private void StartGrab()
	{
		RaycastHit[] array = Physics.RaycastAll(this.playerCam.transform.position, this.playerCam.transform.forward, 8f, this.whatIsGrabbable);
		if (array.Length < 1)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			MonoBehaviour.print("testing on: " + array[i].collider.gameObject.layer);
			if (array[i].transform.GetComponent<Rigidbody>())
			{
				this.objectGrabbing = array[i].transform.GetComponent<Rigidbody>();
				this.grabPoint = array[i].point;
				this.grabJoint = this.objectGrabbing.gameObject.AddComponent<SpringJoint>();
				this.grabJoint.autoConfigureConnectedAnchor = false;
				this.grabJoint.minDistance = 0f;
				this.grabJoint.maxDistance = 0f;
				this.grabJoint.damper = 4f;
				this.grabJoint.spring = 40f;
				this.grabJoint.massScale = 5f;
				this.objectGrabbing.angularDrag = 5f;
				this.objectGrabbing.drag = 1f;
				this.previousLookdir = this.playerCam.transform.forward;
				this.grabLr = this.objectGrabbing.gameObject.AddComponent<LineRenderer>();
				this.grabLr.positionCount = 2;
				this.grabLr.startWidth = 0.05f;
				this.grabLr.material = new Material(Shader.Find("Sprites/Default"));
				this.grabLr.numCapVertices = 10;
				this.grabLr.numCornerVertices = 10;
				return;
			}
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00004A5C File Offset: 0x00002C5C
	private void HoldGrab()
	{
		this.grabJoint.connectedAnchor = this.playerCam.transform.position + this.playerCam.transform.forward * 5.5f;
		this.grabLr.startWidth = 0f;
		this.grabLr.endWidth = 0.0075f * this.objectGrabbing.velocity.magnitude;
		this.previousLookdir = this.playerCam.transform.forward;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00004AED File Offset: 0x00002CED
	private void StopGrab()
	{
		UnityEngine.Object.Destroy(this.grabJoint);
		UnityEngine.Object.Destroy(this.grabLr);
		this.objectGrabbing.angularDrag = 0.05f;
		this.objectGrabbing.drag = 0f;
		this.objectGrabbing = null;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00004B2C File Offset: 0x00002D2C
	private void StartCrouch()
	{
		float d = 400f;
		base.transform.localScale = new Vector3(1f, 0.5f, 1f);
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.5f, base.transform.position.z);
		if (this.rb.velocity.magnitude > 0.1f && this.grounded)
		{
			this.rb.AddForce(this.orientation.transform.forward * d);
			AudioManager.Instance.Play("StartSlide");
			AudioManager.Instance.Play("Slide");
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00004C08 File Offset: 0x00002E08
	private void StopCrouch()
	{
		base.transform.localScale = new Vector3(1f, 1.5f, 1f);
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z);
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00004C7A File Offset: 0x00002E7A
	private void StopGrapple()
	{
		UnityEngine.Object.Destroy(this.joint);
		this.grapplePoint = Vector3.zero;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00004C94 File Offset: 0x00002E94
	private void StartGrapple()
	{
		RaycastHit[] array = Physics.RaycastAll(this.playerCam.transform.position, this.playerCam.transform.forward, 70f, this.whatIsGround);
		if (array.Length < 1)
		{
			return;
		}
		this.grapplePoint = array[0].point;
		this.joint = base.gameObject.AddComponent<SpringJoint>();
		this.joint.autoConfigureConnectedAnchor = false;
		this.joint.connectedAnchor = this.grapplePoint;
		this.joint.spring = 6.5f;
		this.joint.damper = 2f;
		this.joint.maxDistance = Vector2.Distance(this.grapplePoint, base.transform.position) * 0.8f;
		this.joint.minDistance = Vector2.Distance(this.grapplePoint, base.transform.position) * 0.25f;
		this.joint.spring = 4.5f;
		this.joint.damper = 7f;
		this.joint.massScale = 4.5f;
		this.endPoint = this.gun.transform.GetChild(0).position;
		this.offsetMultiplier = 2f;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00004DF8 File Offset: 0x00002FF8
	private void DrawGrapple()
	{
		if (this.grapplePoint == Vector3.zero || this.joint == null)
		{
			this.lr.positionCount = 0;
			return;
		}
		this.lr.positionCount = 2;
		this.endPoint = Vector3.Lerp(this.endPoint, this.grapplePoint, Time.deltaTime * 15f);
		this.offsetMultiplier = Mathf.SmoothDamp(this.offsetMultiplier, 0f, ref this.offsetVel, 0.1f);
		int num = 100;
		this.lr.positionCount = num;
		Vector3 position = this.gun.transform.GetChild(0).position;
		float num2 = Vector3.Distance(this.endPoint, position);
		this.lr.SetPosition(0, position);
		this.lr.SetPosition(num - 1, this.endPoint);
		float num3 = num2;
		float num4 = 1f;
		for (int i = 1; i < num - 1; i++)
		{
			float num5 = (float)i / (float)num;
			float num6 = num5 * this.offsetMultiplier;
			float num7 = (Mathf.Sin(num6 * num3) - 0.5f) * num4 * (num6 * 2f);
			Vector3 normalized = (this.endPoint - position).normalized;
			float num8 = Mathf.Sin(num5 * 180f * 0.017453292f);
			float num9 = Mathf.Cos(this.offsetMultiplier * 90f * 0.017453292f);
			//Vector3 position2 = position + (this.endPoint - position) / (float)num * (float)i + (num9 * num7 * Vector2.Perpendicular(normalized) + this.offsetMultiplier * num8 * Vector3.down);
			Vector3 position2 = new Vector3(0, 0, 0);
			this.lr.SetPosition(i, position2);
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00004FCC File Offset: 0x000031CC
	private void WallRunning()
	{
		if (this.wallRunning)
		{
			this.rb.AddForce(-this.wallNormalVector * Time.deltaTime * this.moveSpeed);
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00005004 File Offset: 0x00003204
	private void FootSteps()
	{
		if (this.crouching || this.dead)
		{
			return;
		}
		if (this.grounded || this.wallRunning)
		{
			float num = 1.2f;
			float num2 = this.rb.velocity.magnitude;
			if (num2 > 20f)
			{
				num2 = 20f;
			}
			this.distance += num2;
			if (this.distance > 300f / num)
			{
				AudioManager.Instance.PlayFootStep();
				this.distance = 0f;
			}
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x0000508C File Offset: 0x0000328C
	private void Move()
	{
		if (this.dead)
		{
			return;
		}
		this.grounded = (Physics.OverlapSphere(this.groundChecker.position, 0.1f, this.whatIsGround).Length != 0);
		if (!this.touchingGround)
		{
			this.grounded = false;
		}
		Vector2 vector = this.FindVelRelativeToLook();
		float num = vector.x;
		float num2 = vector.y;
		this.FootSteps();
		this.CounterMovement(this.x, this.y, vector);
		if (this.readyToJump && this.jumping)
		{
			this.Jump();
		}
		float num3 = this.walkSpeed;
		if (this.sprinting)
		{
			num3 = this.runSpeed;
		}
		if (this.crouching && this.grounded && this.readyToJump)
		{
			this.rb.AddForce(Vector3.down * Time.deltaTime * 2000f);
			return;
		}
		if (this.x > 0f && num > num3)
		{
			this.x = 0f;
		}
		if (this.x < 0f && num < -num3)
		{
			this.x = 0f;
		}
		if (this.y > 0f && num2 > num3)
		{
			this.y = 0f;
		}
		if (this.y < 0f && num2 < -num3)
		{
			this.y = 0f;
		}
		float d = 1f;
		float d2 = 1f;
		if (!this.grounded)
		{
			d = 0.5f;
		}
		if (this.grounded && this.crouching)
		{
			d2 = 0f;
		}
		this.rb.AddForce(this.orientation.transform.forward * this.y * this.moveSpeed * Time.deltaTime * d * d2);
		this.rb.AddForce(this.orientation.transform.right * this.x * this.moveSpeed * Time.deltaTime * d);
		this.SpeedLines();
	}

	// Token: 0x0600009E RID: 158 RVA: 0x000052AC File Offset: 0x000034AC
	private void SpeedLines()
	{
		float num = Vector3.Angle(this.rb.velocity, this.playerCam.transform.forward) * 0.15f;
		if (num < 1f)
		{
			num = 1f;
		}
		float rateOverTimeMultiplier = this.rb.velocity.magnitude / num;
		if (this.grounded && !this.wallRunning)
		{
			rateOverTimeMultiplier = 0f;
		}
		this.psEmission.rateOverTimeMultiplier = rateOverTimeMultiplier;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00005328 File Offset: 0x00003528
	private void CameraShake()
	{
		float num = this.rb.velocity.magnitude / 9f;
		CameraShaker.Instance.ShakeOnce(num, 0.1f * num, 0.25f, 0.2f);
		base.Invoke("CameraShake", 0.2f);
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0000537C File Offset: 0x0000357C
	private void ResetJump()
	{
		this.readyToJump = true;
		MonoBehaviour.print("reset");
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00005390 File Offset: 0x00003590
	private void Jump()
	{
		if (this.grounded || this.wallRunning)
		{
			Vector3 velocity = this.rb.velocity;
			this.rb.velocity = new Vector3(velocity.x, 0f, velocity.z);
			this.readyToJump = false;
			this.rb.AddForce(Vector2.up * this.jumpForce * 1.5f);
			this.rb.AddForce(this.wallNormalVector * this.jumpForce * 0.5f);
			if (this.wallRunning)
			{
				this.rb.AddForce(this.wallNormalVector * this.jumpForce * 1.5f);
			}
			base.Invoke("ResetJump", this.jumpCooldown);
			if (this.wallRunning)
			{
				this.wallRunning = false;
			}
			AudioManager.Instance.PlayJump();
		}
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x0000548C File Offset: 0x0000368C
	private void Look()
	{
		float num = Input.GetAxis("Mouse X") * this.sensitivity * Time.fixedDeltaTime * this.sensMultiplier;
		float num2 = Input.GetAxis("Mouse Y") * this.sensitivity * Time.fixedDeltaTime * this.sensMultiplier;
		Vector3 eulerAngles = this.playerCam.transform.localRotation.eulerAngles;
		this.desiredX = eulerAngles.y + num;
		this.xRotation -= num2;
		this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);
		this.FindWallRunRotation();
		this.actualWallRotation = Mathf.SmoothDamp(this.actualWallRotation, this.wallRunRotation, ref this.wallRotationVel, 0.2f);
		this.playerCam.transform.localRotation = Quaternion.Euler(this.xRotation, this.desiredX, this.actualWallRotation);
		this.orientation.transform.localRotation = Quaternion.Euler(0f, this.desiredX, 0f);
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x0000559C File Offset: 0x0000379C
	private void FindWallRunRotation()
	{
		if (!this.wallRunning)
		{
			this.wallRunRotation = 0f;
			return;
		}
		Vector3 normalized = new Vector3(0f, this.playerCam.transform.rotation.y, 0f).normalized;
		new Vector3(0f, 0f, 1f);
		float current = this.playerCam.transform.rotation.eulerAngles.y;
		if (Math.Abs(this.wallNormalVector.x - 1f) >= 0.1f)
		{
			if (Math.Abs(this.wallNormalVector.x - -1f) >= 0.1f)
			{
				if (Math.Abs(this.wallNormalVector.z - 1f) >= 0.1f)
				{
					if (Math.Abs(this.wallNormalVector.z - -1f) < 0.1f)
					{
					}
				}
			}
		}
		float target = Vector3.SignedAngle(new Vector3(0f, 0f, 1f), this.wallNormalVector, Vector3.up);
		float num = Mathf.DeltaAngle(current, target);
		this.wallRunRotation = -(num / 90f) * 15f;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x000056F4 File Offset: 0x000038F4
	private void CounterMovement(float x, float y, Vector2 mag)
	{
		if (!this.grounded)
		{
			return;
		}
		float d = 0.2f;
		if (this.crouching)
		{
			this.rb.AddForce(this.moveSpeed * Time.deltaTime * -this.rb.velocity.normalized * 0.045f);
			return;
		}
		if (Math.Abs(x) < 0.05f || (mag.x < 0f && x > 0f) || (mag.x > 0f && x < 0f))
		{
			this.rb.AddForce(this.moveSpeed * this.orientation.transform.right * Time.deltaTime * -mag.x * d);
		}
		if (Math.Abs(y) < 0.05f || (mag.y < 0f && y > 0f) || (mag.y > 0f && y < 0f))
		{
			this.rb.AddForce(this.moveSpeed * this.orientation.transform.forward * Time.deltaTime * -mag.y * d);
		}
		if (Mathf.Sqrt(Mathf.Pow(this.rb.velocity.x, 2f) + Mathf.Pow(this.rb.velocity.z, 2f)) > 20f)
		{
			float num = this.rb.velocity.y;
			Vector3 vector = this.rb.velocity.normalized * 20f;
			this.rb.velocity = new Vector3(vector.x, num, vector.z);
		}
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x000058D8 File Offset: 0x00003AD8
	public Vector2 FindVelRelativeToLook()
	{
		float current = this.orientation.transform.eulerAngles.y;
		float target = Mathf.Atan2(this.rb.velocity.x, this.rb.velocity.z) * 57.29578f;
		float num = Mathf.DeltaAngle(current, target);
		float num2 = 90f - num;
		float magnitude = this.rb.velocity.magnitude;
		float num3 = magnitude * Mathf.Cos(num * 0.017453292f);
		return new Vector2(magnitude * Mathf.Cos(num2 * 0.017453292f), num3);
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0000596C File Offset: 0x00003B6C
	private void OnCollisionEnter(Collision other)
	{
		int layer = other.gameObject.layer;
		if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			CameraShaker.Instance.ShakeOnce(5.5f * GameState.Instance.cameraShake, 1.2f, 0.2f, 0.3f);
			if (this.wallRunning && other.contacts[0].normal.y == -1f)
			{
				MonoBehaviour.print("ROOF");
				return;
			}
			this.wallNormalVector = other.contacts[0].normal;
			MonoBehaviour.print("nv: " + this.wallNormalVector);
			AudioManager.Instance.PlayLanding();
			if (Math.Abs(this.wallNormalVector.y) < 0.1f)
			{
				this.StartWallRun();
			}
			this.airborne = false;
		}
		if (layer == LayerMask.NameToLayer("Enemy"))
		{
			if (this.grounded && !this.crouching)
			{
				return;
			}
			if (this.rb.velocity.magnitude < 3f)
			{
				return;
			}
			Enemy enemy = (Enemy)other.transform.root.GetComponent(typeof(Enemy));
			if (!enemy)
			{
				return;
			}
			if (enemy.IsDead())
			{
				return;
			}
			UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
			RagdollController ragdollController = (RagdollController)other.transform.root.GetComponent(typeof(RagdollController));
			if (this.grounded && this.crouching)
			{
				ragdollController.MakeRagdoll(this.rb.velocity * 1.2f * 34f);
			}
			else
			{
				ragdollController.MakeRagdoll(this.rb.velocity.normalized * 250f);
			}
			this.rb.AddForce(this.rb.velocity.normalized * 2f, ForceMode.Impulse);
			enemy.DropGun(this.rb.velocity.normalized * 2f);
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00005BB4 File Offset: 0x00003DB4
	private void StartWallRun()
	{
		if (this.wallRunning)
		{
			MonoBehaviour.print("stopping since wallrunning");
			return;
		}
		if (this.touchingGround)
		{
			MonoBehaviour.print("stopping since grounded");
			return;
		}
		MonoBehaviour.print("got through");
		float d = 20f;
		this.wallRunning = true;
		this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
		this.rb.AddForce(Vector3.up * d, ForceMode.Impulse);
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00005C4C File Offset: 0x00003E4C
	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			if (Math.Abs(this.wallNormalVector.y) < 0.1f)
			{
				MonoBehaviour.print("oof");
				this.wallRunning = false;
				this.wallNormalVector = Vector3.up;
			}
			else
			{
				this.touchingGround = false;
			}
			this.airborne = true;
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
		{
			this.touchingGround = false;
		}
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00005CD4 File Offset: 0x00003ED4
	private void OnCollisionStay(Collision other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && Math.Abs(other.contacts[0].normal.y) > 0.1f)
		{
			this.touchingGround = true;
			this.wallRunning = false;
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
		{
			this.touchingGround = true;
		}
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00005D46 File Offset: 0x00003F46
	public Vector3 GetVelocity()
	{
		return this.rb.velocity;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00005D53 File Offset: 0x00003F53
	public float GetFallSpeed()
	{
		return this.rb.velocity.y;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00005D65 File Offset: 0x00003F65
	public Vector3 GetGrapplePoint()
	{
		return this.detectWeapons.GetGrapplerPoint();
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00005D72 File Offset: 0x00003F72
	public Collider GetPlayerCollider()
	{
		return this.playerCollider;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00005D7A File Offset: 0x00003F7A
	public Transform GetPlayerCamTransform()
	{
		return this.playerCam.transform;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00005D88 File Offset: 0x00003F88
	public Vector3 HitPoint()
	{
		RaycastHit[] array = Physics.RaycastAll(this.playerCam.transform.position, this.playerCam.transform.forward, (float)this.whatIsHittable);
		if (array.Length < 1)
		{
			return this.playerCam.transform.position + this.playerCam.transform.forward * 100f;
		}
		if (array.Length > 1)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
				{
					return array[i].point;
				}
			}
		}
		return array[0].point;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00005E4C File Offset: 0x0000404C
	public float GetRecoil()
	{
		return this.detectWeapons.GetRecoil();
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00005E5C File Offset: 0x0000405C
	public void KillPlayer()
	{
		if (Game.Instance.done)
		{
			return;
		}
		CameraShaker.Instance.ShakeOnce(3f * GameState.Instance.cameraShake, 2f, 0.1f, 0.6f);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		UIManger.Instance.DeadUI(true);
		Timer.Instance.Stop();
		this.dead = true;
		this.rb.freezeRotation = false;
		this.playerCollider.material = this.deadMat;
		this.detectWeapons.Throw(Vector3.zero);
		this.paused = false;
		this.ResetSlowmo();
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00005F02 File Offset: 0x00004102
	public void Respawn()
	{
		this.detectWeapons.StopUse();
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00005F0F File Offset: 0x0000410F
	public void Slowmo(float timescale, float length)
	{
		if (!GameState.Instance.shake)
		{
			return;
		}
		base.CancelInvoke("Slowmo");
		this.desiredTimeScale = timescale;
		base.Invoke("ResetSlowmo", length);
		AudioManager.Instance.Play("SlowmoStart");
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00005F4B File Offset: 0x0000414B
	private void ResetSlowmo()
	{
		this.desiredTimeScale = 1f;
		AudioManager.Instance.Play("SlowmoEnd");
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00005F67 File Offset: 0x00004167
	public bool IsCrouching()
	{
		return this.crouching;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00005F6F File Offset: 0x0000416F
	public bool HasGun()
	{
		return this.detectWeapons.HasGun();
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00005F7C File Offset: 0x0000417C
	public bool IsDead()
	{
		return this.dead;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00005F84 File Offset: 0x00004184
	public Rigidbody GetRb()
	{
		return this.rb;
	}

	// Token: 0x0400005A RID: 90
	public GameObject spawnWeapon;

	// Token: 0x0400005B RID: 91
	private float sensitivity = 50f;

	// Token: 0x0400005C RID: 92
	private float sensMultiplier = 1f;

	// Token: 0x0400005D RID: 93
	private bool dead;

	// Token: 0x0400005E RID: 94
	public PhysicMaterial deadMat;

	// Token: 0x0400005F RID: 95
	public Transform playerCam;

	// Token: 0x04000060 RID: 96
	public Transform orientation;

	// Token: 0x04000061 RID: 97
	public Transform gun;

	// Token: 0x04000062 RID: 98
	private float xRotation;

	// Token: 0x04000063 RID: 99
	public Rigidbody rb;

	// Token: 0x04000064 RID: 100
	private float moveSpeed = 4500f;

	// Token: 0x04000065 RID: 101
	private float walkSpeed = 20f;

	// Token: 0x04000066 RID: 102
	private float runSpeed = 10f;

	// Token: 0x04000067 RID: 103
	public bool grounded;

	// Token: 0x04000068 RID: 104
	public Transform groundChecker;

	// Token: 0x04000069 RID: 105
	public LayerMask whatIsGround;

	// Token: 0x0400006A RID: 106
	private bool readyToJump;

	// Token: 0x0400006B RID: 107
	private float jumpCooldown = 0.2f;

	// Token: 0x0400006C RID: 108
	private float jumpForce = 550f;

	// Token: 0x0400006D RID: 109
	private float x;

	// Token: 0x0400006E RID: 110
	private float y;

	// Token: 0x0400006F RID: 111
	private bool jumping;

	// Token: 0x04000070 RID: 112
	private bool sprinting;

	// Token: 0x04000071 RID: 113
	private bool crouching;

	// Token: 0x04000072 RID: 114
	public LineRenderer lr;

	// Token: 0x04000073 RID: 115
	private Vector3 grapplePoint;

	// Token: 0x04000074 RID: 116
	private SpringJoint joint;

	// Token: 0x04000075 RID: 117
	private Vector3 normalVector;

	// Token: 0x04000076 RID: 118
	private Vector3 wallNormalVector;

	// Token: 0x04000077 RID: 119
	private bool wallRunning;

	// Token: 0x04000078 RID: 120
	private Vector3 wallRunPos;

	// Token: 0x04000079 RID: 121
	private DetectWeapons detectWeapons;

	// Token: 0x0400007A RID: 122
	public ParticleSystem ps;

	// Token: 0x0400007B RID: 123
	private ParticleSystem.EmissionModule psEmission;

	// Token: 0x0400007C RID: 124
	private Collider playerCollider;

	// Token: 0x0400007E RID: 126
	public bool paused;

	// Token: 0x0400007F RID: 127
	public LayerMask whatIsGrabbable;

	// Token: 0x04000080 RID: 128
	private Rigidbody objectGrabbing;

	// Token: 0x04000081 RID: 129
	private Vector3 previousLookdir;

	// Token: 0x04000082 RID: 130
	private Vector3 grabPoint;

	// Token: 0x04000083 RID: 131
	private float dragForce = 700000f;

	// Token: 0x04000084 RID: 132
	private SpringJoint grabJoint;

	// Token: 0x04000085 RID: 133
	private LineRenderer grabLr;

	// Token: 0x04000086 RID: 134
	private Vector3 myGrabPoint;

	// Token: 0x04000087 RID: 135
	private Vector3 myHandPoint;

	// Token: 0x04000088 RID: 136
	private Vector3 endPoint;

	// Token: 0x04000089 RID: 137
	private Vector3 grappleVel;

	// Token: 0x0400008A RID: 138
	private float offsetMultiplier;

	// Token: 0x0400008B RID: 139
	private float offsetVel;

	// Token: 0x0400008C RID: 140
	private float distance;

	// Token: 0x0400008D RID: 141
	private float actualWallRotation;

	// Token: 0x0400008E RID: 142
	private float wallRotationVel;

	// Token: 0x0400008F RID: 143
	private float desiredX;

	// Token: 0x04000090 RID: 144
	private float wallRunRotation;

	// Token: 0x04000091 RID: 145
	private bool airborne;

	// Token: 0x04000092 RID: 146
	private bool touchingGround;

	// Token: 0x04000093 RID: 147
	public LayerMask whatIsHittable;

	// Token: 0x04000094 RID: 148
	private float desiredTimeScale = 1f;

	// Token: 0x04000095 RID: 149
	private float timeScaleVel;
}
