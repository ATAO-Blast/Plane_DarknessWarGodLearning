using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarknessWarGodLearning
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Animator playerAnimator;
        [SerializeField] CharacterController characterController;

        private Transform camTrans;
        private Vector2 dir = Vector2.zero;
        private bool isMove = false;
        private Vector3 camOffset;
        private float targetBlend;
        private float currentBlend;

        public Vector2 Dir { get => dir; set 
            { 
                if(value == Vector2.zero)
                {
                    isMove = false;
                }
                else
                {
                    isMove = true;
                }
                dir = value; 
            } }

        public void Init()
        {
            camTrans = Camera.main.transform;
            camOffset = transform.position - camTrans.position;
        }
        private void Update()
        {
            /*
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector2 _dir = new Vector2(h, v).normalized;

            if (_dir != Vector2.zero)
            {
                Dir = _dir;
                SetBlend(Constants.BlendWalk);
            }
            else
            {
                Dir = Vector2.zero; 
                SetBlend(Constants.BlendIdle);
            }
            */
            if(currentBlend != targetBlend)
            {
                UpdateMixBlend();
            }

            if (isMove)
            {
                //设置方向
                SetDir();

                //产生移动
                SetMove();

                //摄像机跟随
                SetCam();
            }
        }
        private void SetDir()
        {
            float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
            Vector3 eularAngle = new Vector3(0, angle, 0);
            transform.localEulerAngles = eularAngle;
        }
        private void SetMove()
        {
            characterController.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
        }
        private void SetCam()
        {
            if (camTrans != null)
            {
                camTrans.position = transform.position - camOffset;
            }
        }
        public void SetBlend(float blend)
        {
            targetBlend = blend;
        }
        private void UpdateMixBlend()
        {
            if(Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed*Time.deltaTime)
            {
                currentBlend = targetBlend;
            }
            else if(currentBlend > targetBlend)
            {
                currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
            }
            else
            {
                currentBlend += Constants.AccelerSpeed * Time.deltaTime;
            }
            playerAnimator.SetFloat("Blend", currentBlend);
            
        }
    }
}