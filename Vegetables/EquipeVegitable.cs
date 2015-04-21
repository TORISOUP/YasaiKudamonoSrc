using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using UniRx;
namespace Ahoge
{
    /// <summary>
    /// 投げる野菜を画面右下に出す
    /// </summary>
    public class EquipeVegitable : MonoBehaviour
    {

        private GameObject _equipedVegetable;
        private VegetableManager _vegetableManager;

        private void Start()
        {
            _vegetableManager = VegetableManager.Instance;

            //ゲーム開始のタイミングから表示を始める
            _vegetableManager
                .SelectedVegetable
                .SkipUntil(GameRestartManager.Instance.RestartObservable)
                .Subscribe(SetVegetable);

            //初回起動時のみ自分から野菜を取りに行く
            GameRestartManager.Instance.RestartObservable
                .First()
                .Subscribe(_ => SetVegetable(_vegetableManager.SelectedVegetable.Value));
        }

        /// <summary>
        /// 野菜を画面右下に出す
        /// </summary>
        /// <param name="vegetable">野菜</param>
        private void SetVegetable(Vegetables vegetable)
        {
            if (_equipedVegetable != null) { Destroy(_equipedVegetable);}
            var vo = _vegetableManager.GetVegitableObject(vegetable);
            if (vo == null) { return;}
            var o = Instantiate(vo, this.transform.position, Quaternion.LookRotation(Vector3.up)) as GameObject;
            o.transform.SetParent(transform);

            //表示する野菜から当たり判定とかを消す
            Destroy(o.GetComponent<Rigidbody>());
            Destroy(o.GetComponent<Collider>());

            _equipedVegetable = o;
        }
    }
}