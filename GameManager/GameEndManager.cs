using System;
using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Ahoge
{
    /// <summary>
    /// ゲームの終了時の管理全般
    /// </summary>
    public class GameEndManager : SingletonMonoBehaviour<GameEndManager>
    {
        private readonly ReactiveProperty<Boolean> _gameActiveReactiveProperty = new ReactiveProperty<bool>(false);

        [SerializeField] private GameObject _gameEndPanel;
        [SerializeField]
        private AudioClip _payAudioClip;

        /// <summary>
        /// ゲームが進行中であるか
        /// </summary>
        public ReadOnlyReactiveProperty<Boolean> IsGameActive
        {
            get
            {
                return _gameActiveReactiveProperty.ToReadOnlyReactiveProperty();
            }
        } 

        /// <summary>
        /// ゲームが終了したことを通知する
        /// </summary>
        public IObservable<Unit> GameEneObservable
        {
            //実体はタイマが0になったタイミングをフィルタして通知しているだけ
            get { return GameTimerManager.Instance.GameTime.Where(x => x == 0).AsUnitObservable(); }
        }

        /// <summary>
        /// ゲーム終了時の点数
        /// </summary>
        private readonly IntReactiveProperty _endScoreIntReactiveProperty= new IntReactiveProperty(0);

        /// <summary>
        /// ゲーム終了時の点数通知
        /// </summary>
        public ReadOnlyReactiveProperty<int> EndScoreReadOnlyReactiveProperty
        {
            get
            {
                return _endScoreIntReactiveProperty.ToReadOnlyReactiveProperty();
            }
        }


        private void Start()
        {
            var audioSource = GetComponent<AudioSource>();

            //ゲーム終了時の処理
            GameEneObservable
                .Subscribe(_ =>
                {
                    //ゲーム中フラグ書き換え
                    _gameActiveReactiveProperty.Value = false;
                    //最終点数保存
                    _endScoreIntReactiveProperty.Value = ScoreCounter.Instance.ScoreReactiveProperty.Value;
                    //リザルト画面出す
                    _gameEndPanel.SetActive(true);
                    //チーン
                    audioSource.PlayOneShot(_payAudioClip);

                });

            //ゲームリセット時にリザルト画面を消してゲーム中フラグを書き換える
            GameRestartManager
                .Instance.RestartObservable
                .Do(_ =>
                {
                    _gameEndPanel.SetActive(false);
                })
                .Delay(TimeSpan.FromMilliseconds(1500))
                .Subscribe(_ => _gameActiveReactiveProperty.Value = true);

#if UNITY_STANDALONE
            //PCビルド版のみQでゲーム終了
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Q))
                .First()
                .Subscribe(_ => Application.Quit());
#endif
        }

    }
}