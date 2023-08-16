using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext context;

        public CategoryRepository(DataContext context) {
            this.context = context;
        }

        public bool createCategory(Category category)
        {
            context.Add(category);
            return save();
        }

        public bool deleteCategory(Category category)
        {
            context.Remove(category);
            return save();
        }

        public bool exists(int id)
        {
            return context.categories.Any(c => c.id == id);
        }

        public ICollection<Category> getCategories()
        {
            return context.categories.ToList();
        }

        public Category getCategory(int id)
        {
            return context.categories.Where(c => c.id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> getPokemonByCategory(int id)
        {
            return context.PokemonCategories.Where(pc => pc.categoryID == id).Select(pc => pc.pokemon).ToList();
        }

        public bool save()
        {
            var save = context.SaveChanges() > 0;
            return save;
        }

        public bool updateCategory(Category category)
        {
            context.Update(category);
            return save();
        }
    }
}
