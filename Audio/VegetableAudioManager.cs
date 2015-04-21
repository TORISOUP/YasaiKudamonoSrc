using UnityEngine;
using System.Collections;

namespace Ahoge
{

    /// <summary>
    /// 野菜がぶつかった時に音を鳴らす処理の実体
    /// </summary>
    public class VegetableAudioManager : SingletonMonoBehaviour<VegetableAudioManager>
    {

        [SerializeField] private AudioClip[] hitAudioClip;

        private AudioSource audioSource;

        // Use this for initialization
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

        }


        public void SEStart()
        {
            //ランダムに鳴らす
            audioSource.PlayOneShot(hitAudioClip[Random.Range(0,hitAudioClip.Length)]);
        }

    }
}