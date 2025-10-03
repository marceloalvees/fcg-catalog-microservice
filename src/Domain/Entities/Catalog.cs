﻿using Domain._Common.Exceptions;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class Catalog : EntityBase
    {

        private List<Game> _games = new();

        protected Catalog() : base() { }

        public Catalog(
        Guid key,
        string name,
        string description
    ) : base(key)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public virtual IReadOnlyCollection<Game> Games
        {
            get => _games.AsReadOnly();
            set => _games = value?.ToList() ?? new List<Game>(); 
        }

        public void SetData(string name, string description)
        {
            Name = name;
            Description = description;

            WasUpdated();
        }

        public void AddGame(Game game)
        {
            if (_games.Any(g => g.Key == game.Key || g.Title == game.Title))
                throw new FCGDuplicateException(
                game.Key,
                nameof(Game),
                $"Game with key or title '{game.Key}' already exists in the catalog."
            );

            _games.Add(game);
        }

        public void SetGame(Game game)
        {
            var existingGame = _games.Find(g => g.Key == game.Key);

            if (existingGame is null)
                throw new FCGNotFoundException(
                    game.Key,
                    nameof(Game),
                    $"Game with key '{game.Key}' does not exist in the catalog."
                );

            existingGame.SetData(
                game.Title,
                game.Description
            );
        }

        public void RemoveGame(Game game)
        {
            var existingGame = _games.Find(g => g.Key == game.Key);

            if (existingGame is null)
                throw new FCGNotFoundException(
                    game.Key,
                    nameof(Game),
                    $"Game with key '{game.Key}' does not exist in the catalog."
                );

            _games.Remove(existingGame);
        }

        public Game GetGame(Guid key) => _games.FirstOrDefault(g => g.Key == key)
            ?? throw new FCGNotFoundException(
                key,
                nameof(Game),
                $"Game with key '{key}' does not exist in the catalog."
            );

        public void UpdateGames(List<Game> games)
        {
            _games = games;
            WasUpdated();
        }
    }
}
