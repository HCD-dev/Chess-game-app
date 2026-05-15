using UnityEngine;
using UnityEngine.UI; // Image bileþeni için gerekli

public class SpawnPieces : MonoBehaviour
{
    [Header("Prefabs - White")]
    public GameObject wPawn;
    public GameObject wRook, wKnight, wBishop, wQueen, wKing;

    [Header("Prefabs - Black")]
    public GameObject bPawn;
    public GameObject bRook, bKnight, bBishop, bQueen, bKing;

    [Header("Settings")]
    public Transform boardTransform;
    private float step = 0.22f;
    private float startX = -0.77f;
    private float startY_White_Back = -0.765f;
    private float startY_White_Pawn = -0.545f;

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            float xPos = startX + (i * step);

            // --- BEYAZLAR ---
            Spawn(wPawn, xPos, startY_White_Pawn, $"White_Pawn_{i + 1}");

            string whitePieceName;
            GameObject wPiece = GetBackRowPiece(i, true, out whitePieceName);
            Spawn(wPiece, xPos, startY_White_Back, $"White_{whitePieceName}");

            // --- SÝYAHLAR ---
            float blackPawnY = startY_White_Pawn + (5 * step);
            float blackBackY = startY_White_Back + (7 * step);

            Spawn(bPawn, xPos, blackPawnY, $"Black_Pawn_{i + 1}");

            string blackPieceName;
            GameObject bPiece = GetBackRowPiece(i, false, out blackPieceName);
            Spawn(bPiece, xPos, blackBackY, $"Black_{blackPieceName}");
        }
    }

    GameObject GetBackRowPiece(int index, bool isWhite, out string pieceName)
    {
        // Ýsimleri burada belirliyoruz
        switch (index)
        {
            case 0: case 7: pieceName = (index == 0) ? "Rook_1" : "Rook_2"; return isWhite ? wRook : bRook;
            case 1: case 6: pieceName = (index == 1) ? "Knight_1" : "Knight_2"; return isWhite ? wKnight : bKnight;
            case 2: case 5: pieceName = (index == 2) ? "Bishop_1" : "Bishop_2"; return isWhite ? wBishop : bBishop;
            case 3: pieceName = "Queen"; return isWhite ? wQueen : bQueen;
            case 4: pieceName = "King"; return isWhite ? wKing : bKing;
            default: pieceName = "Unknown"; return null;
        }
    }

    void Spawn(GameObject prefab, float x, float y, string name)
    {
        if (prefab == null) return;
        GameObject go = Instantiate(prefab, boardTransform);

        // RectTransform kullanýyorsan position ayarý önemlidir
        RectTransform rt = go.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.localPosition = new Vector3(x, y, 0);
            rt.localScale = Vector3.one; // UI'da scale bozulmasýn
        }
        else
        {
            rt.localPosition = new Vector3(x, y, -1f);
        }

        go.name = name;
    }
}