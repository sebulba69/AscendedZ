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

        private static GameObject _instance;

        private PersistentGameObjects() { }

        public static GameObject Instance()
        {
            if (_instance == null)
            {
                _instance = new GameObject();
                _instance.Initialize(SAVE_CACHE_PATH);
            }

            return _instance;
        }

        /// <summary>
        /// Load our game object into our persistent object.
        /// </summary>
        /// <param name="entry"></param>
        public static void Load(SaveEntry entry)
        {
            _instance = JsonUtil.LoadObject<GameObject>(entry.Path);
        }

        public static void DeleteSaveAtIndex(int selectedIndex)
        {
            var entry = _instance.SaveCache[selectedIndex];
            _instance.SaveCache.RemoveAt(selectedIndex);
            _instance.SaveSaveCache(SAVE_CACHE_PATH);

            JsonUtil.DeleteFileAtPath(entry.Path);
        }

        /// <summary>
        /// Save this object for the first time.
        /// </summary>
        public static void SaveNew()
        {
            StringBuilder id = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
                id.Append(random.Next(0, 10));

            string savePath = $"user://{id.ToString()}_{_instance.MainPlayer.Name}.json";

            SaveEntry saveEntry = new SaveEntry()
            {
                Path = savePath,
                Name = _instance.MainPlayer.Name
            };

            _instance.SaveCache.Add(saveEntry);
            _instance.SavePath = savePath;

            // save our updated cache
            JsonUtil.SaveObject(_instance.SaveCache, SAVE_CACHE_PATH);

            // save our game instance
            Save();
        }

        /// <summary>
        /// Save our game object.
        /// </summary>
        public static void Save()
        {
            JsonUtil.SaveObject(_instance, _instance.SavePath);
        }
    }
}
