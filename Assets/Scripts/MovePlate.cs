using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;
    
    //board positions
    int matrixX;
    int matrixY;

    public bool attack = false;

    public void Start()
    {
        // Ensure collider so OnMouseUp works and size it to sprite
        if (GetComponent<Collider2D>() == null)
        {
            var bc = gameObject.AddComponent<BoxCollider2D>();
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                bc.size = sr.sprite.bounds.size;
            }
            Debug.Log($"[MovePlate] Added BoxCollider2D to MovePlate instance");
        }

        if (attack)
        {
            //change to red
            var sr = gameObject.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        Debug.Log($"[MovePlate] Clicked target {matrixX},{matrixY} attack={attack}");
        controller = GameObject.FindGameObjectWithTag("GameController");
        Game gameScript = controller.GetComponent<Game>();

        if (attack)
        {
            GameObject cp = gameScript.GetPosition(matrixX, matrixY);
            if (cp != null) Destroy(cp);
        }

        // YENÝ: Eðer oyun durdurulduysa plaka tetiklenmesin
        if (gameScript.IsGamePaused())
        {
            return;
        }

        // ... Mevcut hamle yapma kodlarýn aynen devam ediyor ...
        Debug.Log($"[MovePlate] Clicked target {matrixX},{matrixY} attack={attack}");
        // ...
        int oldX = reference.GetComponent<Chessman>().GetXBoard();
        int oldY = reference.GetComponent<Chessman>().GetYBoard();

        gameScript.SetPositionEmpty(oldX, oldY);

        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        gameScript.SetPosition(reference);

        // YENÝ: Piyon terfi kontrolü
        if (gameScript.CheckPawnPromotion(reference, matrixX, matrixY))
        {
            reference.GetComponent<Chessman>().DestroyMovePlates();
            // Paneli aç (Sýra deðiþimi paneldeki seçimden sonra tetiklenecek)
            gameScript.OpenPromotionMenu(reference, matrixX, matrixY);
        }
        else
        {
            // Normal hamleyse sýrayý direkt geçir
            gameScript.NextTurn();
            reference.GetComponent<Chessman>().DestroyMovePlates();
        }

        PlayMoveSound soundScript = Object.FindAnyObjectByType<PlayMoveSound>();
        if (soundScript != null) soundScript.PlaySound();
    }
    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }
    public void SetReference(GameObject obj)
    {
        reference = obj;
    }
    public GameObject GetReference()
    { return reference; }

}
