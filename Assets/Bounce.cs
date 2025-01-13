using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli kütüphane

public class Bounce : MonoBehaviour
{
    public float jumpForce = 10f;
    public float randomForceRange = 5f;
    private GameObject firstObject = null;
    private GameObject secondObject = null;
    public static int score = 0;
    public static int level = 1;  // Seviye deðiþkeni
    public Random_Object objectSpawner;  // Nesne oluþturma için referans

    public ParticleSystem destructionEffect;  // Patlama efekti için bir ParticleSystem

    private float timeRemaining = 30f;  // Süreyi tutacak deðiþken
    private bool isGameOver = false;    // Oyunun bitip bitmediðini kontrol eder

    void Start()
    {
        if (objectSpawner == null)
        {
            objectSpawner = FindObjectOfType<Random_Object>();
            if (objectSpawner == null)
            {
                Debug.LogError("Random_Object scriptine referans bulunamadý!");
            }
        }
    }

    void Update()
    {
        if (isGameOver)
        {
            // Eðer oyun bitmiþse "R" tuþuna basýldýðýnda yeniden baþlat
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            return;
        }

        // Zamaný azalt
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");

        // Oyunu durdur
        Time.timeScale = 0;
    }

    void RestartGame()
    {
        // Skor ve seviye deðiþkenlerini sýfýrla
        score = 0;
        level = 1;

        // Oyunu yeniden baþlatmak için sahneyi yeniden yükle
        Time.timeScale = 1; // Oyun zamanýný tekrar baþlat
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGameOver)
            return;

        if (other.CompareTag("Player"))
        {
            if (firstObject == null)
            {
                firstObject = other.gameObject;
            }
            else if (secondObject == null)
            {
                secondObject = other.gameObject;

                if (firstObject.name == secondObject.name)
                {
                    CreateDestructionEffect(firstObject.transform.position);
                    CreateDestructionEffect(secondObject.transform.position);

                    Destroy(firstObject);
                    Destroy(secondObject);
                    score += 20;
                }
                else
                {
                    ApplyRandomForce(firstObject.GetComponent<Rigidbody>());
                    ApplyRandomForce(secondObject.GetComponent<Rigidbody>());
                }

                ResetObjects();
                CheckAndRespawnObjects();
            }
        }
    }

    void ResetObjects()
    {
        firstObject = null;
        secondObject = null;
    }

    void CheckAndRespawnObjects()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 7)  // Eðer tüm nesneler yok olduysa
        {
            level++;  // Seviye artýr
            timeRemaining = 30f;  // Süreyi sýfýrla

            // Nesne sayýsýný artýr ve yeni nesneleri oluþtur
            if (objectSpawner != null)
            {
                objectSpawner.IncreaseObjectCount(); // Nesne sayýsýný artýr
                objectSpawner.SpawnObjects();       // Yeni nesneleri oluþtur
            }
            else
            {
                Debug.LogError("objectSpawner referansý atanmadý!");
            }
        }
    }

    void CreateDestructionEffect(Vector3 position)
    {
        if (destructionEffect != null)
        {
            ParticleSystem effectInstance = Instantiate(destructionEffect, position, Quaternion.identity);
            Destroy(effectInstance.gameObject, effectInstance.main.duration);
        }
    }

    void ApplyRandomForce(Rigidbody rb)
    {
        if (rb != null)
        {
            Vector3 randomForce = new Vector3(
                Random.Range(3, randomForceRange),
                Random.Range(0, jumpForce),
                Random.Range(3, randomForceRange)
            );

            rb.AddForce(randomForce, ForceMode.Impulse);
        }
    }

    void OnGUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);

        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        // Puan ve seviye bilgilerini göster
        GUI.Label(new Rect(Screen.width - 100, 10, 200, 50), "Puan: " + score, style);
        GUI.Label(new Rect(Screen.width - 100, 50, 200, 50), "Seviye: " + level, style);
        GUI.Label(new Rect(10, 10, 200, 50), timeText, style);

        // Oyun bittiðinde ekrana mesaj yazdýr
        if (isGameOver)
        {
            GUIStyle gameOverStyle = new GUIStyle(style);
            gameOverStyle.fontSize = 55;
            gameOverStyle.normal.textColor = Color.red;
            gameOverStyle.alignment = TextAnchor.MiddleCenter;

            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 120, 200, 50), "Your Score is " + score, style);
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 50), "Game Over!", gameOverStyle);
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 50), "Press R to restart the game", style);
        }
    }
}