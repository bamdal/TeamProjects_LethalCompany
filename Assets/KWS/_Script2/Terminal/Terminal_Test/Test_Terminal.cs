using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Test_Terminal : MonoBehaviour
{
    /// <summary>
    /// SphereCollider�� ã�� ���� ���� sphere
    /// </summary>
    SphereCollider sphere;

    /// <summary>
    /// TextMeshProUGUI�� ����ϱ� ���� ���� PressE_text
    /// </summary>
    TextMeshProUGUI PressE_text;

    private void Awake()
    {
        // SphereCollider�� ã�Ƽ� ������ �Ҵ��մϴ�.
        sphere = GetComponent<SphereCollider>();

        // Canvas�� ù ��° �ڽ��� �����ɴϴ�.
        Transform canvas = transform.GetChild(0);

        // "Press_E"�� �̸����� ���� �ڽ��� ã���ϴ�.
        PressE_text = canvas.Find("Press_E")?.GetComponent<TextMeshProUGUI>();

        // ���� "Press_E"�� ã�� ���ߴٸ�, ��� ����մϴ�.
        if (PressE_text == null)
        {
            Debug.LogWarning("TextMeshProUGUI�� ã�� �� �����ϴ�.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")           // �浹�� ��� ������Ʈ�� �±װ� Player�̸�
        {
            Debug.Log($"[Player] �� ���� �ȿ� ���Դ�.");
            PressE_text.gameObject.SetActive(true);            // TextMeshProUGUI�� Ȱ��ȭ
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")           // �浹�� ��� ������Ʈ�� �±װ� Player�̸�
        {
            Debug.Log($"[Player] �� ���� ������ ������.");
            PressE_text.gameObject.SetActive(false);            // TextMeshProUGUI�� ��Ȱ��ȭ
        }
    }
}

/// 0. �÷��̾ �͹̳��� ���� ������ ������ "Access terminal : [E]" �� Ȱ��ȭ     // 1
/// 1. �÷��̾� ��ǲ�׼����� EŰ�� �޾Ƽ� �͹̳� Ȱ��ȭ                               // 2
/// 1-1. �͹̳��� Ȱ��ȭ�Ǹ� �͹̳��� ����ͷ� VCam ��ġ ����                         // 3
/// 2. �͹̳��� ó�� ȭ�� �ؽ�Ʈ ���                                                 // 1��-UI

/*
���� īŻ�α׿� ���� ���� ȯ���մϴ�.
�ڵ� ���� ��ġ�� ��θ� �����Ϸ��� ROUTE�� �Է��ϼ���.
�޿� ���� �˾ƺ����� INFO�� �Է��ϼ���.
----------------------------		// (28��)

* ȸ�� �ǹ�	//	30%�� �Ÿ� ��.

* �ͽ��䷯�����̼� (����)	Experimentation
* ���� (����)		    Assurance
* ���� (����)		        Vow

* ���潺 (����)		        Offense
* ��ġ (����)		        March

* ���� (����)		        Rend
* ���� (����)		        Dine
* Ÿ��ź (����)		        Titan

(�Է��ϴ� ��)    ���⿡ Ŀ���� �������� ��?
*/

/// 3. �͹̳ο��� Help(��ҹ���X) �� �Է��ϰ� ���͸� ������ ��밡���� ��� ��ɾ �͹̳ο� ���   // 2��-UI
/// 3-1 Ȯ��(Confirm), ���(Deny), ����(Store)...                                                    // 2��-UI
/// 4. Store ���
/*
Store�� �Է��ϰ� ���͸� ������ ������ ����
�ƾ��� ���ŵ� �͹̳ο��� �� / ���� ������ ������ ���(ġ�� ���� ������ ��)
/ ������				        12��
/ ������				        15��
/ ��				            30��
/ ������ Ű			            20��
/ ���� ������(�������� ������)	25��
/ ���� ����ź			        36��
/ �չڽ�				        60��
/ Tzp ���Ա�			        72��
/ ���� ����(Zap Gun)		    400��
/ ��Ʈ��				        700��
/ Ȯ�� ��ٸ�(����� ��ٸ�)	60��
*/
