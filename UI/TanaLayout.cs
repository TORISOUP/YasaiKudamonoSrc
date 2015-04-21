using System;
using UnityEngine;
using System.Collections;
using UniRx;

namespace Ahoge
{
    /// <summary>
    /// 棚を再配置する
    /// </summary>
    public class TanaLayout : MonoBehaviour
    {
        private Vector3 _initPosition;
        private Quaternion _initRotation;
        private new Rigidbody _rigidbody;

        private void Awake()
        {
            //起動時に初期位置を記憶
            _initPosition = this.transform.position;
            _initRotation = this.transform.rotation;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            //Reset通知が来て1.5秒後に再配置する（床が回転しているため）
            GameRestartManager.Instance
                .RestartObservable
                .Delay(TimeSpan.FromMilliseconds(1500))
                .Subscribe(_ =>
                {
                    transform.position = _initPosition;
                    transform.rotation = _initRotation;
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.angularVelocity = Vector3.zero;
                });
        }

        private void FixedUpdate()
        {
            _rigidbody.WakeUp();
        }
    }
}