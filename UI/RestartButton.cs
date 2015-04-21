using System;
using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace Ahoge
{
    /// <summary>
    /// ゲーム終了時のRetryボタン
    /// </summary>
    public class RestartButton : MonoBehaviour
    {
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();

            button.OnClickAsObservable()
                .Subscribe(_ => GameRestartManager.Instance.GameReset());

            //ゲーム終了直後の誤爆防止のために、ゲーム終了から１秒まって押せるようにする
            GameEndManager.Instance
                .GameEneObservable
                .Do(_ => button.interactable = false)
                .Delay(TimeSpan.FromSeconds(1))
                .Subscribe(_ => button.interactable = true);
        }

    }
}