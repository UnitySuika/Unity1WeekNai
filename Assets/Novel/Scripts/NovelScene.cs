using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatenoWorks.Novel
{
    [CreateAssetMenu(menuName = "Data/NovelScene")]
    public class NovelScene : ScriptableObject
    {
        public Moment[] Moments => _Moments;
        public TextWindow DefaultMessageWindowPrefab => _DefaultMessageWindowPrefab;
        public TextWindow DefaultNameWindowPrefab => _DefaultNameWindowPrefab;
        public NovelScene NextSceneIfNotChoices => _NextSceneIfNotChoices;
        public bool IsEnableChoices => _IsEnableChoices;
        public ChoicesFormat SceneChoicesFormat => _SceneChoicesFormat;
        public string ChoicesDescription => _ChoicesDescription;
        public Choice[] Choices => _Choices;

        [SerializeField] private Moment[] _Moments;
        [SerializeField] private TextWindow _DefaultMessageWindowPrefab;
        [SerializeField] private TextWindow _DefaultNameWindowPrefab;
        [SerializeField] private NovelScene _NextSceneIfNotChoices;
        [Header("----ˆÈ‰º‚Í‘I‘ðŽˆ‚É‚Â‚¢‚Ä----")]
        [SerializeField] private bool _IsEnableChoices;
        [SerializeField] private ChoicesFormat _SceneChoicesFormat;
        [Multiline(3)]
        [SerializeField] private string _ChoicesDescription;
        [SerializeField] private Choice[] _Choices;

        [System.Serializable]
        public class Choice
        {
            public string Message => _Message;
            public NovelScene NextScene => _NextScene;

            [SerializeField] string _Message;
            [SerializeField] NovelScene _NextScene;
        }
        
    }
}
