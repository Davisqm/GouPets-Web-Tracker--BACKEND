using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using PetsApi.Models;

namespace PetsApi.Models
{
    public class SuppliersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<PetsApi.Models.Client> Client { get; set; }
        public DbSet<PetsApi.Models.Vaccine> Vaccine { get; set; }
        public DbSet<PetsApi.Models.Antiparasite> Antiparasite { get; set; }
        public DbSet<PetsApi.Models.InventorySim> InventorySim { get; set; }

        public DbSet<PetsApi.Models.DeviceTraking> DeviceTraking { get; set; }
        public DbSet<PetsApi.Models.Rate> Rate { get; set; }
        public DbSet<PetsApi.Models.Specie> Specie { get; set; }
        public DbSet<PetsApi.Models.GpsPet> GpsPet { get; set; }
        public DbSet<PetsApi.Models.BoxHistoryPet> BoxHistoryPet { get; set; }
        public DbSet<PetsApi.Models.TrakingAccess> TrakingAccess { get; set; }
        public DbSet<PetsApi.Models.GpsHistoryLocate> GpsHistoryLocate { get; set; }
        public DbSet<PetsApi.Models.Notification> Notification { get; set; }
        public DbSet<PetsApi.Models.AdvertisingBanner> AdvertisingBanner { get; set; }
        public DbSet<PetsApi.Models.Ticket> Ticket { get; set; }
        public DbSet<PetsApi.Models.TicketMessage> TicketMessage { get; set; }
     
        public SuppliersContext(DbContextOptions<SuppliersContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            


            TemporalSeeders(modelBuilder);
            
            
            // Codigo para relacionar de uno a uno (De user a Client)

            // modelBuilder.Entity<User>()
            // .HasOne(a => a.Client)
            // .WithOne(b => b.User)
            // .HasForeignKey<Client>(b => b.UserId);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseModel)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseModel)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private void TemporalSeeders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Infinity Systems - ADMIN", Password = "123", RoleId = 1, Active = true, Email = "admin@admin.com" }
            );

            //
             modelBuilder.Entity<Specie>().HasData(
                new Specie { Id = 1, Name = "Gato"},
                new Specie { Id = 2, Name = "Perro"}
            );

            //
             modelBuilder.Entity<Rate>().HasData(
                new Rate { Id = 1, SpecieId =2, Name = "Airedale Terrier"},
                new Rate { Id = 2, SpecieId =2, Name = "Akita"},
                new Rate { Id = 3, SpecieId =2, Name = "Alaskan Malamute"},
                new Rate { Id = 4, SpecieId =2, Name = "American Bulldog"},
                new Rate { Id = 5, SpecieId =2, Name = "American Bully (Standard)"},
                new Rate { Id = 6, SpecieId =2, Name = "American Eskimo Dog (Miniature)"},
                new Rate { Id = 7, SpecieId =2, Name = "American Eskimo Dog (Standard)"},
                new Rate { Id = 8, SpecieId =2, Name = "Am. Staffordshire Terrier"},
                new Rate { Id = 9, SpecieId =2, Name = "American Pit Bull Terrier"},
                new Rate { Id = 10, SpecieId =2, Name = "Australian Cattle Dog (Heeler)"},
                new Rate { Id = 11, SpecieId =2, Name = "Australian Kelpie"},
                new Rate { Id = 12, SpecieId =2, Name = "Australian Shepherd"},
                new Rate { Id = 13, SpecieId =2, Name = "Austrialian Terrier"},
                new Rate { Id = 14, SpecieId =2, Name = "Barbet"},
                new Rate { Id = 15, SpecieId =2, Name = "Basenji"},
                new Rate { Id = 16, SpecieId =2, Name = "Basset Hound"},
                new Rate { Id = 17, SpecieId =2, Name = "Beagle"},
                new Rate { Id = 18, SpecieId =2, Name = "Beauceron"},
                new Rate { Id = 19, SpecieId =2, Name = "Bedlington Terrier"},
                new Rate { Id = 20, SpecieId =2, Name = "Belgian Malinois"},
                new Rate { Id = 21, SpecieId =2, Name = "Belgian Tervuren"},
                new Rate { Id = 22, SpecieId =2, Name = "Bernese Mountain Dog"},
                new Rate { Id = 23, SpecieId =2, Name = "Bichon Frise"},
                new Rate { Id = 24, SpecieId =2, Name = "Black and Tan Coonhound"},
                new Rate { Id = 25, SpecieId =2, Name = "Bloodhound"},
                new Rate { Id = 26, SpecieId =2, Name = "Bluetick Coonhound"},
                new Rate { Id = 27, SpecieId =2, Name = "Boerboel"},
                new Rate { Id = 28, SpecieId =2, Name = "Border Collie"},
                new Rate { Id = 29, SpecieId =2, Name = "Border Terrier"},
                new Rate { Id = 30, SpecieId =2, Name = "Boston Terrier"},
                new Rate { Id = 31, SpecieId =2, Name = "Bouvier des Flandres"},
                new Rate { Id = 32, SpecieId =2, Name = "Boxer"},
                new Rate { Id = 33, SpecieId =2, Name = "Boykin Spaniel"},
                new Rate { Id = 34, SpecieId =2, Name = "Bracco Italiano"},
                new Rate { Id = 35, SpecieId =2, Name = "Briard"},
                new Rate { Id = 36, SpecieId =2, Name = "Brittany"},
                new Rate { Id = 37, SpecieId =2, Name = "Bull Terrier (Standard)"},
                new Rate { Id = 38, SpecieId =2, Name = "Bull Terrier (Miniature)"},
                new Rate { Id = 39, SpecieId =2, Name = "Bulldog"},
                new Rate { Id = 40, SpecieId =2, Name = "Bullmastiff"},
                new Rate { Id = 41, SpecieId =2, Name = "Cairn Terrier"},
                new Rate { Id = 42, SpecieId =2, Name = "Cane Corso"},
                new Rate { Id = 43, SpecieId =2, Name = "Cardigan Welsh Corgi"},
                new Rate { Id = 44, SpecieId =2, Name = "Catahoula Leopard Dog"},
                new Rate { Id = 45, SpecieId =2, Name = "Caucasian Shepherd (Ovcharka)"},
                new Rate { Id = 46, SpecieId =2, Name = "Cavalier King Charles Spaniel"},
                new Rate { Id = 47, SpecieId =2, Name = "Chesapeake Bay Retriever"},
                new Rate { Id = 48, SpecieId =2, Name = "Chihuahua (Long hair)"},
                new Rate { Id = 49, SpecieId =2, Name = "Chihuahua (Smooth)"},
                new Rate { Id = 50, SpecieId =2, Name = "Chinese Crested"},
                new Rate { Id = 51, SpecieId =2, Name = "Chinese Shar-Pei"},
                new Rate { Id = 52, SpecieId =2, Name = "Chinook"},
                new Rate { Id = 53, SpecieId =2, Name = "Chow Chow"},
                new Rate { Id = 54, SpecieId =2, Name = "Clumber Spaniel"},
                new Rate { Id = 55, SpecieId =2, Name = "Cocker Spaniel (American)"},
                new Rate { Id = 56, SpecieId =2, Name = "Cocker Spaniel (English)"},
                new Rate { Id = 57, SpecieId =2, Name = "Collie (Smooth)"},
                new Rate { Id = 58, SpecieId =2, Name = "Collie (Rough)"},
                new Rate { Id = 59, SpecieId =2, Name = "Coton De Tulear"},
                new Rate { Id = 60, SpecieId =2, Name = "Dachshund (smooth)"},
                new Rate { Id = 61, SpecieId =2, Name = "Dachshund (wire/long haired)"},
                new Rate { Id = 62, SpecieId =2, Name = "Dalmatian"},
                new Rate { Id = 63, SpecieId =2, Name = "Doberman Pinscher"},
                new Rate { Id = 64, SpecieId =2, Name = "Dogo Argentino"},
                new Rate { Id = 65, SpecieId =2, Name = "Dutch Shepherd"},
                new Rate { Id = 66, SpecieId =2, Name = "English Setter"},
                new Rate { Id = 67, SpecieId =2, Name = "English Shepherd"},
                new Rate { Id = 68, SpecieId =2, Name = "English Springer Spaniel"},
                new Rate { Id = 69, SpecieId =2, Name = "English Toy Spaniel"},
                new Rate { Id = 70, SpecieId =2, Name = "English Toy Terrier"},
                new Rate { Id = 71, SpecieId =2, Name = "Eurasier"},
                new Rate { Id = 72, SpecieId =2, Name = "Field Spaniel"},
                new Rate { Id = 73, SpecieId =2, Name = "Finnish Lapphund"},
                new Rate { Id = 74, SpecieId =2, Name = "Finnish Spitz"},
                new Rate { Id = 75, SpecieId =2, Name = "Flat Coat Retriever"},
                new Rate { Id = 76, SpecieId =2, Name = "French Bulldog"},
                new Rate { Id = 77, SpecieId =2, Name = "German Pinscher"},
                new Rate { Id = 78, SpecieId =2, Name = "German Shepherd Dog"},
                new Rate { Id = 79, SpecieId =2, Name = "German Shorthaired Pointer"},
                new Rate { Id = 80, SpecieId =2, Name = "Giant Schnauzer"},
                new Rate { Id = 81, SpecieId =2, Name = "Glen of Imaal Terrier"},
                new Rate { Id = 82, SpecieId =2, Name = "Golden Retriever"},
                new Rate { Id = 83, SpecieId =2, Name = "Gordon Setter"},
                new Rate { Id = 84, SpecieId =2, Name = "Great Dane"},
                new Rate { Id = 85, SpecieId =2, Name = "Great Pyrenees"},
                new Rate { Id = 86, SpecieId =2, Name = "Greyhound"},
                new Rate { Id = 87, SpecieId =2, Name = "Harrier"},
                new Rate { Id = 88, SpecieId =2, Name = "Havanese"},
                new Rate { Id = 89, SpecieId =2, Name = "Irish Setter"},
                new Rate { Id = 90, SpecieId =2, Name = "Irish Terrier"},
                new Rate { Id = 91, SpecieId =2, Name = "Irish Wolfhound"},
                new Rate { Id = 92, SpecieId =2, Name = "Italian Greyhound"},
                new Rate { Id = 93, SpecieId =2, Name = "Japanese Chin"},
                new Rate { Id = 94, SpecieId =2, Name = "Japanese Spitz"},
                new Rate { Id = 95, SpecieId =2, Name = "Keeshond"},
                new Rate { Id = 96, SpecieId =2, Name = "Komondor"},
                new Rate { Id = 97, SpecieId =2, Name = "Kooikerhondje"},
                new Rate { Id = 98, SpecieId =2, Name = "Kuvasz"},
                new Rate { Id = 99, SpecieId =2, Name = "Labrador Retriever"},
                new Rate { Id = 100, SpecieId =2, Name = "Lagotto Romagnolo"},
                new Rate { Id = 101, SpecieId =2, Name = "Lancashire Heeler"},
                new Rate { Id = 102, SpecieId =2, Name = "Leonberger"},
                new Rate { Id = 103, SpecieId =2, Name = "Lhasa Apso"},
                new Rate { Id = 104, SpecieId =2, Name = "Maltese"},
                new Rate { Id = 105, SpecieId =2, Name = "Miniature American Shepherd"},
                new Rate { Id = 106, SpecieId =2, Name = "Miniature Pinscher"},
                new Rate { Id = 107, SpecieId =2, Name = "Miniature Schnauzer"},
                new Rate { Id = 108, SpecieId =2, Name = "Newfoundland"},
                new Rate { Id = 109, SpecieId =2, Name = "Norfolk Terrier"},
                new Rate { Id = 110, SpecieId =2, Name = "Norwich Terrier"},
                new Rate { Id = 111, SpecieId =2, Name = "Nova Scotia Duck Tolling Retriever"},
                new Rate { Id = 112, SpecieId =2, Name = "Olde English Bulldogge"},
                new Rate { Id = 113, SpecieId =2, Name = "Old English Sheepdog"},
                new Rate { Id = 114, SpecieId =2, Name = "Papillon"},
                new Rate { Id = 115, SpecieId =2, Name = "Parson Russell Terrier"},
                new Rate { Id = 116, SpecieId =2, Name = "Patterdale Terrier (Smooth or Broken)"},
                new Rate { Id = 117, SpecieId =2, Name = "Patterdale Terrier (Rough)"},
                new Rate { Id = 118, SpecieId =2, Name = "Pekingese"},
                new Rate { Id = 119, SpecieId =2, Name = "Pembroke Welsh Corgi"},
                new Rate { Id = 120, SpecieId =2, Name = "Pharaoh Hound"},
                new Rate { Id = 121, SpecieId =2, Name = "Plott"},
                new Rate { Id = 122, SpecieId =2, Name = "Pointer (English)"},
                new Rate { Id = 123, SpecieId =2, Name = "Pomeranian"},
                new Rate { Id = 124, SpecieId =2, Name = "Poodle (Miniature)"},
                new Rate { Id = 125, SpecieId =2, Name = "Poodle (Standard)"},
                new Rate { Id = 126, SpecieId =2, Name = "Poodle (Toy)"},
                new Rate { Id = 127, SpecieId =2, Name = "Portugese Water Dog"},
                new Rate { Id = 128, SpecieId =2, Name = "Presa Canario"},
                new Rate { Id = 129, SpecieId =2, Name = "Pug"},
                new Rate { Id = 130, SpecieId =2, Name = "Puli"},
                new Rate { Id = 131, SpecieId =2, Name = "Pumi"},
                new Rate { Id = 132, SpecieId =2, Name = "Rat Terrier"},
                new Rate { Id = 133, SpecieId =2, Name = "Redbone Coonhound"},
                new Rate { Id = 134, SpecieId =2, Name = "Rhodesian Ridgeback"},
                new Rate { Id = 135, SpecieId =2, Name = "Rottweiler"},
                new Rate { Id = 136, SpecieId =2, Name = "Russian Toy"},
                new Rate { Id = 137, SpecieId =2, Name = "Saluki"},
                new Rate { Id = 138, SpecieId =2, Name = "Samoyed"},
                new Rate { Id = 139, SpecieId =2, Name = "Schipperke"},
                new Rate { Id = 140, SpecieId =2, Name = "Scottish Deerhound"},
                new Rate { Id = 141, SpecieId =2, Name = "Scottish Terrier"},
                new Rate { Id = 142, SpecieId =2, Name = "Shetland Sheepdog (Sheltie)"},
                new Rate { Id = 143, SpecieId =2, Name = "Shiba Inu"},
                new Rate { Id = 144, SpecieId =2, Name = "Shih Tzu"},
                new Rate { Id = 145, SpecieId =2, Name = "Shiloh Shepherd"},
                new Rate { Id = 146, SpecieId =2, Name = "Siberian Husky"},
                new Rate { Id = 147, SpecieId =2, Name = "Silky Terrier"},
                new Rate { Id = 148, SpecieId =2, Name = "Smooth Fox Terrier"},
                new Rate { Id = 149, SpecieId =2, Name = "Soft Coated Wheaten Terrier"},
                new Rate { Id = 150, SpecieId =2, Name = "Spanish Water Dog"},
                new Rate { Id = 151, SpecieId =2, Name = "Spinone Italiano"},
                new Rate { Id = 152, SpecieId =2, Name = "St. Bernard"},
                new Rate { Id = 153, SpecieId =2, Name = "Staffordshire Bull Terrier"},
                new Rate { Id = 154, SpecieId =2, Name = "Standard Schnauzer"},
                new Rate { Id = 155, SpecieId =2, Name = "Swedish Vallhund"},
                new Rate { Id = 156, SpecieId =2, Name = "Thai Ridgeback"},
                new Rate { Id = 157, SpecieId =2, Name = "Tibetan Mastiff"},
                new Rate { Id = 158, SpecieId =2, Name = "Tibetan Spaniel"},
                new Rate { Id = 159, SpecieId =2, Name = "Tibetan Terrier"},
                new Rate { Id = 160, SpecieId =2, Name = "Toy Fox Terrier"},
                new Rate { Id = 161, SpecieId =2, Name = "Treeing Walker Coonhound"},
                new Rate { Id = 162, SpecieId =2, Name = "Vizsla"},
                new Rate { Id = 163, SpecieId =2, Name = "Weimaraner"},
                new Rate { Id = 164, SpecieId =2, Name = "Welsh Springer Spaniel"},
                new Rate { Id = 165, SpecieId =2, Name = "West Highland White Terrier"},
                new Rate { Id = 166, SpecieId =2, Name = "Whippet"},
                new Rate { Id = 167, SpecieId =2, Name = "White Shepherd"},
                new Rate { Id = 168, SpecieId =2, Name = "Wire Fox Terrier"},
                new Rate { Id = 169, SpecieId =2, Name = "Wirehaired Pointing Griffon"},
                new Rate { Id = 170, SpecieId =2, Name = "Xoloitzcuintli"},
                new Rate { Id = 171, SpecieId =2, Name = "Yorkshire Terrier"},
                new Rate { Id = 172, SpecieId =1, Name = "Abyssinian"},
                new Rate { Id = 173, SpecieId =1, Name = "American Bobtail"},
                new Rate { Id = 174, SpecieId =1, Name = "American Curl"},
                new Rate { Id = 175, SpecieId =1, Name = "American Shorthair"},
                new Rate { Id = 176, SpecieId =1, Name = "American Wirehair"},
                new Rate { Id = 177, SpecieId =1, Name = "Balinese"},
                new Rate { Id = 178, SpecieId =1, Name = "Bengal"},
                new Rate { Id = 179, SpecieId =1, Name = "Birman"},
                new Rate { Id = 180, SpecieId =1, Name = "Bombay"},
                new Rate { Id = 181, SpecieId =1, Name = "British Shorthair"},
                new Rate { Id = 182, SpecieId =1, Name = "Burmese"},
                new Rate { Id = 183, SpecieId =1, Name = "Chartreux"},
                new Rate { Id = 184, SpecieId =1, Name = "Chausie"},
                new Rate { Id = 185, SpecieId =1, Name = "Cornish Rex"},
                new Rate { Id = 186, SpecieId =1, Name = "Devon Rex"},
                new Rate { Id = 187, SpecieId =1, Name = "Donskoy"},
                new Rate { Id = 188, SpecieId =1, Name = "Egyptian Mau"},
                new Rate { Id = 189, SpecieId =1, Name = "Exotic Shorthair"},
                new Rate { Id = 190, SpecieId =1, Name = "Havana"},
                new Rate { Id = 191, SpecieId =1, Name = "Highlander"},
                new Rate { Id = 192, SpecieId =1, Name = "Himalayan"},
                new Rate { Id = 193, SpecieId =1, Name = "Household Pet Cat"},
                new Rate { Id = 194, SpecieId =1, Name = "Household Pet Kitten"},
                new Rate { Id = 195, SpecieId =1, Name = "Japanese Bobtail"},
                new Rate { Id = 196, SpecieId =1, Name = "Korat"},
                new Rate { Id = 197, SpecieId =1, Name = "Kurilian Bobtail"},
                new Rate { Id = 198, SpecieId =1, Name = "LaPerm"},
                new Rate { Id = 199, SpecieId =1, Name = "Maine Coon"},
                new Rate { Id = 200, SpecieId =1, Name = "Manx"},
                new Rate { Id = 201, SpecieId =1, Name = "Minskin"},
                new Rate { Id = 202, SpecieId =1, Name = "Munchkin"},
                new Rate { Id = 203, SpecieId =1, Name = "Nebelung"},
                new Rate { Id = 204, SpecieId =1, Name = "Norwegian Forest Cat"},
                new Rate { Id = 205, SpecieId =1, Name = "Ocicat"},
                new Rate { Id = 206, SpecieId =1, Name = "Ojos Azules"},
                new Rate { Id = 207, SpecieId =1, Name = "Oriental"},
                new Rate { Id = 208, SpecieId =1, Name = "Persian"},
                new Rate { Id = 209, SpecieId =1, Name = "Peterbald"},
                new Rate { Id = 210, SpecieId =1, Name = "Pixiebob"},
                new Rate { Id = 211, SpecieId =1, Name = "Ragdoll"},
                new Rate { Id = 212, SpecieId =1, Name = "Russian Blue"},
                new Rate { Id = 213, SpecieId =1, Name = "Savannah"},
                new Rate { Id = 214, SpecieId =1, Name = "Scottish Fold"},
                new Rate { Id = 215, SpecieId =1, Name = "Selkirk Rex"},
                new Rate { Id = 216, SpecieId =1, Name = "Serengeti"},
                new Rate { Id = 217, SpecieId =1, Name = "Siberian"},
                new Rate { Id = 218, SpecieId =1, Name = "Siamese"},
                new Rate { Id = 219, SpecieId =1, Name = "Singapura"},
                new Rate { Id = 220, SpecieId =1, Name = "Snowshoe"},
                new Rate { Id = 221, SpecieId =1, Name = "Sokoke"},
                new Rate { Id = 222, SpecieId =1, Name = "Somali"},
                new Rate { Id = 223, SpecieId =1, Name = "Sphynx"},
                new Rate { Id = 224, SpecieId =1, Name = "Thai"},
                new Rate { Id = 225, SpecieId =1, Name = "Tonkinese"},
                new Rate { Id = 226, SpecieId =1, Name = "Toyger"},
                new Rate { Id = 227, SpecieId =1, Name = "Turkish Angora"},
                new Rate { Id = 228, SpecieId =1, Name = "Turkish Van"}
            );

            //
        }      

        public DbSet<PetsApi.Models.GeoZona> GeoZona { get; set; }

       

        



        

    }
}
