using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;
namespace Ahoge
{
    /// <summary>
    /// クリックして野菜投げるまでを管理
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        private VegetableEmitter _vegetableEmitter;

        private void Start()
        {
            _vegetableEmitter = GetComponent<VegetableEmitter>();
            var vegetableManager = VegetableManager.Instance;

            //左クリックされたらマウスの位置から投げる方向を割り出してEmitterに伝える
            this.UpdateAsObservable()
                .Where(_=>Input.GetMouseButtonDown(0) && GameEndManager.Instance.IsGameActive.Value)
                .Select(_ => new
                {
                    Vegetable = vegetableManager.SelectedVegetable.Value,
                    Direction = GetDirection(Input.mousePosition)
                })
                .Subscribe(x =>
                {
                    _vegetableEmitter.EmitVegetable(x.Vegetable, x.Direction);
                });

            //右クリックされたら野菜変える
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(1))
                .Subscribe(_ =>
                {
                    vegetableManager.ToNext();
                });

            //ゲーム開始時にりんごを装備させる
            GameRestartManager.Instance
                .RestartObservable
                .First()
                .Subscribe(_ => vegetableManager.Reset() );
        }

        /// <summary>
        /// マウスの位置から投げる方向を決める
        /// </summary>
        /// <param name="mousePosition">マウスの位置</param>
        /// <returns>方向</returns>
        private Vector3 GetDirection(Vector2 mousePosition)
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out hit)) return Camera.main.transform.forward;
            var point = hit.point;

            return (point - transform.position).normalized;
        }

    }
}