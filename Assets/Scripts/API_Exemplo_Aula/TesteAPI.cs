using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class TesteAPI : MonoBehaviour
{
    private GameApiService apiService;
    
    async void Start()
    {
        apiService = new GameApiService();
        
        Debug.Log("=== TESTE DA API ===");

        //Adicionar Jogadores
        Jogador novoJogador1 = new Jogador();
        novoJogador1.Nome = "Heroi";
        novoJogador1.Login = "heroi";
        novoJogador1.Senha = "123";
        novoJogador1.Fase = "1";
        novoJogador1.Vida = "100";
        novoJogador1.PosicaoX = 0;
        novoJogador1.PosicaoY = 0;
        novoJogador1.PosicaoZ = 0;
        //adicionar jogador na API
        Jogador criadoJogador1 = await apiService.CriarJogador(novoJogador1);
        Jogador novoJogador2 = new Jogador();
        novoJogador2.Nome = "Vilao";
        novoJogador2.Login = "vilao";
        novoJogador2.Senha = "123";
        novoJogador2.Fase = "1";
        novoJogador2.Vida = "100";
        novoJogador2.PosicaoX = 10;
        novoJogador2.PosicaoY = 0;
        novoJogador2.PosicaoZ = 0;
        //adicionar jogador na API
        Jogador criadoJogador2 = await apiService.CriarJogador(novoJogador2);
        Debug.Log($"Jogadores criados: {criadoJogador1.Nome} (ID: {criadoJogador1.id}), {criadoJogador2.Nome} (ID: {criadoJogador2.id})");
        //adicionar itens para o jogador 1
        ItemJogador novoItem1 = new ItemJogador();
        novoItem1.Nome = "Espada";
        novoItem1.Descricao = "Espada de A�o";
        novoItem1.Dano = "10";
        novoItem1.JogadorId = criadoJogador1.id;
        ItemJogador criadoItem1 = await apiService.AdicionarItem(criadoJogador1.id, novoItem1);
        ItemJogador novoItem2 = new ItemJogador();
        novoItem2.Nome = "Escudo";
        novoItem2.Descricao = "Escudo de Madeira";
        novoItem2.Dano = "5";
        novoItem2.JogadorId = criadoJogador1.id;
        ItemJogador criadoItem2 = await apiService.AdicionarItem(criadoJogador1.id, novoItem2);
        criadoJogador1.Vida = "80";
        Jogador atualizadoJogador1 = await apiService.AtualizarJogador(criadoJogador1.id, criadoJogador1);

        await MostrarTodosJogadores();


        Debug.Log("=== FIM DOS TESTES ===");
    }
    
    async Task MostrarTodosJogadores()
    {
        Jogador[] jogadores = await apiService.GetTodosJogadores();
        Debug.Log($"Total de jogadores: {jogadores.Length}");
        foreach (var jogador in jogadores)
        {
            Debug.Log($"Jogador: {jogador.Nome} (ID: {jogador.id}, Vida: {jogador.Vida})");
            ItemJogador[] itens = await apiService.GetItensJogador(jogador.id);
            Debug.Log($"  Itens ({itens.Length}):");
            foreach (var item in itens)
            {
                Debug.Log($"    - {item.Nome} (Dano: {item.Dano})");
            }
        }
    }


    void OnDestroy()
    {
        apiService?.Dispose();
    }
}