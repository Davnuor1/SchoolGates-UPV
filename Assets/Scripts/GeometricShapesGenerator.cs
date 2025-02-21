using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeometricShapesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject shapePrefab; // Prefab de la figura geométrica
    [SerializeField] private Transform shapeContainer; // Contenedor para las figuras geométricas

    [Header("Shape Size Settings")]
    [SerializeField] private float minShapeSize = 50f; // Tamaño mínimo de las figuras
    [SerializeField] private float maxShapeSize = 200f; // Tamaño máximo de las figuras

    [Header("Text Size Settings")]
    [SerializeField] private float minFontSize = 10f; // Tamaño mínimo del texto
    [SerializeField] private float maxFontSize = 50f; // Tamaño máximo del texto

    [Header("Spacing Settings")]
    [SerializeField] private float minSpacing = 10f; // Espacio mínimo entre las figuras

    private List<Rect> occupiedAreas = new List<Rect>();

    /// <summary>
    /// Genera las figuras geométricas con un posicionamiento más caótico y sin superposición.
    /// </summary>
    public void GenerateShapes(List<(string nombre, int rating, Sprite figuraGeometrica)> likedMirrors)
    {
        // Limpiar el contenedor de figuras anteriores
        foreach (Transform child in shapeContainer)
        {
            Destroy(child.gameObject);
        }

        occupiedAreas.Clear();

        int totalRating = 0;
        foreach (var mirror in likedMirrors)
        {
            totalRating += mirror.rating;
        }

        float containerWidth = shapeContainer.GetComponent<RectTransform>().rect.width;
        float containerHeight = shapeContainer.GetComponent<RectTransform>().rect.height;

        int maxAttempts = 10000000; // Número máximo de intentos para ubicar una figura sin superposición

        foreach (var (nombre, rating, figuraGeometrica) in likedMirrors)
        {
            GameObject shapeObj = Instantiate(shapePrefab, shapeContainer);
            RectTransform rt = shapeObj.GetComponent<RectTransform>();

            // Ajustar el tamaño del rectángulo basado en el rating y las restricciones de tamaño
            float sizeFactor = (float)rating / totalRating;
            float shapeSize = Mathf.Lerp(minShapeSize, maxShapeSize, sizeFactor);

            rt.sizeDelta = new Vector2(shapeSize, shapeSize);

            // Asignar la imagen de la figura geométrica
            Image shapeImage = shapeObj.GetComponent<Image>();
            shapeImage.sprite = figuraGeometrica;

            // Configurar el texto dentro de la figura con un tamaño proporcional
            TMP_Text shapeText = shapeObj.GetComponentInChildren<TMP_Text>();
            if (shapeText != null)
            {
                shapeText.text = nombre;
                // Calcular el tamaño de la fuente en proporción al tamaño de la figura
                float fontSizeFactor = (shapeSize - minShapeSize) / (maxShapeSize - minShapeSize);
                float fontSize = Mathf.Lerp(minFontSize, maxFontSize, fontSizeFactor);

                shapeText.enableAutoSizing = false; // Desactivar el auto-sizing
                shapeText.fontSize = fontSize;
                shapeText.ForceMeshUpdate(); // Forzar la actualización del texto

                Debug.Log($"Nombre: {nombre}, Rating: {rating}, Tamaño Figura: {shapeSize}, Tamaño Texto: {fontSize}");
            }

            // Posicionamiento aleatorio no superpuesto
            bool positionFound = false;
            int attempts = 0;

            while (!positionFound && attempts < maxAttempts)
            {
                attempts++;
                // Generar una posición aleatoria dentro del contenedor
                float randomX = Random.Range(-containerWidth / 2 + shapeSize / 2, containerWidth / 2 - shapeSize / 2);
                float randomY = Random.Range(-containerHeight / 2 + shapeSize / 2, containerHeight / 2 - shapeSize / 2);

                Rect potentialRect = new Rect(new Vector2(randomX, randomY), new Vector2(shapeSize + minSpacing, shapeSize + minSpacing));

                if (!IsOverlapping(potentialRect))
                {
                    rt.anchoredPosition = new Vector2(randomX, randomY);
                    occupiedAreas.Add(potentialRect);
                    positionFound = true;
                }
            }

            if (!positionFound)
            {
                Debug.LogWarning($"No se encontró una posición válida para la figura {nombre} después de {maxAttempts} intentos.");
                Destroy(shapeObj);
            }
        }
    }

    /// <summary>
    /// Comprueba si un rectángulo propuesto se superpone con alguno de los rectángulos ya ocupados.
    /// </summary>
    private bool IsOverlapping(Rect newRect)
    {
        foreach (Rect occupiedRect in occupiedAreas)
        {
            if (newRect.Overlaps(occupiedRect))
            {
                return true;
            }
        }
        return false;
    }
}
