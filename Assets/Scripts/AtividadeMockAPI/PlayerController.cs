using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Atributos do Jogador")]
    public int vida = 100;
    public float vel = 5f;

    [Header("Referências de UI")]
    public TextMeshProUGUI textoAutoSave;

    private GameApiService api;
    private Jogador jogadorAtual;
    private Rigidbody2D rb;
    private Vector2 direcao;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        api = new GameApiService();
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
        rb.linearVelocity = direcao * vel;
    }

    private async void CarregarJogador()
    {
        jogadorAtual = await api.GetJogador("1");

        if (jogadorAtual != null)
        {
            int valorVida = 100;
            if (!int.TryParse(jogadorAtual.Vida, out valorVida))
                valorVida = 100;

            vida = valorVida;
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
    }

    private async Task AtualizarJogador()
    {
        if (jogadorAtual == null)
            jogadorAtual = await api.GetJogador("1");

        jogadorAtual.Vida = vida.ToString();
        await api.AtualizarJogador(jogadorAtual.id, jogadorAtual);
    }

    public System.Collections.IEnumerator ShowAutoSave()
    {
        textoAutoSave.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        textoAutoSave.gameObject.SetActive(false);
    }
}
