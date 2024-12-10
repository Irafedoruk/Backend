using BackendShop.Core.Interfaces;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Bogus;
using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BackendShop.Data.DataSeeder
{
    public static class DataSeederExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
                var imageHulk = scope.ServiceProvider.GetRequiredService<IImageHulk>();
                dbContext.Database.Migrate();

                // Seed Categories
                if (!dbContext.Categories.Any())
                {
                    int number = 5;
                    var faker = new Faker("uk");
                    var categories = new List<Category>();

                    for (int i = 0; i < number; i++)
                    {
                        string image = await imageHulk.Save("https://picsum.photos/1200/800?category");
                        categories.Add(new Category
                        {
                            Name = faker.Commerce.Categories(1).First(),
                            Description = faker.Commerce.ProductDescription(),
                            ImageCategoryPath = image
                        });
                    }

                    dbContext.Categories.AddRange(categories);
                    dbContext.SaveChanges();
                }

                // Seed SubCategories
                if (!dbContext.SubCategories.Any())
                {
                    var categories = dbContext.Categories.ToList();
                    if (!categories.Any()) return; // Без категорій підкатегорії неможливо створити

                    var faker = new Faker("uk");
                    var subCategories = new List<SubCategory>();

                    for (int i = 0; i < 5; i++)
                    {
                        string image = await imageHulk.Save("https://picsum.photos/1200/800?subcategory");
                        var randomCategory = faker.PickRandom(categories);

                        subCategories.Add(new SubCategory
                        {
                            Name = faker.Commerce.Categories(1).First(),
                            Description = faker.Commerce.ProductDescription(),
                            ImageSubCategoryPath = image,
                            CategoryId = randomCategory.CategoryId
                        });
                    }

                    dbContext.SubCategories.AddRange(subCategories);
                    dbContext.SaveChanges();
                }
                if (!dbContext.Products.Any())
                {
                    var subcategories = dbContext.SubCategories.ToList();
                    if (!subcategories.Any()) return; // Переконайтеся, що підкатегорії існують

                    var faker = new Faker("uk");
                    var products = new List<Product>();

                    for (int i = 0; i < 5; i++)
                    {
                        var subCategory = faker.PickRandom(subcategories);

                        var product = new Product
                        {
                            Code = faker.Commerce.Ean13(),
                            Name = faker.Commerce.ProductName(),
                            Description = faker.Lorem.Paragraph(),
                            Manufacturer = faker.Company.CompanyName(),
                            Size = faker.Commerce.ProductAdjective(),
                            Color = faker.Commerce.Color(),
                            Type = faker.Commerce.Department(),
                            Form = faker.Commerce.ProductMaterial(),
                            Price = decimal.Parse(faker.Commerce.Price()),
                            QuantityInPack = faker.Random.Int(1, 10),
                            QuantityInStock = faker.Random.Int(0, 100),
                            Model = faker.Commerce.ProductMaterial(),
                            SubCategoryId = subCategory.SubCategoryId // Зв'язок з підкатегорією
                        };

                        products.Add(product);
                    }

                    dbContext.Products.AddRange(products);
                    dbContext.SaveChanges();

                    // Додаємо зображення для кожного продукту
                    foreach (var product in products)
                    {
                        var images = new List<ProductImageEntity>();
                        for (int j = 0; j < 3; j++)
                        {
                            string imageUrl = $"https://picsum.photos/200/300?random={Guid.NewGuid()}";
                            images.Add(new ProductImageEntity
                            {
                                ProductId = product.ProductId,
                                Image = await imageHulk.Save(imageUrl),
                                Priority = j + 1
                            });
                        }

                        dbContext.ProductImageEntity.AddRange(images);

                        // Додаємо описові зображення (ImagesDescIds)
                        for (int j = 0; j < 2; j++)
                        {
                            string descImageUrl = $"https://picsum.photos/400/300?random={Guid.NewGuid()}";
                            dbContext.ProductDescImages.Add(new ProductDescImageEntity
                            {
                                ProductId = product.ProductId,
                                Image = await imageHulk.Save(descImageUrl)
                            });
                        }
                    }

                    dbContext.SaveChanges();
                }

                //using (var scope = app.ApplicationServices
                //    .GetRequiredService<IServiceScopeFactory>().CreateScope())
                //{
                //    var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
                //    var imageHulk = scope.ServiceProvider.GetRequiredService<IImageHulk>();
                //    //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
                //    //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
                //    //dbContext.Database.EnsureDeleted();
                //    dbContext.Database.Migrate();

                //    if (dbContext.Categories.Count() == 0)
                //    {
                //        int number = 10;
                //        var list = new Faker("uk")
                //            .Commerce.Categories(number);
                //        foreach (var name in list)
                //        {
                //            string image = imageHulk.Save("https://picsum.photos/1200/800?category").Result;
                //            var cat = new Category
                //            {
                //                Name = name,
                //                Description = new Faker("uk").Commerce.ProductDescription(),
                //                ImageCategoryPath = image
                //            };
                //            dbContext.Categories.Add(cat);
                //            dbContext.SaveChanges();
                //        }
                //    }

                //    if (dbContext.SubCategories.Count() == 0)
                //    {
                //        var categories = dbContext.Categories.ToList(); // Отримуємо список існуючих категорій
                //        int number = 10;
                //        var list = new Faker("uk").Commerce.Categories(number);

                //        foreach (var name in list)
                //        {
                //            // Перевіряємо, чи є хоча б одна категорія
                //            if (categories.Any())
                //            {
                //                string image = imageHulk.Save("https://picsum.photos/1200/800?subcategory").Result;

                //                // Випадковий зв'язок із категорією
                //                var randomCategory = new Faker().PickRandom(categories);

                //                var subCategory = new SubCategory
                //                {
                //                    Name = name,
                //                    Description = new Faker("uk").Commerce.ProductDescription(),
                //                    ImageSubCategoryPath = image,
                //                    CategoryId = randomCategory.CategoryId // Зв'язок із категорією
                //                };

                //                dbContext.SubCategories.Add(subCategory);
                //                dbContext.SaveChanges();
                //            }
                //        }
                //    }


                //    if (dbContext.Products.Count() == 0)
                //    {
                //        var subcategories = dbContext.SubCategories.ToList();

                //        var fakerProduct = new Faker<Product>("uk")
                //            .RuleFor(u => u.Name, (f, u) => f.Commerce.Product())
                //            .RuleFor(u => u.Price, (f, u) => decimal.Parse(f.Commerce.Price()))
                //            .RuleFor(u => u.SubCategoryId, (f, u) => f.PickRandom(subcategories).SubCategoryId);

                //        string url = "https://picsum.photos/1200/800?product";

                //        var products = fakerProduct.GenerateLazy(32);
                //        Random r = new Random();

                //        foreach (var product in products)
                //        {
                //            dbContext.Add(product);
                //            dbContext.SaveChanges();
                //            int imageCount = r.Next(3, 5);
                //            for (int i = 0; i < imageCount; i++)
                //            {
                //                var imageName = imageHulk.Save(url).Result;
                //                var imageProduct = new ProductImageEntity
                //                {
                //                    Product = product,
                //                    Image = imageName,
                //                    Priority = i
                //                };
                //                dbContext.Add(imageProduct);
                //                dbContext.SaveChanges();
                //            }
                //        }
                //    }

                // seed roles
                //if (dbContext.Roles.Count() == 0)
                //{
                //    var roles = new[]
                //    {
                //        new RoleEntity { Name = Roles.Admin },
                //        new RoleEntity { Name = Roles.User }
                //    };

                //    foreach (var role in roles)
                //    {
                //        var outcome = roleManager.CreateAsync(role).Result;
                //        if (!outcome.Succeeded) Console.WriteLine($"Failed to create role: {role.Name}");
                //    }
                //}

                // seed users
                //if (dbContext.Users.Count() == 0)
                //{
                //    var users = new[]
                //    {
                //        new { User = new UserEntity { Firstname = "Tony", Lastname = "Stark", UserName = "admin@gmail.com", Email = "admin@gmail.com" }, Password = "admin1", Role = Roles.Admin },
                //        new { User = new UserEntity { Firstname = "Boba", Lastname = "Gray", UserName = "user@gmail.com", Email = "user@gmail.com" }, Password = "bobapass1", Role = Roles.User },
                //        new { User = new UserEntity { Firstname = "Biba", Lastname = "Undefined", UserName = "biba@gmail.com", Email = "biba@gmail.com" }, Password = "bibapass3", Role = Roles.User }
                //    };

                //    foreach (var i in users)
                //    {
                //        var outcome = userManager.CreateAsync(i.User, i.Password).Result;

                //        if (!outcome.Succeeded)
                //            Console.WriteLine($"Failed to create user: {i.User.UserName}");
                //        else
                //            outcome = userManager.AddToRoleAsync(i.User, i.Role).Result;
                //    }
                //}
            }
        }
    }
}
