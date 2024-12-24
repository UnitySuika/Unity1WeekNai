using HatenoWorks.Novel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatenoWorks.Novel
{
    [System.Serializable]
    public class DisplayedActorInfo
    {
        public ActorData DisplayedActorData => _DisplayedActorData;
        public bool IsSpeaking => _IsSpeaking;
        public bool IsShow => _IsShow;
        public Actor.StatusEnum Status => _Status;
        public Vector2 GraphicPosition => _GraphicPosition;

        [SerializeField] private ActorData _DisplayedActorData;
        [SerializeField] private bool _IsSpeaking;
        [SerializeField] private bool _IsShow;
        [SerializeField] private Actor.StatusEnum _Status;
        [SerializeField] private Vector2 _GraphicPosition;
    }
}
