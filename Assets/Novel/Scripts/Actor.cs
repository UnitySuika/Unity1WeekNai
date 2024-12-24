using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace HatenoWorks.Novel
{
    [RequireComponent(typeof(Image))]
    public class Actor : MonoBehaviour
    {
        public enum StatusEnum { Natural, Angry, Scared, Surprised, Sad, Happy, }

        public ActorData Data { get; set; }

        public bool IsSpeaking
        {
            get => _IsSpeaking;
            set
            {
                _IsSpeaking = value;
                graphic.color = _IsSpeaking ? Color.white : new Color(0.8f, 0.8f, 0.8f);
            }
        }
        public bool IsShow
        {
            get => _IsShow;
            set
            {
                if (_IsShow != value)
                {
                    gameObject.SetActive(value);
                }
                _IsShow = value;
            }
        }
        public StatusEnum Status
        {
            get => _Status;
            set
            {
                _Status = value;
                graphic.sprite = Array.Find(Data.Graphics, g => g.Status == value).Graphic;
                graphic.SetNativeSize();
            }
        }
        public Vector2 GraphicPosition 
        { 
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        private bool _IsSpeaking;

        private bool _IsShow;

        private StatusEnum _Status;

        private Image graphic => GetComponent<Image>();
    }
}
