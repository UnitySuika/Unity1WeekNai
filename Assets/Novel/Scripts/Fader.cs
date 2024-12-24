using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace HatenoWorks.Novel
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private Image curtain;

        public void SetColor(Color color)
        {
            curtain.color = color;
        }

        public async UniTask FadeOut(float fadeOutTime, CancellationToken token)
        {
            token = CancellationTokenSource.CreateLinkedTokenSource(
                token, this.GetCancellationTokenOnDestroy()).Token;

            curtain.gameObject.SetActive(true);

            Color c = curtain.color;
            c.a = 0f;
            curtain.color = c;

            await curtain.DOFade(1f, fadeOutTime)
                .ToUniTask(cancellationToken: token);
        }

        public async UniTask FadeIn(float fadeInTime, CancellationToken token)
        {
            token = CancellationTokenSource.CreateLinkedTokenSource(
                token, this.GetCancellationTokenOnDestroy()).Token;

            curtain.gameObject.SetActive(true);

            Color c = curtain.color;
            c.a = 1f;
            curtain.color = c;

            await curtain.DOFade(0f, fadeInTime)
                .ToUniTask(cancellationToken: token);

            token.ThrowIfCancellationRequested();

            curtain.gameObject.SetActive(false);
        }
    }
}