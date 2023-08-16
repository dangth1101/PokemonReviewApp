using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> getReviews();
        Review getReview(int id);
        ICollection<Review> getReviewsOfAPokemon(int id);
        bool exists(int id);
        bool createReview(Review review);
        bool updateReview(Review review);
        bool deleteReview(Review review);
        bool removeRange(List<Review> reviews);
        bool save();
    }
}
