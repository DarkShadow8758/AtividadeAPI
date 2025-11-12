using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemService
{
    private readonly HttpClient httpClient;
    private const string BASE_URL = "https://localhost:7116/api/itens";


    public ItemService()
    {
        httpClient = new HttpClient();
    }

    // 🔹 Busca os itens de um jogador específico
    public async Task<ItemJogador[]> GetItensJogador(string jogadorId)
    {
        try
        {
            string url = $"{BASE_URL}/jogador/{jogadorId}";
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            string wrapped = $"{{\"itens\":{json}}}";
            ItemArray wrapper = JsonUtility.FromJson<ItemArray>(wrapped);
            return wrapper.itens;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao buscar itens do jogador {jogadorId}: {ex.Message}");
            return Array.Empty<ItemJogador>();
        }
    }

    // 🔹 Cria um novo item (POST)
    public async Task<ItemJogador> CriarItem(ItemJogador item)
    {
        try
        {
            string json = JsonUtility.ToJson(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(BASE_URL, content);
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();
            return JsonUtility.FromJson<ItemJogador>(responseJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao criar item: {ex.Message}");
            return null;
        }
    }

    // 🔹 Atualiza um item existente (PUT)
    public async Task<ItemJogador> AtualizarItem(string id, ItemJogador item)
    {
        try
        {
            string url = $"{BASE_URL}/{id}";
            string json = JsonUtility.ToJson(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();
            return JsonUtility.FromJson<ItemJogador>(responseJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao atualizar item {id}: {ex.Message}");
            return null;
        }
    }

    // 🔹 Deleta um item (DELETE)
    public async Task<bool> DeletarItem(string id)
    {
        try
        {
            string url = $"{BASE_URL}/{id}";
            var response = await httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao deletar item {id}: {ex.Message}");
            return false;
        }
    }

    // 🔹 Classe auxiliar para desserialização
    [Serializable]
    private class ItemArray
    {
        public ItemJogador[] itens;
    }
}
