using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    private Dictionary<string, Queue<PooledObject>> poolDictionary = new Dictionary<string, Queue<PooledObject>>();
    public List<Pool> pools;
    public Ability ability;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public PooledObject prefab;
        public int poolCount;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        foreach(Pool pool in pools)
        {
            Queue<PooledObject> poolQueue = new Queue<PooledObject>();

            for (int i = 0; i < pool.poolCount; i++)
            {
                PooledObject gameObject = Instantiate(pool.prefab, this.transform.position, this.transform.rotation);
                gameObject.gameObject.SetActive(false);
                poolQueue.Enqueue(gameObject);
            }
        }
    }

    public PooledObject GetSpawnObjects(string tag, Vector3 position, Vector3 vector3)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

        PooledObject pooledObject = poolDictionary[tag].Dequeue();
        pooledObject.gameObject.SetActive(true);
        return pooledObject;
    }

}

public enum Ability
{
    Fire,
    Water,
    Ice
}
