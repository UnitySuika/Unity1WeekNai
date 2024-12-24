using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatenoWorks.Novel
{
    [CreateAssetMenu(menuName = "Data/Place")]
    public class Place : ScriptableObject
    {
        public string Name => _Name;
        public Sprite BgSprite => _BgSprite;

        [SerializeField] private string _Name;
        [SerializeField] private Sprite _BgSprite;
    }
}
