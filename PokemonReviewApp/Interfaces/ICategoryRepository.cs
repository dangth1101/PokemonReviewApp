using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> getCategories();
        Category getCategory(int id);
        ICollection<Pokemon> getPokemonByCategory(int id);

        bool exists(int id);
        bool createCategory(Category category);
        bool updateCategory(Category category);
        bool deleteCategory(Category category);
        bool save();
    }
}
