using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Random = UnityEngine.Random;

namespace Ahoge
{
    /// <summary>
    /// 床回すやつ
    /// </summary>
    public class FloorReseter : MonoBehaviour
    {
        private void Start()
        {
            var audioSource = GetComponent<AudioSource>();

            //回転方向リスト
            var tweenHashes = new List<Hashtable>
            {
                iTween.Hash("z", -180, "time", 1.5f),
                iTween.Hash("z", 180, "time", 1.5f),
                iTween.Hash("x", 180, "time", 1.5f)
            };


            //リセットのタイミングでSEを鳴らして300ミリ秒後に床を回す
            //床回し初めてから1.5秒後に床を完全に初期位置に戻す
            GameRestartManager.Instance
                .RestartObservable
                .Do(_ => audioSource.Play())
                .Delay(TimeSpan.FromMilliseconds(300))
                .Do(_ =>
                {
                    //床回す
                    iTween.RotateAdd(gameObject, tweenHashes[Random.Range(0,tweenHashes.Count)]);
                })
                .Delay(TimeSpan.FromMilliseconds(1500))
                .Subscribe(_ =>
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);

                });
        }

    }
}