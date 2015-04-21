using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace Ahoge
{
    /// <summary>
    /// ゲーム中のスコア表示
    /// </summary>
    public class ScorePresenter : MonoBehaviour
    {
        [SerializeField] private string format = "{0:F2}kg";

        private void Start()
        {
            var text = GetComponent<Text>();

            //スコアが変動したら表示を変える
            ScoreCounter.Instance
                .ScoreReactiveProperty
                .Where(_=>GameEndManager.Instance.IsGameActive.Value)
                .SubscribeToText(text, x => String.Format(format, (x/1000.0f)));

            //初期化
            GameRestartManager.Instance
                .RestartObservable
                .SubscribeToText(text, _ => String.Format(format, 0));
            
            //ゲームが終了したら表示を消す
            GameEndManager.Instance.GameEneObservable
                .SubscribeToText(text, _ => "");
        }

    }
}