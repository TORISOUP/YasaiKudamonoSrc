using System;
using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Ahoge
{
    /// <summary>
    /// タイトル画面で最初に画面をクリックされた時の処理
    /// </summary>
    public class GameStartManager : MonoBehaviour
    {

        private void Start()
        {
            //自分自身を消してゲームリスタートを叩く
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .First()
                .Do(_=> Destroy(this.gameObject))
                .Delay(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ =>
                {
                    GameRestartManager.Instance.GameReset();

                });
        }

    }
}