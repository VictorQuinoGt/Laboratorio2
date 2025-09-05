using Microsoft.EntityFrameworkCore;
using HelloApi.Models;    // Message
    // Person, Client, Product, Invoice, Detail, Item, Order, OrderDetail

namespace HelloApi.Data;   // Opcional: cámbialo a HelloApi.Data si prefieres

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Item> Items => Set<Item>(); 
    //public DbSet<Product> Products => Set<Product>();
    //public DbSet<Client> Clients => Set<Client>();
    //public DbSet<Invoice> Invoices => Set<Invoice>();
    //public DbSet<Detail> Details => Set<Detail>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ---- Message
        modelBuilder.Entity<Message>(e =>
        {
            e.ToTable("Messages");
            e.HasKey(x => x.Id);
            e.Property(x => x.MessageText).IsRequired().HasMaxLength(500);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

         // ---- Person
        modelBuilder.Entity<Person>(e =>
        {
            e.ToTable("Persons");
            e.HasKey(x => x.Id);
            e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            e.Property(x => x.Email).HasMaxLength(200);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            // e.HasIndex(x => x.Email).IsUnique(); // opcional
            e.HasMany(x => x.Orders)
             .WithOne(o => o.Person)
             .HasForeignKey(o => o.PersonId)
             .OnDelete(DeleteBehavior.Restrict);
        }); 

        // ---- Item
        modelBuilder.Entity<Item>(e =>
        {
            e.ToTable("Items");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Price).HasColumnType("decimal(18,2)");
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(x => x.Name);
            e.HasMany(x => x.OrderDetails)
             .WithOne(od => od.Item)
             .HasForeignKey(od => od.ItemId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Order
        modelBuilder.Entity<Order>(e =>
        {
            e.ToTable("Orders");
            e.HasKey(x => x.Id);
            e.Property(x => x.Number).IsRequired(); // si usas correlativo
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(x => x.Number); // e.HasIndex(x => x.Number).IsUnique(); // si lo quieres único
            e.HasOne(x => x.Person)
             .WithMany(p => p.Orders)
             .HasForeignKey(x => x.PersonId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.OrderDetails)
             .WithOne(od => od.Order)
             .HasForeignKey(od => od.OrderId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ---- OrderDetail
        modelBuilder.Entity<OrderDetail>(e =>
        {
            e.ToTable("OrderDetails");
            e.HasKey(x => x.Id);
            e.Property(x => x.Quantity).IsRequired();
            e.Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
            e.Property(x => x.Total).HasColumnType("decimal(18,2)").IsRequired();
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        /*// ---- Client
        modelBuilder.Entity<Client>(e =>
        {
            e.ToTable("Clients");
            e.HasKey(x => x.Id);
            e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            e.Property(x => x.Email).HasMaxLength(200);
            e.Property(x => x.Nit).IsRequired().HasMaxLength(50);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(x => x.Nit).IsUnique(); // común en facturación
            e.HasMany(x => x.Invoices)
             .WithOne(i => i.Client)
             .HasForeignKey(i => i.ClientId)
             .OnDelete(DeleteBehavior.Restrict);
        }); 

        // ---- Product
        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("Products");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(x => x.Name);
            e.HasMany(x => x.Details)
             .WithOne(d => d.Product)
             .HasForeignKey(d => d.ProductId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Invoice
        modelBuilder.Entity<Invoice>(e =>
        {
            e.ToTable("Invoices");
            e.HasKey(x => x.Id);
            e.Property(x => x.Number).IsRequired();      // quita si no existe
            
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(x => x.Number); // .IsUnique(); si aplica
            e.HasOne(x => x.Client)
             .WithMany(c => c.Invoices)
             .HasForeignKey(x => x.ClientId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.Details)
             .WithOne(d => d.Invoice)
             .HasForeignKey(d => d.InvoiceId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ---- Detail (líneas de factura)
        modelBuilder.Entity<Detail>(e =>
        {
            e.ToTable("Details");
            e.HasKey(x => x.Id);
            e.Property(x => x.Quantity).IsRequired();
            e.Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
            e.Property(x => x.Total).HasColumnType("decimal(18,2)").IsRequired();
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }); */
    }
}
