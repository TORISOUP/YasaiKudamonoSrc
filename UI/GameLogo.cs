using UnityEngine;
using System.Collections;

namespace Ahoge
{

    public class GameLogo : MonoBehaviour
    {

        private void Start()
        {
            iTween.ShakeScale(gameObject, iTween.Hash("z", 10));
        }
    }
}