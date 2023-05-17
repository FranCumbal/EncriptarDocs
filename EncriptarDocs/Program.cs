using System;
using System.Collections.Generic;
using System.IO;

public class LZWEncryptor
{
    public static void Main()
    {
        Console.WriteLine("Ingrese la ruta completa del archivo a encriptar: ");
        string filePath = Console.ReadLine();

        string encryptedFilePath = Path.ChangeExtension(filePath, ".enc");

        try
        {
            EncryptFile(filePath, encryptedFilePath);
            Console.WriteLine("Archivo encriptado exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al encriptar el archivo: " + ex.Message);
        }
    }

    public static void EncryptFile(string inputFilePath, string outputFilePath)
    {
        Dictionary<string, int> dictionary = new Dictionary<string, int>();

        // Inicializar el diccionario con caracteres ASCII
        for (int i = 0; i < 256; i++)
        {
            dictionary.Add(((char)i).ToString(), i);
        }

        List<int> encryptedData = new List<int>();

        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            string currentPhrase = string.Empty;
            string nextChar;

            while ((nextChar = reader.ReadLine()) != null)
            {
                string combinedPhrase = currentPhrase + nextChar;

                if (dictionary.ContainsKey(combinedPhrase))
                {
                    currentPhrase = combinedPhrase;
                }
                else
                {
                    encryptedData.Add(dictionary[currentPhrase]);

                    // Agregar la nueva frase al diccionario
                    dictionary.Add(combinedPhrase, dictionary.Count);

                    currentPhrase = nextChar;
                }
            }

            // Agregar el último código de la frase actual
            if (!string.IsNullOrEmpty(currentPhrase))
            {
                encryptedData.Add(dictionary[currentPhrase]);
            }
        }

        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            foreach (int code in encryptedData)
            {
                writer.Write(code.ToString() + " ");
            }
        }
    }
}
