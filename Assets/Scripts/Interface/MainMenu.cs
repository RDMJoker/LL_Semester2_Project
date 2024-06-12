using System;
using Scriptables.Holder;
using TMPro;
using UnityEngine;

namespace Interface
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject sidebar;
        [SerializeField] TextMeshProUGUI header;
        [SerializeField] TextMeshProUGUI description;

        
        
        public void SetText(TextHolder _text)
        {
            header.text = _text.header;
            description.text = _text.description;
            OpenSidebar();
        }

        void OpenSidebar()
        {
            sidebar.SetActive(true);
        }

        public void CloseSidebar()
        {
            sidebar.SetActive(false);
        }
    }
}