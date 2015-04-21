using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngineInternal;

namespace Ahoge
{
    public class AtariHanteiObject : MonoBehaviour
    {

        private readonly List<VegitableObject> _putOnVegitables = new List<VegitableObject>();

        /// <summary>
        /// 当たり判定に含まれている野菜群
        /// </summary>
        public IEnumerable<VegitableObject> RiddeOnVegitableObject
        {
            get { return _putOnVegitables.AsReadOnly(); }
        }

        private void Start()
        {
            //野菜が入ってきたらリストに追加
            this.OnTriggerEnterAsObservable()
                .Select(x => x.gameObject.GetComponent<VegitableObject>())
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    _putOnVegitables.Add(x);
                });

            //野菜が出て行ったらリストから除外（あんまり信用ならない）
            this.OnTriggerExitAsObservable()
                .Select(x => x.gameObject.GetComponent<VegitableObject>())
                .Where(x => x != null)
                .Subscribe(x => _putOnVegitables.Remove(x));

            //ゲームリセット時にリストクリア
            GameRestartManager
                .Instance
                .RestartObservable
                .Subscribe(_ => _putOnVegitables.Clear());
        }

    }
}