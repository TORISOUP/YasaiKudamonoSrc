using System;
using UnityEngine;
using System.Collections;
using UniRx;

namespace Ahoge
{

    /// <summary>
    /// カメラ（自機）を動かす
    /// </summary>
    public class CameraMover : MonoBehaviour
    {

        [SerializeField] private float movableDistance = 1.0f;
        private Vector3 _startPosition;

        private void Start()
        {
            //初期位置記憶
            _startPosition = this.transform.position;

            //リセット時には1.5秒後に場所を戻す（床が回ってるから）
            GameRestartManager.Instance
                .RestartObservable
                .Delay(TimeSpan.FromMilliseconds(1500))
                .Subscribe(_ => this.transform.position = _startPosition);
        }

        private void Update()
        {
            //カメラ移動
            var inputVector = new Vector3(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
            var normalized = inputVector.normalized;
            var nextPosition = transform.position + normalized*Time.deltaTime;

            var nextX = Mathf.Clamp(nextPosition.x, _startPosition.x - movableDistance, _startPosition.x + movableDistance);
            var nextZ = Mathf.Clamp(nextPosition.z, _startPosition.z - movableDistance, _startPosition.z + movableDistance);

            this.transform.position = new Vector3(nextX, transform.position.y, nextZ);

        }
    }
}