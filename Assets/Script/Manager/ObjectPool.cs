using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefeb;
    Queue<GameObject> objectPool = new Queue<GameObject>(); //������Ʈ�� ���� ť
    public static ObjectPool instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            for (int i = 0; i < 1000; i++)
            {
                instance.objectPool.Enqueue(CreateObject()); //�ʱ⿡ 30���� ������Ʈ�� ������
            }
        }
    }
    GameObject CreateObject() //�ʱ� OR ������Ʈ Ǯ�� ���� ������Ʈ�� ������ ��, ������Ʈ�� �����ϱ����� ȣ��Ǵ� �Լ�
    {
        GameObject newObj = Instantiate(objectPrefeb, instance.transform);
        newObj.gameObject.SetActive(false);

        return newObj;
    }
    public GameObject GetObject(Vector3 pos, Quaternion rot) //������Ʈ�� �ʿ��� �� �ٸ� ��ũ��Ʈ���� ȣ��Ǵ� �Լ�
    {
        if (objectPool.Count > 0) //���� ť�� �����ִ� ������Ʈ�� �ִٸ�,
        {
            GameObject objectInPool = objectPool.Dequeue();
            objectInPool.gameObject.SetActive(true);
            objectInPool.transform.position = pos;
            objectInPool.transform.rotation = rot;
            objectInPool.GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * 4000);
            objectInPool.transform.SetParent(transform);
            return objectInPool;
        }
        else //ť�� �����ִ� ������Ʈ�� ���� �� ���� ���� ���
        {
            GameObject objectInPool = CreateObject();

            objectInPool.gameObject.SetActive(true);
            objectInPool.transform.position = pos;
            objectInPool.transform.rotation = rot;
            objectInPool.GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * 4000);
            objectInPool.transform.SetParent(transform);
            return objectInPool;
        }
    }
    public void ReturnObjectToQueue(GameObject obj) //����� �Ϸ� �� ������Ʈ�� �ٽ� ť�� ������ ȣ�� �Ķ����->��Ȱ��ȭ �� ������Ʈ
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        instance.objectPool.Enqueue(obj); //�ٽ� ť�� ����
    }
}