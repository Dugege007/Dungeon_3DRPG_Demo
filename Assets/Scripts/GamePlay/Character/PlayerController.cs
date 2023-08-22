using FrameworkDesign;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * 创建人：杜
 * 功能说明：玩家控制
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class PlayerController : MonoBehaviour, IController
    {
        [Header("基础组件")]
        private CameraController mCameraController;
        private Rigidbody mRigidbody;
        private Animator mAnim;
        private RuntimeAnimatorController mCurrentRAC;
        private GameObject mEquipWeapon;
        private GameObject mUnEquipWeapon;
        private ParticleSystem mEquipPS;

        public RuntimeAnimatorController RAC;

        // 提供给外部
        [HideInInspector]
        public Transform mCameraFollowTrans;
        [HideInInspector]
        public Transform mSummonedFollowTrans;

        // 移动相关
        private float horizontal;
        private float vertical;

        private float moveSpeed = 3f;
        private float sprintSpeed = 1f;
        private Vector3 moveDir;
        private Quaternion rotationOffset;
        private float rotateSpeed = 7.5f;
        private float jumpForce = 300f;

        [Header("动画状态")]
        public bool canInput;
        public bool canMove;
        private bool isSprint;
        public bool canRotate;
        public bool canJump;
        public bool canAttack;
        public bool canEquip;
        private bool isEquip;
        public bool canTalk;

        public bool isCombat;
        public bool isInvincible;

        private bool canCombo;
        private int comboNum;

        private bool isPointerOnUI;

        public IArchitecture GetArchiteccture()
        {
            return DungeonRPG.Interface;
        }

        private void Awake()
        {
            mCameraController = Camera.main.GetComponent<CameraController>();
            mRigidbody = GetComponent<Rigidbody>();
            mAnim = GetComponent<Animator>();
            mCurrentRAC = mAnim.runtimeAnimatorController;
            mEquipWeapon = transform.DeepFindChild("SM_Wep_Crystal_Axe_01_eq").gameObject;  // 深度查找
            mUnEquipWeapon = transform.Find("Root/Hips/SM_Wep_Crystal_Axe_01_unEq").gameObject;
            mEquipPS = transform.Find("Root/Hips/FX_Fireworks_Orange_Weapon").GetComponent<ParticleSystem>();
            mCameraFollowTrans = transform.Find("CameraFollowTrans");
            mSummonedFollowTrans = transform.Find("SummonedFollowTrans");
        }

        private void Start()
        {
            //mAnim.CrossFade("WakeUp", 0.1f);

            UnlockInput();
            ResetCombo();

            this.RegisterEvent<OnPointerEnterUIEvent>(OnPointerEnterUI).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<OnPointerExitUIEvent>(OnPointerExitUI).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (!canInput) return;

            if (Input.GetKeyDown(KeyCode.F) && canTalk && !isCombat) Talk();
            if (Input.GetKeyDown(KeyCode.G) && !isCombat) Gather();

            if (horizontal < 0.1f && horizontal > -0.1f && vertical < 0.1f && vertical > -0.1f)
            {
                isSprint = false;
                sprintSpeed = 1f;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) SwitchSprint();
            if (Input.GetKeyDown(KeyCode.Space) && canJump) Jump();

            if (Input.GetMouseButtonDown(0) && canAttack && !isPointerOnUI) Attack();

            if (Input.GetKeyDown(KeyCode.E) && canEquip) SwitchCombatState();
            if (Input.GetKeyDown(KeyCode.C) && canEquip) SwitchWeapon();
        }

        private void FixedUpdate()
        {
            if (!canInput) return;

            // 获取轴向输入
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            rotationOffset = Quaternion.Euler(0f, mCameraController.currentAngle + 45f, 0f);
            // 设置移动方向
            moveDir = rotationOffset * new Vector3(horizontal, 0, vertical);

            if (canMove) Move();
            if (canRotate) Rotate();
        }

        private void OnPointerEnterUI(OnPointerEnterUIEvent obj)
        {
            isPointerOnUI = true;
        }

        private void OnPointerExitUI(OnPointerExitUIEvent obj)
        {
            isPointerOnUI = false;
        }


        #region 移动
        /// <summary>
        /// 移动
        /// </summary>
        private void Move()
        {
            if (!isCombat) // 正常移动
            {
                // 开启旋转
                if (!canRotate) canRotate = true;

                mRigidbody.MovePosition(mRigidbody.position + moveDir.normalized * moveSpeed * sprintSpeed * Time.fixedDeltaTime);

                // 设置动画
                mAnim.SetBool("IsCombat", false);
                mAnim.SetFloat("MoveSpeed", moveDir.magnitude * sprintSpeed);
            }
            else // 攻击状态
            {
                // 关闭旋转
                if (canRotate) canRotate = false;
                //TODO 锁定目标

                // 平行移动
                rotationOffset = Quaternion.Euler(0f, mCameraController.currentAngle + 45f - transform.rotation.eulerAngles.y, 0f); // 这里玩家自身的角度要用 rotation.eulerAngles 获取
                                                                                                                                    // 设置移动方向
                moveDir = rotationOffset * new Vector3(horizontal, 0, vertical);
                moveDir.Normalize();

                // 使用动画自身的移动
                // 设置动画
                mAnim.SetBool("IsCombat", true);
                mAnim.SetFloat("MoveX", moveDir.x * sprintSpeed);
                mAnim.SetFloat("MoveY", moveDir.z * sprintSpeed);
            }
        }

        /// <summary>
        /// 切换加速
        /// </summary>
        private void SwitchSprint()
        {
            isSprint = !isSprint;

            if (isSprint) sprintSpeed = 2f;
            else sprintSpeed = 1f;
        }

        /// <summary>
        /// 旋转
        /// </summary>
        private void Rotate()
        {
            if (!isCombat)
            {
                moveDir = rotationOffset * new Vector3(horizontal, 0, vertical);
                Quaternion quaternionDir = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, moveDir, rotateSpeed * Time.fixedDeltaTime, 0));

                mRigidbody.MoveRotation(quaternionDir);
            }
            else
            {
                //TODO 锁定目标

                // 面向鼠标点击位置
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 50f))
                {
                    Vector3 targetPos = hit.point;
                    targetPos.y = transform.position.y;
                    transform.LookAt(targetPos);
                }
            }
        }

        /// <summary>
        /// 跳跃
        /// </summary>
        private void Jump()
        {
            if (!isCombat)
            {
                canJump = false;
                mRigidbody.AddForce(Vector3.up * jumpForce);
                mAnim.SetBool("IsOnGround", canJump);
                mAnim.CrossFade("Jump_Up", 0.1f);
            }
            else
            {
                mAnim.CrossFade("Roll", 0f);
            }
        }
        #endregion

        #region 战斗
        /// <summary>
        /// 普通攻击
        /// </summary>
        private void Attack()
        {
            if (!isCombat)
            {
                isCombat = true;
                return;
            }

            canAttack = false;

            if (comboNum == 0)
            {
                comboNum++;
                mAnim.SetInteger("ComboNum", comboNum);
            }

            if (canCombo)
            {
                comboNum++;
                if (comboNum > 3)
                {
                    ResetCombo();
                }
                mAnim.SetInteger("ComboNum", comboNum);
                EndCombo();
            }

            Debug.Log("当前连击次数：" + comboNum);
        }

        /// <summary>
        /// 动画事件：开始连击
        /// </summary>
        private void StartCombo()
        {
            canCombo = true;
            canAttack = true;
        }

        /// <summary>
        /// 动画事件：连击结束
        /// </summary>
        private void EndCombo()
        {
            canCombo = false;
        }

        /// <summary>
        /// 动画事件：重置连击
        /// </summary>
        private void ResetCombo()
        {
            comboNum = 0;
            mAnim.SetInteger("ComboNum", comboNum);
            EndCombo();
        }

        private void SwitchCombatState()
        {
            isCombat = !isCombat;
        }

        private void SwitchWeapon()
        {
            mAnim.runtimeAnimatorController = RAC;
            RAC = mCurrentRAC;
            mCurrentRAC = mAnim.runtimeAnimatorController;

            isCombat = false;
            isEquip = !isEquip;
            mEquipPS.Play();
            if (isEquip)
                mUnEquipWeapon.gameObject.SetActive(true);
            else
            {
                mEquipWeapon.gameObject.SetActive(false);
                mUnEquipWeapon.gameObject.SetActive(false);
            }
        }
        #endregion

        #region 受击
        private void GetHit()
        {
            if (isInvincible) return;

        }

        private void StartInvincible()
        {
            isInvincible = true;
        }

        private void EndInvincible()
        {
            isInvincible = false;
        }


        #endregion

        #region 互动
        /// <summary>
        /// 对话
        /// </summary>
        public void Talk()
        {
            this.SendCommand<DialogueStartCommand>();
            mAnim.CrossFade("Talk", 0.1f);
        }

        /// <summary>
        /// 采集
        /// </summary>
        private void Gather()
        {
            mAnim.CrossFade("Gathering", 0f);
        }
        #endregion

        #region 动画
        /// <summary>
        /// 解锁全部行为
        /// </summary>
        public void UnlockInput()
        {
            canInput = canMove = canRotate = canJump = canAttack = canEquip = canTalk = true;
        }

        private void EquipWeapon()
        {
            mEquipWeapon.gameObject.SetActive(true);
            mUnEquipWeapon.gameObject.SetActive(false);
        }

        private void UnequipWeapon()
        {
            mEquipWeapon.gameObject.SetActive(false);
            mUnEquipWeapon.gameObject.SetActive(true);
        }

        private void PlayWalkAudio()
        {
            AudioManager.Instance.PlayWalkSFX();
        }

        private void PlayJumpAudio()
        {
            AudioManager.Instance.PlayJumpSFX();
        }

        private void PlayEquipAudio()
        {
            AudioManager.Instance.PlayEquipSFX();
        }

        private void PlayAttackAudio()
        {
            if (isEquip)
                AudioManager.Instance.PlayAxeSFX();
            else
                AudioManager.Instance.PlayPunchSFX();
        }
        #endregion

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.transform.parent.CompareTag("Ground"))
            {
                canJump = true;
                mAnim.SetBool("IsOnGround", canJump);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC") && transform.IsFacingTarget(other.transform))
            {
                canTalk = true;
            }
        }
    }
}
