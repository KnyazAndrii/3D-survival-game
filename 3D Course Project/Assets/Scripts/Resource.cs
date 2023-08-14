using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Resource : MonoBehaviour
{
    public float Health;
    private float _randDistace = 1.3f;
    public int _countMin = 2;
    public int _countMax = 5;

    public GameObject WoodPrefab;

    private bool isCanSpawnResourse = true;

    public enum Type
    {
        wood,
        stone,
        ore
    }

    public Type type;

    public void Damage(float damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            switch(type)
            {
                case Type.wood:
                    GetComponent<Rigidbody>().isKinematic = false;

                    StartCoroutine(SpawnResourseTimer());
                    isCanSpawnResourse = false;

                    Destroy(gameObject, 3f);
                    break;
                case Type.stone:
                    Destroy(gameObject);
                    break;
            }
        }
    }

    IEnumerator SpawnResourseTimer()
    {
        if (isCanSpawnResourse)
        {
            yield return new WaitForSeconds(2);

            int count = Random.Range(_countMin, _countMax);

            for (int i = 1; i < count; i++)
            {
                Vector3 spawnPosition = transform.position + transform.up * i * _randDistace;
                Instantiate(WoodPrefab, spawnPosition, Quaternion.identity, null);
            }
        }
    }
}
