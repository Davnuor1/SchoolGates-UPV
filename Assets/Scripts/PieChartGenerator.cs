using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PieChartGenerator : MonoBehaviour
{
    [SerializeField] private GameObject pieSlicePrefab; // Prefab del sector
    [SerializeField] private GameObject textPrefab; // Prefab del texto
    [SerializeField] private Transform pieChartContainer; // Contenedor del gráfico circular
    [SerializeField] private float chartRadius = 100f; // Radio efectivo del gráfico

    public void GeneratePieChart(List<(string nombre, int rating)> likedMirrors)
    {
        // Limpiar el contenedor
        foreach (Transform child in pieChartContainer)
        {
            Destroy(child.gameObject);
        }

        // Calcular el total de ratings
        int totalRating = 0;
        foreach (var mirror in likedMirrors)
        {
            totalRating += mirror.rating;
        }

        float currentAngle = 0f;

        for (int i = 0; i < likedMirrors.Count; i++)
        {
            var mirror = likedMirrors[i];
            float sliceAngle = (mirror.rating / (float)totalRating) * 360f;

            // Instanciar el prefab del sector
            GameObject sliceObj = Instantiate(pieSlicePrefab, pieChartContainer);
            RectTransform rt = sliceObj.GetComponent<RectTransform>();
            rt.localPosition = Vector3.zero;
            rt.localRotation = Quaternion.identity;

            // Configurar la imagen del slice
            Image sliceImage = sliceObj.GetComponent<Image>();
            sliceImage.fillAmount = sliceAngle / 360f;
            sliceObj.transform.localRotation = Quaternion.Euler(0, 0, -currentAngle);

            // Calcular el ángulo medio del sector (en radianes)
            float midAngleDeg = currentAngle + sliceAngle / 2f;
            float midAngleRad = midAngleDeg * Mathf.Deg2Rad;

            // Crear un nuevo objeto de texto (no como hijo del sector)
            GameObject textObj = Instantiate(textPrefab, pieChartContainer);
            TMP_Text sliceText = textObj.GetComponent<TMP_Text>();
            if (sliceText != null)
            {
                sliceText.text = mirror.nombre;

                RectTransform textRT = sliceText.GetComponent<RectTransform>();
                float textRadius = chartRadius * 0.6f; // El 60% del radio para mejor visibilidad

                // Posición del texto en el centro del sector visible
                Vector2 offset = new Vector2(textRadius * Mathf.Cos(midAngleRad),
                                             textRadius * Mathf.Sin(midAngleRad));

                textRT.anchoredPosition = offset;
                textRT.localRotation = Quaternion.identity; // Mantener el texto horizontal
                textRT.gameObject.SetActive(true);
            }

            currentAngle += sliceAngle;
        }
    }
}
