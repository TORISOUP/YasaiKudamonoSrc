using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace Ahoge
{
    /// <summary>
    /// 棚がひっくり返った時に音を鳴らす
    /// </summary>
    public class TanaAudioDetector : MonoBehaviour
    {
        private void Start()
        {
            var audioManager = TanaAudioManager.Instance;

            //実際の発音はManagerに委譲
            this.OnTriggerEnterAsObservable()
                .Where(x => x.gameObject.layer == (int) GameLayers.Field)
                .Subscribe(_ => audioManager.SEStart());
            
        }
    }
}