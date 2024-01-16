using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Model.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext() { }
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }
        public DbSet<Product> Product { get; set; }
        //private readonly string _connectionString;
        //public MySqlContext(DbContextOptions<MySqlContext> options)
        //: base(options)
        //{
        //}
        ////public MySqlContext() {}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql("Server=localhost;Database=geek_shopping_product_api;Uid=root;Pwd=Admin123;",
        //        new MySqlServerVersion(new Version(8, 0, 3)));
        //}
    }
}