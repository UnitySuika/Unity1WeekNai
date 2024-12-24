using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatenoWorks.Novel
{
    /// <summary>
    /// �A�}�K�~�i����̓�����Q�l�ɂ��܂����B
    /// URL�Fhttps://www.youtube.com/watch?v=0LC5BgwPKOc&t=497s
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        /// <summary>
        /// �j�����ɃQ�[���I�u�W�F�N�g���Ɣj�����邩
        /// </summary>
        public virtual bool IsDestroyGameObject => true;

        public virtual bool IsDestroyFollowing => true;

        public static T Instance;

        /// <summary>
        /// �V���O���g�����L����
        /// </summary>
        public static bool IsValid => Instance != null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                Instance.OverrideAwake();
            }
            else if (IsDestroyGameObject)
            {
                if (IsDestroyFollowing)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(Instance.gameObject);
                    Instance.OverrideAwake();
                }
            }
            else
            {
                if (IsDestroyFollowing)
                {
                    Destroy(this);
                }
                else
                {
                    Destroy(Instance);
                    Instance.OverrideAwake();
                }
            }
        }

        /// <summary>
        /// �h���N���X�p��Awake
        /// </summary>
        protected virtual void OverrideAwake() { }

        public void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>
        /// �h���N���X�p��OnDestroy
        /// </summary>
        protected virtual void OverrideDestory() { }
    }
}