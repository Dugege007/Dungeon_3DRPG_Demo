using FrameworkDesign;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：相机控制
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class CameraController : MonoBehaviour, IController
    {
        private Transform mPlayerTrans;

        // 摄像机移动的平滑速度
        private float moveSmoothSpeed = 3f;
        // 摄像机和玩家之间的距离
        public Vector3 offset;
        // 摄像机当前旋转角度
        [HideInInspector]
        public float currentAngle = 0f;
        // 每次旋转角度
        public float rotateAngle = 45f;

        public IArchitecture GetArchiteccture()
        {
            return DungeonRPG.Interface;
        }

        private void Update()
        {
            // 获取 Player 引用
            if (mPlayerTrans == null)
            {
                var playerGameObj = GameObject.FindWithTag("Player");
                if (playerGameObj != null)
                    mPlayerTrans = playerGameObj.transform;
                else
                    return;
            }
        }

        private void LateUpdate()
        {
            // 按下4或6键，计算新的旋转角度
            if (Input.GetKeyDown(KeyCode.Keypad4))
                currentAngle += rotateAngle;
            else if (Input.GetKeyDown(KeyCode.Keypad6))
                currentAngle -= rotateAngle;

            // 将摄像机绕玩家旋转
            Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);
            // 旋转偏移
            Vector3 rotatedOffset = rotation * offset;
            // 摄像机的目标位置
            Vector3 desiredPosition = mPlayerTrans.position + rotatedOffset;
            // 平滑插值移动摄像机到目标位置
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, moveSmoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            // 将摄像机朝向玩家
            transform.LookAt(mPlayerTrans);
        }

    }
}
