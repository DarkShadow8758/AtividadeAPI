using UnityEngine;

public class ItemEx : MonoBehaviour
{
    public string nomeItem = "Espada de Ferro";
    public string descricaoItem = "Uma espada simples e resistente.";
    public string danoItem = "10";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Item coletado: {nomeItem}");
            StartCoroutine(ColetarItem());
        }
    }

    private System.Collections.IEnumerator ColetarItem()
    {
        var task = ItemManager.Instance.AdicionarItemAoJogador("1", nomeItem, descricaoItem, danoItem);
        while (!task.IsCompleted)
            yield return null;
            
        var player = FindAnyObjectByType<PlayerController>();
        if (player != null)
            player.StartCoroutine(player.ShowAutoSave());

        var menu = FindAnyObjectByType<MenuUI>();
        if (menu != null && menu.gameObject.activeSelf)
        {
            var updateTask = menu.AtualizarItensExternamente();
            while (!updateTask.IsCompleted)
                yield return null;
        }

        Destroy(gameObject);
    }
}
