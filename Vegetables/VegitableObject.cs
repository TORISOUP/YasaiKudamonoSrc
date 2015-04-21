using UnityEngine;
using System.Collections;

namespace Ahoge
{
    /// <summary>
    /// 野菜そのものの管理
    /// </summary>
    public class VegitableObject : MonoBehaviour
    {
        /// <summary>
        /// 野菜の種類
        /// </summary>
        [SerializeField] private Vegetables _vegitableType;

        public Vegetables VegetableType
        {
            get { return _vegitableType; }
        }

        /// <summary>
        /// 重さ（グラム）
        /// </summary>
        public float Weight
        {
            get { return GetComponent<Rigidbody>().mass*1000.0f; }
        }

        //画面外に消えたら消す
        void Update()
        {
            if (transform.position.y < -3)
            {
                Destroy(this.gameObject);
            }
        }
    }
}