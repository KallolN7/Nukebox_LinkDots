using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace nukebox
{
    public class VictoryScreenController : MonoBehaviour
    {
        [SerializeField]
        private Button backButton;
        [SerializeField]
        private Button continueButton;
        [SerializeField]
        private Button RetryButton;
        [SerializeField]
        private Transform bg;

        bool isShowed;
        bool canShow = true;
        private int level;

        public  void PrepareVictoryScreen()
        {
            bool isLastLevel = Config.currentLevel >= Config.totalLevel[Config.difficulty] - 1;

            backButton.gameObject.SetActive(isLastLevel);
            continueButton.gameObject.SetActive(!isLastLevel);

        }


        //continue
        public void OnContinue()
        {
            if (!isShowed)
                return;
            //GameManager.getInstance().playSfx("click");
            ShowHidePanel();

            if (Config.currentLevel < Config.totalLevel[Config.difficulty] - 1)
            {
                level = Config.currentLevel;
                level++;
                GameManager.instance.SetLevel(level);
            }
            else
            {
                GameManager.instance.SetLevel(0);
            }
        }

        public void OnRetry()
        {
            //GameManager.getInstance().playSfx("click");
            ShowHidePanel();
        }

        public void OnShowCompleted()
        {
            isShowed = true;
            canShow = true;
        }




        public void ShowHidePanel()
        {
            if (!canShow)
                return;

            if (!isShowed)
            {
                bg.localScale = Vector2.zero;
                bg.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic).OnComplete(OnShowCompleted);
                canShow = false;
            }
            else
            {
                bg.DOScale(Vector3.zero, .4f).OnComplete(OnHideCompleted);
                canShow = false;
            }

        }

        private void OnHideCompleted()
        {
            isShowed = false;
            gameObject.SetActive(false);
            canShow = true;
            if (Config.isWin)
            {
                GameManager.instance.GetDotLinkController().PrePareDots();
            }

        }

    }
}

