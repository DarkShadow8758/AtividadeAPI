using UnityEngine;
using System.Threading.Tasks;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    private GameApiService api;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    async void Start()
    {
        api = new GameApiService();
        await ListarItens();
    }

    public async Task AdicionarItemAoJogador(string jogadorId, string nome, string descricao, string dano)
    {
        ItemJogador novoItem = new ItemJogador
        {
            JogadorId = jogadorId,
            Nome = nome,
            Descricao = descricao,
            Dano = dano
        };

        await api.AdicionarItem(jogadorId, novoItem);
        Debug.Log($"Item '{nome}' adicionado ao jogador {jogadorId}!");
    }

    public async Task ListarItens()
    {
        ItemJogador[] itens = await api.GetItensJogador("1");
        Debug.Log($"Total de itens do jogador: {itens.Length}");
        foreach (var item in itens)
        {
            Debug.Log($"• {item.Nome} - {item.Descricao} (Dano: {item.Dano})");
        }
    }
}
