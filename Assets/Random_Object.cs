using UnityEngine;

public class Random_Object : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Prefab'ler
    public int baseNumberOfObjects = 12; // Ýlk seviyedeki nesne sayýsý
    private int currentNumberOfObjects; // Güncel nesne sayýsý
    public Vector3 spawnAreaMin = new Vector3(-10, 0, -10);
    public Vector3 spawnAreaMax = new Vector3(10, 0, 10);

    void Start()
    {
        currentNumberOfObjects = baseNumberOfObjects; // Baþlangýçta 12 nesne
        SpawnObjects();
    }

    public void IncreaseObjectCount()
    {
        currentNumberOfObjects += 2; // Her seviyede nesne sayýsýný 2 artýr
    }

    public void SpawnObjects()
    {
        int prefabsCount = objectPrefabs.Length;

        // Nesne sayýsýný çift yapmak için gerekirse artýr
        if (currentNumberOfObjects % 2 != 0)
        {
            currentNumberOfObjects++;
        }

        // Her prefab'dan oluþturulacak minimum nesne sayýsýný hesapla
        int baseObjectsPerPrefab = 2;
        int remainingObjects = currentNumberOfObjects - (baseObjectsPerPrefab * prefabsCount);

        // Her prefab'dan oluþturulacak nesne sayýsýný hesapla
        int[] objectsPerPrefab = new int[prefabsCount];
        for (int i = 0; i < prefabsCount; i++)
        {
            objectsPerPrefab[i] = baseObjectsPerPrefab;
        }

        // Kalan nesneleri daðýt
        int index = 0;
        while (remainingObjects > 0)
        {
            objectsPerPrefab[index] += 2; // Çift sayýda ekle
            remainingObjects -= 2;
            index = (index + 1) % prefabsCount;
        }

        // Nesneleri sahneye yerleþtir
        for (int i = 0; i < prefabsCount; i++)
        {
            for (int j = 0; j < objectsPerPrefab[i]; j++)
            {
                float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
                float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
                float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
                Vector3 spawnPosition = new Vector3(x, y, z);

                Instantiate(objectPrefabs[i], spawnPosition, Quaternion.identity);
            }
        }
    }
}
