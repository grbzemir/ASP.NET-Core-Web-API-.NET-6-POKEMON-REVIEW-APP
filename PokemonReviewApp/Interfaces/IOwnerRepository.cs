﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {

        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);

        ICollection<Owner> GetOwnerOfAPokemon(int pokemonId);

        ICollection<Pokemon> GetPokemonsByOwner(int ownerId);

        bool OwnerExists(int ownerId);


    }
}