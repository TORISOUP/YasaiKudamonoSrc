using UnityEngine;
using System.Collections;
using UniRx.Triggers;
using UniRx;
namespace Ahoge
{
    /// <summary>
    /// 野菜がぶつかった時に音を鳴らす
    /// </summary>
    public class VegetableHitAudioDetector : MonoBehaviour
    {
        /// <summary>
        /// どれくらいの勢いでぶつかった時に音を鳴らすか
        /// </summary>
        [SerializeField]
        private float ThresholdAudioPlay = 3.0f;

        private void Start()
        {
            var auidoManager = VegetableAudioManager.Instance;
            //野菜が勢い良くぶつかった時に音を鳴らす
            this.OnCollisionEnterAsObservable()
                .Where(x => x.gameObject.layer == (int) GameLayers.Normal)
                .Where(x => x.relativeVelocity.sqrMagnitude > ThresholdAudioPlay*ThresholdAudioPlay)
                .Subscribe(_ => auidoManager.SEStart());
        }

    }
}