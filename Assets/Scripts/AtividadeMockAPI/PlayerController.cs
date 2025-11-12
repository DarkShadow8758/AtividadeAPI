using UnityEngine;
using System.Threading.Tasks;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Atributos do Jogador")]
    public int vida = 100;
    public int qtdItens = 0;
    public float velocidade = 5f;

    [Header("Referências de UI")]
    public TextMeshProUGUI textoAutoSave;

    private LocalApiService api;
    private Player jogadorAtual;
    private Rigidbody2D rb;
    private Vector2 direcao;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        api = new LocalApiService();
        textoAutoSave.gameObject.SetActive(false);
        CarregarJogador();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        direcao = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direcao * velocidade;
    }

    private async void CarregarJogador()
    {
        jogadorAtual = await api.GetPlayerById("1");

        if (jogadorAtual != null)
        {
            jogadorAtual.Id = "1";
            int.TryParse(jogadorAtual.Vida, out vida);
            int.TryParse(jogadorAtual.QuantidadeItens, out qtdItens);
        }
    }

    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            vida -= 10;
            if (vida < 0) vida = 0;
            await AtualizarJogador();
        }

        if (other.CompareTag("Item"))
        {
            qtdItens++;
            await AtualizarJogador();
            Destroy(other.gameObject);
            StartCoroutine(ShowAutoSave());
        }
    }

    private async Task AtualizarJogador()
    {
        jogadorAtual.Vida = vida.ToString();
        jogadorAtual.QuantidadeItens = qtdItens.ToString();
        await api.UpdatePlayer(jogadorAtual.Id, jogadorAtual);
    }

    public System.Collections.IEnumerator ShowAutoSave()
    {
        textoAutoSave.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        textoAutoSave.gameObject.SetActive(false);
    }
}
