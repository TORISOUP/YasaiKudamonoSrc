using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Ahoge
{
    /// <summary>
    /// ゲームのリセット通知管理
    /// </summary>
    public class GameRestartManager : SingletonMonoBehaviour<GameRestartManager>
    {

        private readonly Subject<Unit> _restartSubject = new Subject<Unit>();

        public IObservable<Unit> RestartObservable { get { return _restartSubject.AsObservable(); } }

        private void Start()
        {
            Application.targetFrameRate = 60;

            //Rキーでリセットする
            //Multicastを使ってSubjectを叩いている
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.R))
                .Multicast(_restartSubject)
                .Connect();

        }

        /// <summary>
        /// ゲームのリセットする
        /// </summary>
        public void GameReset()
        {
            _restartSubject.OnNext(Unit.Default);
        }

    }
}