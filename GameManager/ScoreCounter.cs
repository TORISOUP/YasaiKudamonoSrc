using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Ahoge
{
    /// <summary>
    /// 全当たり判定の野菜を集計して点数を求める
    /// </summary>
    public class ScoreCounter : SingletonMonoBehaviour<ScoreCounter>
    {
        private IEnumerable<AtariHanteiObject> _atariHantes;

        /// <summary>
        /// 地面に触れてしまっているかの判定
        /// この数値よりY座標が低い野菜は床に付いていると判定して点数から除外
        /// </summary>
        [SerializeField]
        private float ThresholdFloorTouch = 0.8f;

        /// <summary>
        /// 現在の点数(単位はg)
        /// </summary>
        public ReactiveProperty<int> ScoreReactiveProperty = new ReactiveProperty<int>(0); 

        private void Start()
        {
            _atariHantes = GetComponentsInChildren<AtariHanteiObject>();

            //毎フレーム点数をカウント
            this.ObserveEveryValueChanged(x => x.CountScore())
                .Subscribe(x => ScoreReactiveProperty.Value = x);
        }

        /// <summary>
        /// 点数の計算処理 O(n)
        /// </summary>
        /// <returns>点数</returns>
        private int CountScore()
        {
            return (int) _atariHantes.SelectMany(x => x.RiddeOnVegitableObject)
                .Distinct() //重複除く
                .Where(x => x != null)
                .Where(x => x.transform.position.y > ThresholdFloorTouch)
                .Sum(x => x.Weight);
        }

    }
}