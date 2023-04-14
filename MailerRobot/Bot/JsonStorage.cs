﻿using System.Text;
using MailerRobot.Bot.Domain.Interfaces;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace MailerRobot.Bot;

public class JsonStorage : IDataStorage
{
    private readonly object _lockObject = new();
    /*private readonly ILogger _logger;

    public JsonStorage(ILogger logger)
    {
        _logger = logger;
    }*/

    public void Save<T>(T data, string fileName)
    {
        //_logger.Verbose($"Saving data to file {fileName}");
        lock (_lockObject)
        {
            var jsonString = JsonConvert.SerializeObject(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
            File.WriteAllText(fileName, jsonString, Encoding.UTF8);
        }
    }

    /// <summary>
    /// Deserializes the JSON file
    /// </summary>
    /// <exception cref="JsonException">Is thrown if the file doesn't exist</exception>
    public void Load<T>(string fileName, out T data)
    {
        //_logger.Verbose($"Loading data from file {fileName}");
        lock (_lockObject)
        {
            var jsonString = File.ReadAllText(fileName);
            data = JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}