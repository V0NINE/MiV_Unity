using UnityEngine;

public class CubeRows : MonoBehaviour
{
    public GameObject cubePrefab; // Asigna un prefab de cubo aquí
    public int cubesPerRow = 10; // Cantidad de cubos por fila
    public float spacing = 4f; // Espaciado entre cubos
    public float rowInterval = 1f; // Tiempo entre filas
    public float speed = 5f; // Velocidad de movimiento de los cubos
    public float startZ = 50f; // Posición inicial en z
    public float fixedY = -20f; // Posición fija en y

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= rowInterval)
        {
            SpawnRow();
            timer = 0f;
        }
    }

    void SpawnRow()
    {
        for (int i = -100; i < cubesPerRow; i++) // Cambié el rango para generar filas completas.
        {
            Vector3 position = new Vector3(i * spacing, fixedY, startZ); 
            GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
            cube.AddComponent<GameObjectDestroyer>(); // Agrega el script de destrucción al cubo.
            Rigidbody rb = cube.GetComponent<Rigidbody>();
            if (rb == null) rb = cube.AddComponent<Rigidbody>(); // Verifica si ya existe
            rb.useGravity = false; // Asegúrate de que no caigan debido a la gravedad.
            rb.linearVelocity = Vector3.back * speed; // Establece la velocidad hacia atrás.
        }
    }
}
