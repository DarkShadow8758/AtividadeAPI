using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class MenuUI : MonoBehaviour
{
    [Header("Referências da UI")]
    public GameObject painelMenu;
    public TextMeshProUGUI textoNome;
    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoItens;

    [Header("Lista de Itens")]
    public Transform contentItens;
    public GameObject itemPrefab;
    public ScrollRect scrollView;

    private GameApiService api;
    private Jogador jogadorAtual;
    private bool menuAberto = false;

    void Start()
    {
        painelMenu.SetActive(false);
        api = new GameApiService();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuAberto)
                Fechar();
            else
                _ = AbrirMenuAsync();
        }
    }

    public async Task AbrirMenuAsync()
{
    painelMenu.SetActive(true);
    menuAberto = true;

    jogadorAtual = await api.GetJogador("1");
    textoNome.text = "Nome: " + jogadorAtual.Nome;
    textoVida.text = "Vida: " + jogadorAtual.Vida;

    ItemJogador[] itens = await api.GetItensJogador("1");
    textoItens.text = "Itens: " + itens.Length;

    await AtualizarListaItens(itens);
}


    public async void Recarregar()
    {
        jogadorAtual = await api.GetJogador("1");
        textoNome.text = "Nome: " + jogadorAtual.Nome;
        textoVida.text = "Vida: " + jogadorAtual.Vida;

        ItemJogador[] itens = await api.GetItensJogador("1");
        await AtualizarListaItens(itens);
    }

    private async Task AtualizarListaItens(ItemJogador[] itens)
{
    foreach (Transform child in contentItens)
        Destroy(child.gameObject);

    if (itens.Length == 0)
    {
        GameObject semItens = Instantiate(itemPrefab, contentItens);
        semItens.GetComponent<TextMeshProUGUI>().text = "Nenhum item coletado.";
        return;
    }

    foreach (var item in itens)
    {
        GameObject novoItem = Instantiate(itemPrefab, contentItens);
        novoItem.GetComponent<TextMeshProUGUI>().text = $"{item.Nome} — {item.Descricao} (Dano: {item.Dano})";
    }

    if (scrollView != null)
        scrollView.verticalNormalizedPosition = 1;
}

public async Task AtualizarItensExternamente()
{
    ItemJogador[] itens = await api.GetItensJogador("1");

    textoItens.text = "Itens: " + itens.Length;
    await AtualizarListaItens(itens);
}


    public void Fechar()
    {
        painelMenu.SetActive(false);
        menuAberto = false;
    }
}
