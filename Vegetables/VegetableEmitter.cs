using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Ahoge
{
    /// <summary>
    /// 野菜投げるやつ
    /// </summary>
    public class VegetableEmitter : MonoBehaviour
    {
        [SerializeField]
        private float EmitRotationOffset = -10;

        /// <summary>
        /// 野菜を総括するゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject _vegitables;

        [SerializeField] private AudioClip _seHeavy;
        [SerializeField] private AudioClip _seMedium;
        [SerializeField] private AudioClip _seLight;


        private VegetableManager _vegetableManager;

        private AudioSource _audioSource;

        void Start()
        {
            _vegetableManager = VegetableManager.Instance;
            _audioSource = GetComponent<AudioSource>();
        }


        /// <summary>
        /// 野菜発射
        /// </summary>
        /// <param name="vegetable"></param>
        /// <param name="direction"></param>
        public void EmitVegetable(Vegetables vegetable, Vector3 direction)
        {
            //現在の装備野菜取得
            var vegetableObject = _vegetableManager.GetVegitableObject(vegetable);

            var instantedVegetableObject = Instantiate(vegetableObject, this.transform.position, Random.rotation) as GameObject;
            
            //野菜を管理オブジェクト配下に入れる
            instantedVegetableObject.transform.SetParent(_vegitables.transform);
            
            var vegetableRigidBody = instantedVegetableObject.GetComponent<Rigidbody>();

            //投げる方向を上向きに若干補正する
            var offsetedDirection = Quaternion.AngleAxis(EmitRotationOffset, Vector3.right) * direction;
            
            //回転方向はランダム
            var randomVector = new Vector3(Random.Range(-10.0F, 10.0F), Random.Range(-10.0F, 10.0F), Random.Range(-10.0F, 10.0F)).normalized;
            
            //投げる
            vegetableRigidBody.AddTorque(randomVector * 10000.0f,ForceMode.Impulse);
            vegetableRigidBody.AddForce(offsetedDirection * 10.0f, ForceMode.VelocityChange);

            //SE鳴らす
            SEStart(vegetable);
        }

        /// <summary>
        /// 投げるときの音
        /// </summary>
        /// <param name="vegetable"></param>
        private void SEStart(Vegetables vegetable)
        {

            AudioClip se = null;

            switch (vegetable)
            {
                case Vegetables.Apple:
                case Vegetables.Carrot:
                case Vegetables.Grapes:
                case Vegetables.Lemon:
                    //軽い野菜は軽めの音
                    se = Random.Range(0, 2) == 0 ? _seLight : _seMedium;
                    break;
                case Vegetables.Cabbage:
                case Vegetables.Pumpkin:
                    //重い野菜は重い
                    se = _seHeavy;
                    break;
            }

            _audioSource.PlayOneShot(se);
        }


    }
}