﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {

        ICollection<Category> GetCategories();

        Category GetCategory(int Id);

        ICollection<Pokemon> GetPokemonByCategory(int categoryId);

        bool CategoryExists(int id);

        bool CreateCategory(Category category);

        bool Save();

    }
}
