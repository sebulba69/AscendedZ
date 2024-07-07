using AscendedZ.currency.rewards;
using AscendedZ.entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace AscendedZ.game_object
{
    /// <summary>
    /// This is a singleton, global game progress tracker that can be accessed from any screen.
    /// </summary>
    public partial class PersistentGameObjects
    {

        /// <summary>
        /// Path for saving and loading.
        /// </summary>
        private readonly static string SAVE_CACHE_PATH = "user://save_cache.json";

        private static SaveObject _saveObject;

        private static GameObject _instance;

        private PersistentGameObjects() { }

        public static GameObject GameObjectInstance()
        {
            if (_instance == null)
            {
                _instance = new GameObject();
            }

            return _instance;
        }

        public static SaveObject SaveObjectInstance()
        {
            if(_saveObject == null)
            {
                _saveObject = new SaveObject();
                _saveObject.Initialize(SAVE_CACHE_PATH);
            }

            return _saveObject;
        }

        /// <summary>
        /// Create a new Game Object and save it.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="image"></param>
        public static void NewGame(string name, string image)
        {
            _instance = new GameObject 
            {
                MainPlayer = new MainPlayer 
                {
                    Name = name,
                    Image = image
                },
                Tier = 1,
                MaxTier = 1
            };

            var mainPlayer = _instance.MainPlayer;

            var vorpex = new Vorpex() { Amount = 0 };
            var partyCoin = new PartyCoin() { Amount = 1 };
            var dellencoin = new Dellencoin() { Amount = 10 };

            mainPlayer.Wallet.AddCurrency(vorpex);
            mainPlayer.Wallet.AddCurrency(partyCoin);
            mainPlayer.Wallet.AddCurrency(dellencoin);

            StringBuilder id = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
                id.Append(random.Next(0, 10));

            string savePath = $"user://{id.ToString()}_{_instance.MainPlayer.Name}.json";

            var saveObject = SaveObjectInstance();

            saveObject.SaveCache.Add(new SaveEntry()
            {
                Path = savePath,
                Name = _instance.MainPlayer.Name
            });

            saveObject.SavePathForCurrentGame = savePath;

            // save our updated cache
            JsonUtil.SaveObject(saveObject.SaveCache, SAVE_CACHE_PATH);

            // create a new instance for our game and save it
            Save();
        }

        /// <summary>
        /// Load our game object into our persistent object.
        /// </summary>
        /// <param name="entry"></param>
        public static void Load(SaveEntry entry)
        {
            SaveObjectInstance().SavePathForCurrentGame = entry.Path;
            _instance = JsonUtil.LoadObject<GameObject>(entry.Path);
        }

        public static void DeleteSaveAtIndex(int selectedIndex)
        {
            var saveObject = SaveObjectInstance();
            var saveCache = saveObject.SaveCache;

            var entry = saveCache[selectedIndex];
            saveCache.RemoveAt(selectedIndex);

            saveObject.SaveSaveCache(SAVE_CACHE_PATH);

            JsonUtil.DeleteFileAtPath(entry.Path);
        }

        /// <summary>
        /// Save our game object.
        /// </summary>
        public static void Save()
        {
            var instance = GameObjectInstance();
            var saveObject = SaveObjectInstance();

            JsonUtil.SaveObject<GameObject>(instance, saveObject.SavePathForCurrentGame);
        }
    }
}
