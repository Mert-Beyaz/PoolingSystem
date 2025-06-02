using UnityEngine;
using System.IO;
using System.Linq;

[CreateAssetMenu(fileName = "PoolItem", menuName = "Pooling/Pool Item")]
public class PoolItemSO : ScriptableObject
{
    [Tooltip("PoolType enum ismi (örn: 'Enemy', 'Projectile')")]
    [SerializeField] private string poolTypeName;

    public GameObject Prefab;
    public int InitialSize = 10;

    private PoolType poolType;
    public PoolType PoolType => poolType;

    private const string enumFilePath = "Assets/Scripts/Enums/PoolType.cs"; // enum dosyanın yolunu projene göre ayarla

    [ContextMenu("Add Enum")]
    private void AddEnumToPoolType()
    {
        if (string.IsNullOrWhiteSpace(poolTypeName))
        {
            Debug.LogError("[PoolItemSO] poolTypeName boş olamaz.");
            return;
        }

        if (!File.Exists(enumFilePath))
        {
            Debug.LogError($"[PoolItemSO] Enum dosyası bulunamadı: {enumFilePath}");
            return;
        }

        string[] lines = File.ReadAllLines(enumFilePath);
        int startIndex = -1;
        int endIndex = -1;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("public enum PoolType"))
                startIndex = i;
            else if (startIndex != -1 && lines[i].Contains("}"))
            {
                endIndex = i;
                break;
            }
        }

        if (startIndex == -1 || endIndex == -1)
        {
            Debug.LogError("[PoolItemSO] PoolType enum bloğu bulunamadı.");
            return;
        }

        bool alreadyExists = lines
            .Skip(startIndex)
            .Take(endIndex - startIndex)
            .Any(line => line.Trim().Replace(",", "") == poolTypeName);

        if (alreadyExists)
        {
            Debug.LogWarning($"[PoolItemSO] '{poolTypeName}' zaten enum’da var.");
            return;
        }

        // enum bloğuna yeni satır ekle
        lines[endIndex] = $"    {poolTypeName},\n{lines[endIndex]}";
        File.WriteAllLines(enumFilePath, lines);
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log($"✅ '{poolTypeName}' PoolType enum'ına eklendi.");
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (EnumUtils.TryToEnum(poolTypeName, out PoolType result))
        {
            poolType = result;
        }
#endif
    }
}
