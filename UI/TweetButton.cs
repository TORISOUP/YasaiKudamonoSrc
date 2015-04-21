using System;
using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;

namespace Ahoge
{

    public class TweetButton : MonoBehaviour
    {
        private string urlFormat = "https://twitter.com/intent/tweet?&text={0}";
        private string format = "【YASAI KUDAMONO】{0:F2}kgの野菜と果物を配置できました！ http://torisoup.net/yasaikudamono/ #ysaikudamono #ahoge";
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();

            //TweetWindowを開く
            button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    string message = String.Format(format, GameEndManager.Instance.EndScoreReadOnlyReactiveProperty.Value/1000.0f);
                    string url = string.Format(urlFormat, WWW.EscapeURL(message));
#if UNITY_WEBPLAYER
                    Application.ExternalEval("window.open('"+url+"','','width=600,height=300');");
#else
                    Application.OpenURL(url);
#endif
                });

            //ゲーム終了直後の誤爆防止のために、ゲーム終了から１秒まって押せるようにする
            GameEndManager.Instance
                .GameEneObservable
                .Do(_ => button.interactable = false)
                .Delay(TimeSpan.FromSeconds(1))
                .Subscribe(_ => button.interactable = true);
        }
    }
}