using UnityEngine;
using System.Collections;

namespace Ahoge
{
    /// <summary>
    /// 棚がひっくり返った時に音を鳴らす処理の実体
    /// </summary>
    public class TanaAudioManager : SingletonMonoBehaviour<TanaAudioManager>
    {
        [SerializeField]
        private AudioClip[] DestructionAudioClips;

        private AudioSource audioSource;

        // Use this for initialization
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

        }

        public void SEStart()
        {
            audioSource.PlayOneShot(DestructionAudioClips[Random.Range(0,DestructionAudioClips.Length)]);
        }

    }
}