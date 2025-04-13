using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFABManager;
using XFGameFramework;

namespace TowerDefence
{
    public class Entry : MonoBehaviour
    {
        [SerializeField]
        private StartUpPanel startUpPanel;
        [SerializeField]
        private TipPanel tipPanel;
        IEnumerator Start()
        {
            yield return StartCoroutine(ReadyMainModuleRes());

            // ����MainModule
            StartUpModuleRequest request = ModuleManager.StartUpModule<MainModule>();
            yield return request;
            if (!string.IsNullOrEmpty(request.error))
                Debug.LogErrorFormat("ģ������ʧ��:{0}", request.error);
        }
        /// <summary>
        ///  ׼��MainModule��Դ
        /// </summary>
        /// <returns></returns>
        IEnumerator ReadyMainModuleRes()
        {
            //��ѭ������:�����Դ׼��ʧ��������׼��,����ɹ�������ѭ��
            while (true)
            {
                // �����Դ
                CheckResUpdateRequest request_check = AssetBundleManager.CheckResUpdate("MainModule");

                yield return request_check;

                if (string.IsNullOrEmpty(request_check.error))
                {
                    // ��Դ���ɹ� 
                    if (request_check.result.updateType == UpdateType.DontNeedUpdate) break;
                    // �����Ҫ����,������Ը��ݸ�����������һЩ��ʾ,
                    // ����:һ������»��ڸ���֮ǰ��ʾ�û�,��Ҫ���ض�����Դ,��������,����Ϣ TODO 
                    // �������Ҫ��ʾ�Ļ����������κδ���
                    if(request_check.result.updateSize > 0)//��⵽�и�����Դ
                    {
                        string updateSize = StringTools.FormatByte(request_check.result.updateSize);
                        string version = request_check.result.version;
                        string content = string.Format("��⵽��Դ����\n��С��{0}  �汾��{1}",updateSize,version);

                        tipPanel.ShowTip("������ʾ", content, () => tipPanel.Hide(), () => Application.Quit());
                        while(tipPanel.gameObject.activeSelf == true)
                        {
                            yield return null;
                        }
                    }
                }
                else
                {
                    // ��Դ���ʧ��,������Ӧ��ʾ��,�ٴμ�� TODO
                    string content = string.Format("��Դ���ʧ�ܣ��������硣");

                    tipPanel.ShowTip("������ʾ", content, () => tipPanel.Hide(), () => Application.Quit());
                    while (tipPanel.gameObject.activeSelf == true)
                    {
                        yield return null;
                    }
                }

                // ׼����Դ
                ReadyResRequest request = AssetBundleManager.ReadyRes(request_check.result);

                while (!request.isDone)
                {
                    yield return null;
                    // �������������UI���� TODO
                    switch (request.ExecutionType)
                    {
                        case ExecutionType.Download:
                            // ����������Դ
                            startUpPanel.ShowTip("����������Դ......");
                            startUpPanel.RefreshProgress(request.DownloadedSize / request.NeedDownloadedSize);
                            break;
                        case ExecutionType.Decompression:
                            // ��ѹ��Դ
                            startUpPanel.ShowTip("���ڽ�ѹ��Դ......");
                            break;
                        case ExecutionType.Verify:
                            // У����Դ(��Դ������ɺ�,��ҪУ���ļ��Ƿ���)
                            startUpPanel.ShowTip("����У����Դ......");
                            break;
                        case ExecutionType.ExtractLocal:
                            // �ͷ���Դ(����Դ������Ŀ¼���Ƶ�����Ŀ¼)
                            startUpPanel.ShowTip("�����ͷ���Դ......");
                            break;
                    }
                    startUpPanel.RefreshProgress(request.progress);
                }

                if (string.IsNullOrEmpty(request.error))
                {
                    // ��Դ׼���ɹ�����ѭ��
                    break;
                }
                else
                {
                    // ��Դ׼��ʧ��,������Ӧ��ʾ,Ȼ���ٴ�׼�� TODO
                    string content = string.Format("��Դ׼��ʧ�ܣ��������硣");

                    tipPanel.ShowTip("������ʾ", content, () => tipPanel.Hide(), () => Application.Quit());
                    while (tipPanel.gameObject.activeSelf == true)
                    {
                        yield return null;
                    }
                    yield return new WaitForSeconds(2);
                }
            }

        }
    }

}
