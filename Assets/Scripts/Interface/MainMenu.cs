using System;
using System.Collections;
using DG.Tweening;
using Scriptables.Holder;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject sidebar;
        [SerializeField] TextMeshProUGUI header;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] VerticalLayoutGroup buttonGroup;
        [SerializeField] GameObject sidebarBackdropBanner;
        [SerializeField] float sidebarStartY;
        [SerializeField] float sidebarEndY;
        [SerializeField] float animationDuration;

        
        
        public void SetText(TextHolder _text)
        {
            header.text = _text.header;
            description.text = _text.description;
            OpenSidebar();
        }

        void OpenSidebar()
        {
            StartCoroutine(OpenSidebarWithAnimation());
        }

        IEnumerator OpenSidebarWithAnimation()
        {
            buttonGroup.gameObject.SetActive(false);
            sidebarBackdropBanner.transform.DOMoveY(sidebarEndY, animationDuration);
            yield return new WaitForSeconds(animationDuration);
            sidebar.SetActive(true);
        }

        IEnumerator CloseSidebarWithAnimation()
        {
            sidebar.SetActive(false);
            sidebarBackdropBanner.transform.DOMoveY(sidebarStartY, animationDuration);
            yield return new WaitForSeconds(animationDuration);
            buttonGroup.gameObject.SetActive(true);
        }

        public void CloseSidebar()
        {
            StartCoroutine(CloseSidebarWithAnimation());
        }
    }
}