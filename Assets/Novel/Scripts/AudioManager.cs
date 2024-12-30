using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatenoWorks.Novel
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] AudioSource audioSourceBgm1;
        [SerializeField] AudioSource audioSourceBgm2;
        [SerializeField] AudioSource audioSourceSe;

        [Serializable]
        public class IDAndClip
        {
            public string Id;
            public AudioClip Clip;
        }

        [SerializeField] IDAndClip[] bgmClips;
        [SerializeField] IDAndClip[] seClips;

        private AudioSource currentAudioSourceBgm;

        private bool isAudioFading = false;

        private Tween fadeTween1;
        private Tween fadeTween2;

        /*
        protected override void OverrideAwake()
        {
            DontDestroyOnLoad(gameObject);
        }
        */

        private void Start()
        {
            currentAudioSourceBgm = audioSourceBgm1;
        }

        public void PlayBgm(AudioClip clip, float volume = 0.5f, bool isEnableCrossFade = true, float crossFadeTime = 0.5f)
        {
            if (isAudioFading)
            {
                fadeTween1?.Kill();
                fadeTween2?.Kill();
                audioSourceBgm1.Stop();
                audioSourceBgm2.Stop();
            }
            AudioSource nextAudioSource = currentAudioSourceBgm == audioSourceBgm1 ? audioSourceBgm2 : audioSourceBgm1;
            nextAudioSource.clip = clip;
            nextAudioSource.Play();
            if (isEnableCrossFade)
            {
                nextAudioSource.volume = 0f;
                fadeTween1 = nextAudioSource.DOFade(volume, crossFadeTime).SetEase(Ease.Linear);
            }
            else
            {
                nextAudioSource.volume = volume;
            }

            if (isEnableCrossFade)
            {
                isAudioFading = true;
                fadeTween2 = currentAudioSourceBgm.DOFade(0f, crossFadeTime).SetEase(Ease.Linear).OnComplete(() =>
                {
                    currentAudioSourceBgm.clip = null;
                    currentAudioSourceBgm = currentAudioSourceBgm == audioSourceBgm1 ? audioSourceBgm2 : audioSourceBgm1;
                    isAudioFading = false;
                });
            }
            else
            {
                currentAudioSourceBgm.Stop();
                currentAudioSourceBgm = currentAudioSourceBgm == audioSourceBgm1 ? audioSourceBgm2 : audioSourceBgm1;
            }
        }

        public void PlayBgm(string clipID, float volume = 0.5f, bool isEnableCrossFade = true)
        {
            AudioClip clip = Array.Find(bgmClips, x => x.Id == clipID).Clip;
            PlayBgm(clip, volume, isEnableCrossFade);
        }

        public void StopBgm(float fadeTime = 0.5f)
        {
            if (currentAudioSourceBgm.clip != null)
            {
                currentAudioSourceBgm.DOFade(0f, fadeTime).SetEase(Ease.Linear);
            }
        }

        public void PlaySE(AudioClip clip, float volume = 0.5f, bool isOverride = false)
        {
            audioSourceSe.volume = volume;
            if (isOverride)
            {
                audioSourceSe.Stop();
            }
            audioSourceSe.PlayOneShot(clip);
        }

        public void PlaySE(string clipID, float volume = 0.5f, bool isOverride = false)
        {
            AudioClip clip = Array.Find(seClips, x => x.Id == clipID).Clip;
            PlaySE(clip, volume, isOverride);
        }
    }
}