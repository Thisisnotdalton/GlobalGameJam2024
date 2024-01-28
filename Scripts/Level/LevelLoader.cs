using System;
using GlobalGameJam2024.Scripts.Character;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.Level
{
    public class LevelLoader : Node
    {
        [Export] private PackedScene _defaultLevel = null;
        [Export] private PackedScene _playerScene = null;
        private Node _loadedLevel;
        private Node _player;


        private void UnloadCurrentLevel()
        {
            if (_player != null)
            {
                _player.QueueFree();
            }

            _player = null;
            if (_loadedLevel != null)
            {
                _loadedLevel.QueueFree();
            }

            _loadedLevel = null;
        }

        private ISpawnPoint GetDefaultSpawnPoint(Node node)
        {
            ISpawnPoint spawnPoint = SearchNodeType.FindChildOfType<SpawnPoint>(node);

            if (spawnPoint == null)
            {
                throw new Exception($"No spawn point provided in level {node.Name}!");
            }

            return spawnPoint;
        }

        public void LoadLevel(PackedScene level)
        {
            UnloadCurrentLevel();
            LoadNextLevel(level);
        }
        
        private void LoadNextLevel(PackedScene level){
            _loadedLevel = level.Instance();
            AddChild(_loadedLevel);
            if (_playerScene != null)
            {
                _player = _playerScene.Instance();
                AddChild(_player);
                if (_player is PlatformerCharacter platformerCharacter)
                {
                    platformerCharacter.ResetToSpawnPoint(GetDefaultSpawnPoint(_loadedLevel));
                }
            }
        }

        public void LoadDefaultLevel()
        {
            if (_defaultLevel != null)
            {
                LoadLevel(_defaultLevel);
            }
        }
    }
}