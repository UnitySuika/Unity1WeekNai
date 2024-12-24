using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

namespace HatenoWorks.Novel
{
    public class TextWindow : MonoBehaviour
    {
        public enum TextWindowStates { Send, Completed, }
        public TextWindowStates CurrentState { get; private set; }

        [SerializeField] private TextMeshProUGUI textArea;
        [SerializeField] private GameObject nextSign;

        private bool isMoveToLast;

        private float sendSpeed;

        // TODO: SetUpì‡Ç≈ë¶ç¿Ç…ï\é¶Ç∑ÇÈÇ©åàÇﬂÇÍÇÈÇÊÇ§Ç…Ç∑ÇÈ
        public void SetUp(float sendSpeed = 15f)
        {
            this.sendSpeed = sendSpeed;
            CurrentState = TextWindowStates.Completed;
        }

        public void StartSending(string displayedText, bool immediatelyShow)
        {
            if (immediatelyShow)
            {
                textArea.text = displayedText;
                return;
            }
            CurrentState = TextWindowStates.Send;
            SendCharacters(displayedText, this.GetCancellationTokenOnDestroy()).Forget();
        }

        public void SendToLast()
        {
            if (CurrentState == TextWindowStates.Send)
            {
                isMoveToLast = true;
            }
        }

        private async UniTask SendCharacters(string displayedText, CancellationToken token)
        {
            if (nextSign != null)
            {
                nextSign.SetActive(false);
            }
            isMoveToLast = false;
            textArea.text = "";
            for (int i = 0; i < displayedText.Length; ++i)
            {
                textArea.text += displayedText[i];
                if (i == displayedText.Length - 1) break;

                await UniTask.Delay(Mathf.RoundToInt(1000f / sendSpeed), cancellationToken: token);
                token.ThrowIfCancellationRequested();

                if (isMoveToLast)
                {
                    textArea.text = displayedText;
                    break;
                }
            }
            CurrentState = TextWindowStates.Completed;
            if (nextSign != null)
            {
                nextSign.SetActive(true);
            }
        }
    }
}
