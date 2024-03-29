using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefeb;
    Queue<GameObject> objectPool = new Queue<GameObject>(); //오브젝트를 담을 큐
    public static ObjectPool instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            for (int i = 0; i < 1000; i++)
            {
                instance.objectPool.Enqueue(CreateObject()); //초기에 30개의 오브젝트를 생성함
            }
        }
    }
    GameObject CreateObject() //초기 OR 오브젝트 풀에 남은 오브젝트가 부족할 때, 오브젝트를 생성하기위해 호출되는 함수
    {
        GameObject newObj = Instantiate(objectPrefeb, instance.transform);
        newObj.gameObject.SetActive(false);

        return newObj;
    }
    public GameObject GetObject(Vector3 pos, Quaternion rot) //오프젝트가 필요할 때 다른 스크립트에서 호출되는 함수
    {
        if (objectPool.Count > 0) //현재 큐에 남아있는 오브젝트가 있다면,
        {
            GameObject objectInPool = objectPool.Dequeue();
            objectInPool.gameObject.SetActive(true);
            objectInPool.transform.position = pos;
            objectInPool.transform.rotation = rot;
            objectInPool.GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * 4000);
            objectInPool.transform.SetParent(transform);
            return objectInPool;
        }
        else //큐에 남아있는 오브젝트가 없을 때 새로 만들어서 사용
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
    public void ReturnObjectToQueue(GameObject obj) //사용이 완료 된 오브젝트를 다시 큐에 넣을때 호출 파라미터->비활성화 할 오브젝트
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        instance.objectPool.Enqueue(obj); //다시 큐에 넣음
    }
}