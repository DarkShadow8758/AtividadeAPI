using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocalApiService
{
    private readonly HttpClient httpClient;
    private const string BASE_URL = "https://localhost:7116/api";


    public LocalApiService()
    {
        httpClient = new HttpClient();
    }

    public async Task<Player[]> GetAllPlayers()
    {
        try
        {
            string url = $"{BASE_URL}/players";
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            string wrapped = $"{{\"players\":{json}}}";
            PlayerArray wrapper = JsonUtility.FromJson<PlayerArray>(wrapped);
            return wrapper.players;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao buscar todos jogadores: {ex.Message}");
            return Array.Empty<Player>();
        }
    }

    public async Task<Player> GetPlayerById(string id)
    {
        try
        {
            string url = $"{BASE_URL}/player/{id}";
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonUtility.FromJson<Player>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao buscar jogador {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<Player> UpdatePlayer(string id, Player updated)
    {
        try
        {
            string url = $"{BASE_URL}/player/{id}";
            string json = JsonUtility.ToJson(updated);
            Debug.Log($"Enviando PUT para {url} com body:\n{json}");
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();
            return JsonUtility.FromJson<Player>(responseJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao atualizar jogador {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<Player> CreatePlayer(Player novo)
    {
        try
        {
            string url = $"{BASE_URL}/player";
            string json = JsonUtility.ToJson(novo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();
            return JsonUtility.FromJson<Player>(responseJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao criar jogador: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeletePlayer(string id)
    {
        try
        {
            string url = $"{BASE_URL}/player/{id}";
            var response = await httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao deletar jogador {id}: {ex.Message}");
            return false;
        }
    }

    [Serializable]
    private class PlayerArray
    {
        public Player[] players;
    }
}
