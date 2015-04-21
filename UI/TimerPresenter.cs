using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;

namespace Ahoge
{
    public class TimerPresenter : MonoBehaviour
    {
        private Text text;

        private void Start()
        {
            text = GetComponent<Text>();
            text.text = "";
            GameTimerManager.Instance
                .GameTime
                .SkipUntil(GameRestartManager.Instance.RestartObservable)
                .Where(x => x >= 0)
                .SubscribeToText(text);
            
            GameEndManager.Instance.GameEneObservable
                .SubscribeToText(text, _ => "");

            GameRestartManager.Instance.RestartObservable
                .First()
                .SubscribeToText(text, _=> GameTimerManager.Instance.GameTime.Value.ToString());

        }

    }
}