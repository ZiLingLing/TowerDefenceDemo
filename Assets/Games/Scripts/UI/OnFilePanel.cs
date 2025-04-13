using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFGameFramework;

namespace TowerDefence
{
    public class OnFilePanel : Panel
    {
        [SerializeField]
        private List<Transform> archiveNodeList;

        protected override void Awake()
        {
            base.Awake();
            RegisterBtnListener();
        }

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);
            UpdateArchiveInfo();
        }

        private void RegisterBtnListener()
        {
            for(int i = 0; i<archiveNodeList.Count; i++)
            {
                int index = i;

                Transform archiveNode = archiveNodeList[i];
                if (archiveNode == null) return;
                Button btnNewGame = archiveNode.Find("Empty").GetComponent<Button>();
                Button btnPlay = archiveNode.Find("NotEmpty/Confirm").GetComponent<Button>();
                Button btnDelete = archiveNode.Find("NotEmpty/Delete").GetComponent<Button>();

                btnNewGame.onClick.RemoveAllListeners();
                btnNewGame.onClick.AddListener(() => OnBtnNewGameClick(index));
                btnPlay.onClick.RemoveAllListeners();
                btnPlay.onClick.AddListener(() => OnBtnPlayClick(index));
                btnDelete.onClick.RemoveAllListeners();
                btnDelete.onClick.AddListener(() => OnBtnDeleteClick(index));
            }
        }

        public void OnBtnNewGameClick(int index)
        {
            //����
            Module.LoadController<OnFileController>().Create(index);
            //ѡ��浵
            Module.LoadController<GameController>().SetArchiveIndex(index);
            //����״̬
            Module.LoadController<GameController>().SetGameState(EGameState.SelectLevel);
            Close();
        }
        public void OnBtnPlayClick(int index)
        {
            //ѡ��浵
            Module.LoadController<GameController>().SetArchiveIndex(index);
            //����״̬
            Module.LoadController<GameController>().SetGameState(EGameState.SelectLevel);
            Close();
        }
        public void OnBtnDeleteClick(int index)
        {
            DialogPanelParam dialogPanelParam = new DialogPanelParam();
            dialogPanelParam.message = "�Ƿ�ɾ���浵";
            dialogPanelParam.onConfirmClick = () =>
            {
                Module.LoadController<OnFileController>().Delete(index);
                UpdateArchiveInfo(index);
            };
            //���ز�����ȷ�ϵ���
            Module.LoadPanel<DialogPanel>(UIType, null, dialogPanelParam);
        }

        private void UpdateArchiveInfo()
        {
            for(int i = 0; i<archiveNodeList.Count; i++)
            {
                UpdateArchiveInfo(i);
            }
        }

        private void UpdateArchiveInfo(int index)
        {
            Transform archiveNode = archiveNodeList[index];
            if (archiveNode == null) return;

            GameObject emptyNode = archiveNode.transform.Find("Empty").gameObject;
            GameObject notEmptyNode = archiveNode.transform.Find("NotEmpty").gameObject;

            Text starNumText = notEmptyNode.transform.Find("Confirm/Progress").GetComponent<Text>();
            Text levelText = notEmptyNode.transform.Find("Confirm/Level").GetComponent<Text>();

            OnFileModel onFileModel = Module.LoadController<OnFileController>().Get(index);
            emptyNode.SetActive(onFileModel == null);
            notEmptyNode.SetActive(onFileModel != null);

            if (onFileModel != null)
            {
                int levelCount = Module.LoadController<LevelController>().GetLevelCount();//�ؿ�������������ͨ�غ�δͨ�ص�

                starNumText.text = string.Format("{0}/{1}", onFileModel.AllStarNum, levelCount * 3);
                //ǰ������ͨ���ؿ�������������ȫ���ؿ�����
                levelText.text = string.Format("�ؿ�{0}", onFileModel.levels.Count, levelCount);
            }
        }
    }
}

