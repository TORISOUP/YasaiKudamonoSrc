using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace Ahoge
{
    /// <summary>
    /// 野菜の総合管理する
    /// </summary>
    public class VegetableManager : SingletonMonoBehaviour<VegetableManager>
    {
        /// <summary>
        /// 野菜のリソースを管理
        /// </summary>
        private Dictionary<Vegetables, GameObject> _vegetableResouces = new Dictionary<Vegetables, GameObject>();

        /// <summary>
        /// 装備野菜
        /// </summary>
        public ReactiveProperty<Vegetables> SelectedVegetable 
            = new ReactiveProperty<Vegetables>(Vegetables.Apple);

        private void Start()
        {
            //リソースロード
            foreach (Vegetables vegetable in Enum.GetValues(typeof (Vegetables)))
            {
                _vegetableResouces[vegetable] = VegetableLoader(vegetable);
            }

            //リスタート時の処理
            GameRestartManager.Instance
                .RestartObservable
                .Do(_ =>
                {
                    foreach (var v in GetComponentsInChildren<Rigidbody>())
                    {
                        //床が回転した時に良く吹っ飛ぶように
                        v.WakeUp();
                    }
                })
                .Delay(TimeSpan.FromMilliseconds(1500))
                .Subscribe(_ =>
                {
                    //床が回転し終わったら野菜全部消す
                    foreach (var v in GetComponentsInChildren<VegitableObject>())
                    {
                        Destroy(v.gameObject);
                    }

                });
        }

        /// <summary>
        /// 野菜切り替え
        /// </summary>
        public void ToNext()
        {
            var currentIndex = (int) SelectedVegetable.Value;
            var max = Enum.GetValues(typeof (Vegetables)).Length;
            var next = (currentIndex + 1)%max;
            SelectedVegetable.Value = (Vegetables) next;
        }

        /// <summary>
        /// 野菜をりんごにする #とは
        /// </summary>
        public void Reset()
        {
            SelectedVegetable.Value = 0;
        }

        /// <summary>
        /// やさいくれる
        /// </summary>
        /// <param name="vegetable"></param>
        /// <returns></returns>
        public GameObject GetVegitableObject(Vegetables vegetable)
        {
            return _vegetableResouces.ContainsKey(vegetable) ? _vegetableResouces[vegetable] : null;
        }

        /// <summary>
        /// 野菜をロードする
        /// </summary>
        /// <param name="vegetable"></param>
        /// <returns></returns>
        private GameObject VegetableLoader(Vegetables vegetable)
        {
            return Resources.Load<GameObject>("Vegetables/" + vegetable);
        }
    }
}
