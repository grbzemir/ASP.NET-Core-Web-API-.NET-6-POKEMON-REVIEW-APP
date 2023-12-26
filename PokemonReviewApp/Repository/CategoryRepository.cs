﻿using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private DataContext _context;

        public CategoryRepository(DataContext context)
        {

            _context = context;

        }
        public bool CategoryExists(int id)
        {
               
            return _context.Categories.Any(c => c.Id == id);
        
        }

        public bool CreateCategory(Category category)
        {

            //Change Tracker
            //Add , updating , modifying , deleting
            //connected vs disconnected
            // EntityState.Added
            // EntityState.Modified
            //Emircan Gürbüz
            _context.Add(category);
            
            return Save();

        }

        public ICollection<Category> GetCategories()
        {
           
            return _context.Categories.ToList();
        }

        public Category GetCategory(int Id)
        {
            return _context.Categories.Where(e => e.Id == Id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories.Where(e => e.CategoryId == categoryId).Select(c => c.Pokemon).ToList();
       
        }

        public bool Save()
        {
            
            var saved = _context.SaveChanges();

            return saved >= 0 ? true : false;


        }
    }
}
