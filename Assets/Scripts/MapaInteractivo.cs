using UnityEngine;
using UnityEngine.UI;

public class MapaInteractivo : MonoBehaviour
{
    [Header("Activación")]
    public float distanciaActivacion = 1.5f;
    public KeyCode teclaAbrir = KeyCode.E;

    [Header("Jugador")]
    public GameObject player; // si no lo asignas, se busca por tag "Player"

    [Header("Datos del mapa para este punto")]
    public Sprite mapaSprite;                         // sprite de mapa que quieres mostrar
    public bool usarPosNormalizada = true;            // true = [0..1], false = píxeles
    public Vector2 posNormalizada = new Vector2(0.5f, 0.5f); // (0,0) inf-izq; (1,1) sup-der
    public Vector2 posPixeles = Vector2.zero;         // relativo al centro del rect
    public Vector2 offsetPixeles = Vector2.zero;      // ajuste fino

    // cache resuelta por tag cuando hace falta
    private GameObject panelMapaGO;     // mismo objeto que tiene el Image del mapa
    private Image mapaImage;            // Image del mismo panel
    private RectTransform marcadorRT;   // hijo marcador

    private void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        float dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist <= distanciaActivacion && Input.GetKeyDown(teclaAbrir))
        {
            ToggleMapa();
        }
    }

    private void ToggleMapa()
    {
        if (!ResolverRefsPorTag()) return;

        bool next = !panelMapaGO.activeSelf;

        if (next)
        {
            // Mostrar y configurar sprite + marcador
            if (mapaSprite != null) mapaImage.sprite = mapaSprite;
            // mapaImage.SetNativeSize(); // si quieres forzar tamaño nativo

            ColocarMarcador();
            panelMapaGO.SetActive(true);
        }
        else
        {
            panelMapaGO.SetActive(false);
        }
    }

    private bool ResolverRefsPorTag()
    {
        if (panelMapaGO == null)
        {
            panelMapaGO = FindByTagIncludingInactive("MapPanel");
            if (panelMapaGO == null)
            {
                Debug.LogWarning("MapaInteractivo: no se encontró tag 'MapPanel'.");
                return false;
            }
        }

        if (mapaImage == null)
        {
            mapaImage = panelMapaGO.GetComponent<Image>();
            if (mapaImage == null)
            {
                Debug.LogWarning("MapaInteractivo: el objeto con tag 'MapPanel' no tiene Image.");
                return false;
            }
        }

        if (marcadorRT == null)
        {
            var markGO = FindByTagIncludingInactive("MapMarker");
            if (markGO == null)
            {
                Debug.LogWarning("MapaInteractivo: no se encontró tag 'MapMarker'.");
                return false;
            }
            marcadorRT = markGO.GetComponent<RectTransform>();
        }

        return true;
    }

    private GameObject FindByTagIncludingInactive(string tag)
{
    // Primero intenta los activos (rápido)
    var go = GameObject.FindGameObjectWithTag(tag);
    if (go != null) return go;

    // Ahora incluye inactivos:
    // Nota: FindObjectsOfType<GameObject>(true) está disponible en 2020.1+
    var all = Object.FindObjectsOfType<GameObject>(true);
    foreach (var g in all)
    {
        // Filtra assets/previews fuera de escena
        if (!g.scene.IsValid()) continue;
        if (g.CompareTag(tag)) return g;
    }
    return null;
}

    private void ColocarMarcador()
    {
        if (marcadorRT == null || mapaImage == null) return;

        RectTransform mapRT = mapaImage.rectTransform;
        Vector2 rectSize = mapRT.rect.size;

        Vector2 anchored;
        if (usarPosNormalizada)
        {
            anchored = new Vector2(
                (posNormalizada.x - 0.5f) * rectSize.x,
                (posNormalizada.y - 0.5f) * rectSize.y
            );
        }
        else
        {
            anchored = posPixeles; // relativo al centro
        }

        anchored += offsetPixeles;
        marcadorRT.anchoredPosition = anchored;
    }
}
