using UnityEngine;

public class Secret : MonoBehaviour
{

    [SerializeField] private GameObject OrangeFish;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private float spawnTimer = 30f;
    private float animationTimer = 5f;
    private bool isSpawn = false;

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f && isSpawn == false)
        {
            Instantiate(OrangeFish , canvasTransform);
            DestroyGameObject();
            isSpawn = true;
            spawnTimer = 30f;
        }
    }

    private void DestroyGameObject()
    {
        animationTimer -= Time.deltaTime;
        if (animationTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
