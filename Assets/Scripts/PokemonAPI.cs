using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PokemonType
{
    public TypeInfo type;
}

[System.Serializable]
public class TypeInfo
{
    public string name;
}

[System.Serializable]
public class Sprites
{
    public string front_default;
}

[System.Serializable]
public class Pokemon
{
    public string name;
    public int id;
    public int height;
    public int weight;
    public PokemonType[] types;
    public Sprites sprites;
}

public class PokemonAPI : MonoBehaviour
{
    public TMP_Text pokemonText;
    public Image pokemonImage;

    private void Start()
    {
        StartCoroutine(GetPokemonData("pikachu"));
    }

    IEnumerator GetPokemonData(string pokemonName)
    {
        string url = $"https://pokeapi.co/api/v2/pokemon/{pokemonName.ToLower()}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Pokemon pokemon = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);

                // Atualiza texto
                string tipo = pokemon.types.Length > 0 ? pokemon.types[0].type.name : "desconhecido";
                pokemonText.text = $"Pokémon: {pokemon.name} – Tipo: {tipo}";

                // Carrega sprite
                if (!string.IsNullOrEmpty(pokemon.sprites.front_default))
                {
                    StartCoroutine(LoadPokemonSprite(pokemon.sprites.front_default));
                }
            }
            else
            {
                Debug.LogError("Erro ao buscar Pokémon: " + request.error);
            }
        }
    }

    IEnumerator LoadPokemonSprite(string spriteUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(spriteUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                pokemonImage.sprite = sprite;
            }
            else
            {
                Debug.LogError("Erro ao carregar imagem: " + request.error);
            }
        }
    }
}
