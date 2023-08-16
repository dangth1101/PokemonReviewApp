using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext context;

        public ReviewRepository(DataContext context)
        {
            this.context = context;
        }

        public bool createReview(Review review)
        {
            context.Add(review);
            return save();
        }

        public bool deleteReview(Review review)
        {
            context.Remove(review);
            return save();
        }

        public bool exists(int id)
        {
            return context.reviews.Any(r => r.id == id);
        }

        public Review getReview(int id)
        {
            return context.reviews.Where(r => r.id == id).FirstOrDefault();
        }

        public ICollection<Review> getReviews()
        {
            return context.reviews.ToList();
        }

        public ICollection<Review> getReviewsOfAPokemon(int id)
        {
            return context.reviews.Where(r => r.pokemon.id == id).ToList();
        }

        public bool removeRange(List<Review> reviews)
        {
            context.RemoveRange(reviews);
            return save();
        }

        public bool save()
        {
            var save = context.SaveChanges() > 0;
            return save;
        }

        public bool updateReview(Review review)
        {
            context.Update(review);
            return save();
        }
    }
}

