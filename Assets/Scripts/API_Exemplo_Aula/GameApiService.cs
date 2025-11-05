using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameApiService
{
    private readonly HttpClient httpClient;
    private const string BASE_URL = "https://68f95ab3deff18f212b951cc.mockapi.io";
    
    public GameApiService()
    {
        httpClient = new HttpClient();
    }
    
    #region Jogador Operations
    
    /// <summary>
    /// Busca todos os jogadores
    /// </summary>
    public async Task<Jogador[]> GetTodosJogadores()
    {
        try
        {
            string url = $"{BASE_URL}/Jogador";
            Debug.Log($"GET: {url}");
            
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            string json = await response.Content.ReadAsStringAsync();
            Debug.Log($"Resposta recebida: {json.Substring(0, Math.Min(200, json.Length))}...");
            
            // Como JsonUtility n�o suporta arrays diretamente, vamos usar um wrapper
            string wrappedJson = $"{{\"jogadores\":{json}}}";
            JogadorArray jogadorArray = JsonUtility.FromJson<JogadorArray>(wrappedJson);
            
            return jogadorArray.jogadores;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao buscar jogadores: {ex.Message}");
            return new Jogador[0];
        }
    }
    
    /// <summary>
    /// Busca um jogador espec�fico
    /// </summary>
    public async Task<Jogador> GetJogador(string id)
    {
        try
        {
            string url = $"{BASE_URL}/Jogador/{id}";
            Debug.Log($"GET: {url}");
            
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            string json = await response.Content.ReadAsStringAsync();
            Debug.Log($"Jogador recebido: {json}");
            
            Jogador jogador = JsonUtility.FromJson<Jogador>(json);
            return jogador;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao buscar jogador {id}: {ex.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// Atualiza dados do jogador
    /// </summary>
    public async Task<Jogador> AtualizarJogador(string id, Jogador jogador)
    {
        try
        {
            string url = $"{BASE_URL}/Jogador/{id}";
            Debug.Log($"PUT: {url}");
            
            string json = JsonUtility.ToJson(jogador);
            Debug.Log($"JSON sendo enviado: {json}");
            
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            
            string responseJson = await response.Content.ReadAsStringAsync();
            Debug.Log($"Jogador atualizado: {responseJson}");
            
            return JsonUtility.FromJson<Jogador>(responseJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao atualizar jogador {id}: {ex.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// Cria novo jogador
    /// </summary>
    public async Task<Jogador> CriarJogador(Jogador jogador)
    {
        try
        {
            string url = $"{BASE_URL}/Jogador";
            Debug.Log($"POST: {url}");
            
            string json = JsonUtility.ToJson(jogador);
            Debug.Log($"JSON sendo enviado: {json}");
            
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            
            string responseJson = await response.Content.ReadAsStringAsync();
            Debug.Log($"Jogador criado: {responseJson}");
            
            return JsonUtility.FromJson<Jogador>(responseJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao criar jogador: {ex.Message}");
            return null;
        }
    }
    
    #endregion
    
    #region Itens Operations
    
    /// <summary>
    /// Busca todos os itens de um jogador
    /// </summary>
    public async Task<ItemJogador[]> GetItensJogador(string jogadorId)
    {
        try
        {
            string url = $"{BASE_URL}/Jogador/{jogadorId}/Itens";
            Debug.Log($"GET: {url}");
            
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            string json = await response.Content.ReadAsStringAsync();
            Debug.Log($"Itens recebidos: {json.Substring(0, Math.Min(200, json.Length))}...");
            
            // Wrapper para array de itens
            string wrappedJson = $"{{\"itens\":{json}}}";
            ItemArray itemArray = JsonUtility.FromJson<ItemArray>(wrappedJson);
            
            return itemArray.itens;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao buscar itens do jogador {jogadorId}: {ex.Message}");
            return new ItemJogador[0];
        }
    }
    
    /// <summary>
    /// Adiciona novo item ao jogador
    /// </summary>
    public async Task<ItemJogador> AdicionarItem(string jogadorId, ItemJogador item)
    {
        try
        {
            string url = $"{BASE_URL}/Jogador/{jogadorId}/Itens";
            Debug.Log($"POST: {url}");
            
            // Garante que o JogadorId est� correto
            item.JogadorId = jogadorId;
            
            string json = JsonUtility.ToJson(item);
            Debug.Log($"JSON sendo enviado: {json}");
            
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            
            string responseJson = await response.Content.ReadAsStringAsync();
            Debug.Log($"Item adicionado: {responseJson}");
            
            return JsonUtility.FromJson<ItemJogador>(responseJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao adicionar item: {ex.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// Busca um item espec�fico do jogador
    /// </summary>
    public async Task<ItemJogador> GetItem(string jogadorId, string itemId)
    {
        try
        {
            string url = $"{BASE_URL}/Jogador/{jogadorId}/Itens/{itemId}";
            Debug.Log($"GET: {url}");
            
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            string json = await response.Content.ReadAsStringAsync();
            Debug.Log($"Item recebido: {json}");
            
            return JsonUtility.FromJson<ItemJogador>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao buscar item {itemId}: {ex.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// Atualiza um item espec�fico
    /// </summary>
    public async Task<ItemJogador> AtualizarItem(string jogadorId, string itemId, ItemJogador item)
    {
        try
        {
            string url = $"{BASE_URL}/Jogador/{jogadorId}/Itens/{itemId}";
            Debug.Log($"PUT: {url}");
            
            string json = JsonUtility.ToJson(item);
            Debug.Log($"JSON sendo enviado: {json}");
            
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            
            string responseJson = await response.Content.ReadAsStringAsync();
            Debug.Log($"Item atualizado: {responseJson}");
            
            return JsonUtility.FromJson<ItemJogador>(responseJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao atualizar item {itemId}: {ex.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// Remove um item
    /// </summary>
    public async Task<bool> RemoverItem(string jogadorId, string itemId)
    {
        try
        {
            string url = $"{BASE_URL}/Jogador/{jogadorId}/Itens/{itemId}";
            Debug.Log($"DELETE: {url}");
            
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            
            Debug.Log("Item removido com sucesso");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao remover item {itemId}: {ex.Message}");
            return false;
        }
    }
    
    #endregion
    
    public void Dispose()
    {
        httpClient?.Dispose();
    }
}

// Classes auxiliares para deserializa��o de arrays
[System.Serializable]
public class JogadorArray
{
    public Jogador[] jogadores;
}

[System.Serializable]
public class ItemArray
{
    public ItemJogador[] itens;
}