using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatenoWorks.Novel
{
    [System.Serializable]
    public class Moment
    {
        public enum MessageTypes { Say, Think, }

        public string Message => _Message;
        public DisplayedActorInfo[] DisplayedActorInfoArray => _DisplayedActorInfoArray;
        public MessageTypes MessageType => _MessageType;
        public bool IsUseDefaultTextWindows => _IsUseDefaultTextWindows;
        public TextWindow MessageWindowPrefab => _MessageWindowPrefab;
        public TextWindow NameWindowPrefab => _NameWindowPrefab;
        public Place CurrentPlace => _CurrentPlace;
        public bool IsShowPlaceName => _IsShowPlaceName;
        public bool IsStopBgm => _IsStopBgm;
        public string Bgm => _Bgm;
        public string Se => _Se;

        [Header("----メッセージ関連----")]
        [Multiline(3)]
        [SerializeField] private string _Message; 
        [SerializeField] private DisplayedActorInfo[] _DisplayedActorInfoArray;
        [SerializeField] MessageTypes _MessageType = MessageTypes.Say;
        [Header("----テキストウィンドウ設定----")]
        [SerializeField] private bool _IsUseDefaultTextWindows;
        [SerializeField] private TextWindow _MessageWindowPrefab;
        [SerializeField] private TextWindow _NameWindowPrefab;
        [Header("----状況・演出----")]
        [SerializeField] private Place _CurrentPlace;
        [SerializeField] private bool _IsShowPlaceName;
        [SerializeField] private bool _IsStopBgm;
        [SerializeField] private string _Bgm;
        [SerializeField] private string _Se;
    }
}