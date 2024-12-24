using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HatenoWorks.Novel.Actor;

namespace HatenoWorks.Novel
{
    [CreateAssetMenu(menuName = "Data/Actor")]
    public class ActorData : ScriptableObject
    {
        public string Name => _Name;
        public StatusToGraphic[] Graphics => _Graphics;

        [SerializeField] private string _Name;
        [SerializeField]
        private StatusToGraphic[] _Graphics = new StatusToGraphic[]
        {
            new StatusToGraphic(StatusEnum.Natural, null),
            new StatusToGraphic(StatusEnum.Angry, null),
            new StatusToGraphic (StatusEnum.Scared, null),
            new StatusToGraphic(StatusEnum.Surprised, null),
            new StatusToGraphic(StatusEnum.Sad, null),
            new StatusToGraphic(StatusEnum.Happy, null),
        };

        [System.Serializable]
        public class StatusToGraphic
        {
            public StatusEnum Status => _Status;
            public Sprite Graphic => _Graphic;

            [SerializeField] private StatusEnum _Status;
            [SerializeField] private Sprite _Graphic;

            public StatusToGraphic(StatusEnum status, Sprite graphic)
            {
                _Status = status;
                _Graphic = graphic;
            }
        }
    }
}