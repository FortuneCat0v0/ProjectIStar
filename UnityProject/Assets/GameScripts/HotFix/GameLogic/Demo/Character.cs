using UnityEngine;

namespace GameLogic
{
    public class Character : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();

            _animator.SetFloat("Y", -1f);
        }
    }
}