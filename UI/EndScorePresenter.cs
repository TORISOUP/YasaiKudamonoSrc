using System;
using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;

namespace Ahoge
{
    /// <summary>
    /// ゲーム終了時のスコアを表示する
    /// </summary>
    public class EndScorePresenter : MonoBehaviour
    {
        private Text text;

        private void Start()
        {
            text = GetComponent<Text>();

            GameEndManager.Instance
                .EndScoreReadOnlyReactiveProperty
                .SubscribeToText(text,x=>String.Format("{0:F2}kg",x/1000.0f));
        }
    }
}