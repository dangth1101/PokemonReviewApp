using PokemonReviewApp.Data;
using PokemonReviewApp.Models;

namespace PokemonReviewApp
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.PokemonOwners.Any())
            {
                var pokemonowners = new List<PokemonOwner>()
                {
                    new PokemonOwner()
                    {
                        pokemon = new Pokemon()
                        {
                            name = "Pikachu",
                            birthDay = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { category = new Category() { name = "Electric"}}
                            },
                            reviews = new List<Review>()
                            {
                                new Review { title="Pikachu",text = "Pickahu is the best pokemon, because it is electric", rating = 5,
                                reviewer = new Reviewer(){ firstName = "Teddy", lastName = "Smith" } },
                                new Review { title="Pikachu", text = "Pickachu is the best a killing rocks", rating = 5,
                                reviewer = new Reviewer(){ firstName = "Taylor", lastName = "Jones" } },
                                new Review { title="Pikachu",text = "Pickchu, pickachu, pikachu", rating = 1,
                                reviewer = new Reviewer(){ firstName = "Jessica", lastName = "McGregor" } },
                            }
                        },
                        owner = new Owner()
                        {
                            firstName = "Jack",
                            lastName = "London",
                            gym = "Brocks gym",
                            country = new Country()
                            {
                                name = "Kanto"
                            }
                        }
                    },
                    new PokemonOwner()
                    {
                        pokemon = new Pokemon()
                        {
                            name = "Squirtle",
                            birthDay = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { category = new Category() { name = "Water"}}
                            },
                            reviews = new List<Review>()
                            {
                                new Review { title= "Squirtle", text = "squirtle is the best pokemon, because it is electric", rating = 5,
                                reviewer = new Reviewer(){ firstName = "Teddy", lastName = "Smith" } },
                                new Review { title= "Squirtle",text = "Squirtle is the best a killing rocks", rating = 5,
                                reviewer = new Reviewer(){ firstName = "Taylor", lastName = "Jones" } },
                                new Review { title= "Squirtle", text = "squirtle, squirtle, squirtle", rating = 1,
                                reviewer = new Reviewer(){ firstName = "Jessica", lastName = "McGregor" } },
                            }
                        },
                        owner = new Owner()
                        {
                            firstName = "Harry",
                            lastName = "Potter",
                            gym = "Mistys gym",
                            country = new Country()
                            {
                                name = "Saffron City"
                            }
                        }
                    },
                                    new PokemonOwner()
                    {
                        pokemon = new Pokemon()
                        {
                            name = "Venasuar",
                            birthDay = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { category = new Category() { name = "Leaf"}}
                            },
                            reviews = new List<Review>()
                            {
                                new Review { title="Veasaur",text = "Venasuar is the best pokemon, because it is electric", rating = 5,
                                reviewer = new Reviewer(){ firstName = "Teddy", lastName = "Smith" } },
                                new Review { title="Veasaur",text = "Venasuar is the best a killing rocks", rating = 5,
                                reviewer = new Reviewer(){ firstName = "Taylor", lastName = "Jones" } },
                                new Review { title="Veasaur",text = "Venasuar, Venasuar, Venasuar", rating = 1,
                                reviewer = new Reviewer(){ firstName = "Jessica", lastName = "McGregor" } },
                            }
                        },
                        owner = new Owner()
                        {
                            firstName = "Ash",
                            lastName = "Ketchum",
                            gym = "Ashs gym",
                            country = new Country()
                            {
                                name = "Millet Town"
                            }
                        }
                    }
                };
                dataContext.PokemonOwners.AddRange(pokemonowners);
                dataContext.SaveChanges();
            }
        }
    }
}
