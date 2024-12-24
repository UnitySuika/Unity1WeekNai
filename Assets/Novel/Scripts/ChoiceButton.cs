using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HatenoWorks.Novel
{

    public class ChoiceButton : MonoBehaviour
    {
        public int Index
        {
            get => _Index;
            set
            {
                _Index = value;
                SelectButton.onClick.RemoveAllListeners();
                SelectButton.onClick.AddListener(() => OnClicked(Index));
            }
        }

        public Button SelectButton => GetComponent<Button>();

        public TextWindow ChoiceWindow => GetComponent<TextWindow>();

        public Action<int> OnClicked;

        private int _Index;
    }
}
