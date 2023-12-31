﻿using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Diagnostics.Metrics;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {

        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
               _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
           
            _context.Add(owner);
            return Save();
        }

        public bool DeleteOwner(int ownerId)
        {
             _context.Remove(GetOwner(ownerId));
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokemonId)
        {
            return _context.PokemonOwners.Where(p => p.PokemonId == pokemonId).Select(o => o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(p => p.OwnerId == ownerId).Select(o => o.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
             
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        public bool Save()
        {

            var saved = _context.SaveChanges();
            return _context.SaveChanges() >= 0 ? true : false;

        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }
    }
}
