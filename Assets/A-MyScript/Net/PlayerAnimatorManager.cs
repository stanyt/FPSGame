
using Photon.Pun;
using UnityEngine;

namespace Com.MyCompany.MyGunGame
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        private Animator _animator;
        private int animationSpeedHashCode;
        private int animationDirectionHashCode;
        private int animationJumpHashCode;
        
        [SerializeField]
        private float directionDampTime = 0.25f;

        private void Start()
        {
            if (!TryGetComponent(out _animator))
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }

            animationSpeedHashCode = Animator.StringToHash("Speed");
            animationDirectionHashCode = Animator.StringToHash("Direction");
            animationJumpHashCode = Animator.StringToHash("Jump");
        }

        private void Update()
        {
            if(!_animator) return;
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            if (v < 0)
            {
                v = 0;//没有倒退动画
            }

            _animator.SetFloat(animationSpeedHashCode, v * v + h * h);//平方，它始终是一个正的绝对值，并增加了一些缓和
            _animator.SetFloat(animationDirectionHashCode, h, directionDampTime, Time.deltaTime);// 阻尼时间是有意义的：它是达到所需值需要多长时间
            // deal with Jumping
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            // only allow jumping if we are running.
            if (stateInfo.IsName("Base Layer.Run"))//isName的使用方法
            {
                // When using trigger parameter
                if (Input.GetButtonDown("Jump"))
                {
                    _animator.SetTrigger(animationJumpHashCode);
                }
            }
        }
    }
}
