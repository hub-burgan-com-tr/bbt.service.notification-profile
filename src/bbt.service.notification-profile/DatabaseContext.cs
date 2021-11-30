using Microsoft.EntityFrameworkCore;


public class DatabaseContext : DbContext
{
    public DbSet<Source> Sources { get; set; }
    public DbSet<Consumer> Consumers { get; set; }
    public string DbPath { get; private set; }
    public DatabaseContext()
    {
        DbPath = $"notification.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //options.UseSqlite($"Data Source={DbPath}");
        options.UseSqlServer(@"Data Source=localhost;Initial Catalog=Notification;User Id=SA;Password=@Qwert12;");
        options.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<Consumer>().OwnsOne(e => e.Phone);

        /* TODO: SQL Edge not supporting memory optimized tables
        builder.Entity<Consumer>(c =>
        {
            c.OwnsOne(e => e.Phone).IsMemoryOptimized();
            c.IsMemoryOptimized();
        });
        */


        builder.Entity<Consumer>()
           .Property<long>("$id")
           .ValueGeneratedOnAdd();

        builder.Entity<Consumer>()
            .HasKey(c => c.Id)
            .IsClustered(false);

        builder.Entity<Consumer>()
          .HasIndex("$id")
          .IsUnique()
          .IsClustered(true);



        builder.Entity<Source>().HasData(
             new Source
             {
                 Id = "[SAMPLE]Incoming-EFT",
                 Title = "Gelen EFT",
                 Topic = "http://localhost:8082/topics/cdc_eft/incoming_eft",
                 ApiKey = "a1b2c33d4e5f6g7h8i9jakblc",
                 Secret = "11561681-8ba5-4b46-bed0-905ae1769bc6",
                 PushServiceReference = "notify_push_incoming_eft",
                 SmsServiceReference = "notify_sms_incoming_eft",
                 EmailServiceReference = "notify_email_incoming_eft"
             },
               new Source
               {
                   Id = "[SAMPLE]Incoming-FAST",
                   Title = "Gelen Fast",
                   Topic = "http://localhost:8082/topics/cdc_eft/incoming_fast",
                   ApiKey = "a1b2c33d4e5f6g7h8i9jakblc",
                   Secret = "11561681-8ba5-4b46-bed0-905ae1769bc6",
                   PushServiceReference = "notify_push_incoming_fast",
                   SmsServiceReference = "notify_sms_incoming_fast",
                   EmailServiceReference = "notify_email_incoming_fast"
               },
               new Source
               {
                   Id = "[SAMPLE]Incoming-QR",
                   Title = "Gelen EFT",
                   Topic = "http://localhost:8082/topics/cdc_eft/incoming_qr",
                   ApiKey = "a1b2c33d4e5f6g7h8i9jakblc",
                   Secret = "11561681-8ba5-4b46-bed0-905ae1769bc6",
                   PushServiceReference = "notify_push_incoming_qr",
                   SmsServiceReference = "notify_sms_incoming_qr",
                   EmailServiceReference = "notify_email_incoming_qr"
               }
             );

        builder.Entity<Consumer>(c =>
        {
            c.HasData(
            new
            {
                Id = new Guid("1e15d57c-26e3-4e78-94f9-8649b3302555"),
                Client = (long)123456,
                User = (long)123456,
                SourceId = "[SAMPLE]Incoming-EFT",
                Filter = "Message.data.amount >= 500 && Message.data.iban ==\"TR1234567\"",
                IsPushEnabled = false,
                IsSmsEnabled = true,
                IsEmailEnabled = false
            });
            c.OwnsOne(e => e.Phone).HasData(new { ConsumerId = new Guid("1e15d57c-26e3-4e78-94f9-8649b3302555"), CountryCode = 90, Prefix = 530, Number = 3855206 });

        });

        builder.Entity<Consumer>(c =>
        {
           c.HasData(
           new
           {
               Id = new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"),
               Client = (long)123456,
               User = (long)123456,
               SourceId = "[SAMPLE]Incoming-EFT",
               IsPushEnabled = false,
               IsSmsEnabled = true,
               IsEmailEnabled = false
           });
           c.OwnsOne(e => e.Phone).HasData(new { ConsumerId = new Guid("2e15d57c-26e3-4e78-94f9-8649b3302555"), CountryCode = 90, Prefix = 530, Number = 3855206 });
        });

        builder.Entity<Consumer>(c =>
       {
           c.HasData(
           new
           {
               Id = new Guid("3e15d57c-26e3-4e78-94f9-8649b3302555"),
               Client = (long)0,
               User = (long)123456,
               SourceId = "[SAMPLE]Incoming-EFT",
               Filter = "Message.data.amount >= 500000",
               IsPushEnabled = false,
               IsSmsEnabled = true,
               IsEmailEnabled = false
           });
           c.OwnsOne(e => e.Phone).HasData(new { ConsumerId = new Guid("3e15d57c-26e3-4e78-94f9-8649b3302555"), CountryCode = 90, Prefix = 530, Number = 3855206 });
       });


    }
}