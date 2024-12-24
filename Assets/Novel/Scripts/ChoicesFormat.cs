using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatenoWorks.Novel
{
    [CreateAssetMenu(menuName = "Data/ChoicesFormat")]
    public class ChoicesFormat : ScriptableObject
    {
        public TextWindow DescriptionWindowPrefab => _DescriptionWindowPrefab;
        public TextWindow ChoiceWindowPrefab => _ChoiceWindowPrefab;
        public Vector2 ChoicesOffset => _ChoicesOffset;
        public Vector2 ChoiceDisplacemnet => _ChoicesDisplacement;

        [SerializeField] private TextWindow _DescriptionWindowPrefab;
        [SerializeField] private TextWindow _ChoiceWindowPrefab;
        [SerializeField] private Vector2 _ChoicesOffset;
        [SerializeField] private Vector2 _ChoicesDisplacement;
    }
}
