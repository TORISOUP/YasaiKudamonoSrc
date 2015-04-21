using System;
using UnityEngine;
using System.Collections;
using UniRx;
namespace Ahoge
{
    /// <summary>
    /// ゲーム中における残り時間を管理する
    /// </summary>
    public class GameTimerManager : SingletonMonoBehaviour<GameTimerManager>
    {
        [SerializeField] private AudioClip _beepAudioClip;

        [SerializeField] private int LimitTime = 30;

        private IDisposable _timerDisposable;

        readonly ReactiveProperty<int> _timerReactiveProperty = new IntReactiveProperty(0);

        /// <summary>
        /// 残り時間
        /// </summary>
        public ReadOnlyReactiveProperty<int> GameTime
        {
            get { return _timerReactiveProperty.ToReadOnlyReactiveProperty(); } 
        }

        private void Start()
        {
            var audioSource = GetComponent<AudioSource>();

            //ゲームリスタート時にタイマを止めて1.5秒後に再起動する
            GameRestartManager.Instance
                .RestartObservable
                .Do(_ => TimerStop())
                .Delay(TimeSpan.FromMilliseconds(1500))
                .Subscribe(_ =>
                {
                    _timerReactiveProperty.Value = LimitTime;
                    TimerRestart();
                });

            //初期設定
            _timerReactiveProperty.Value = LimitTime;

            //10秒切ったら1秒毎に音を鳴らす
            GameTime
                .Skip(1) //最初に何故か鳴ってしまうのでSkipで無視
                .Where(x => x <= 10 && x > 0)
                .Subscribe(_ => audioSource.PlayOneShot(_beepAudioClip));

        }

        /// <summary>
        /// タイマを初期化して再スタートする
        /// </summary>
        public void TimerRestart()
        {
            TimerStop();
            //1秒おきに_timerReactivePropertyの値をマイナスする
            _timerDisposable = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
                .Subscribe(_ => _timerReactiveProperty.Value--)
                .AddTo(gameObject); //ゲームオブジェクト破棄時に自動停止させる
        }

        /// <summary>
        /// タイマを止める
        /// </summary>
        public void TimerStop()
        {
            if (_timerDisposable != null)
            {
                _timerDisposable.Dispose();
            }
        }
    }
}