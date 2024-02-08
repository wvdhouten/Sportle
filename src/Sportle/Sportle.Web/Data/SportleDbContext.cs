using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Data
{
    public class SportleDbContext : IdentityDbContext
    {
        public SportleDbContext(DbContextOptions<SportleDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Season>().Navigation(s => s.Drivers).AutoInclude();
            
            builder.Entity<Season>().Navigation(s => s.Events).AutoInclude();
            builder.Entity<Event>().Navigation(s => s.Venue).AutoInclude();
            builder.Entity<Event>().Navigation(s => s.Sessions).AutoInclude();
        }

        public DbSet<Season> Seasons { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventPrediction2024> Predictions2024 { get; set; }

        public DbSet<EventResult2024> Results2024 { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<Venue> Venues { get; set; }

        public void Repair(bool seed = false)
        {
            Database.EnsureCreated();
            Database.Migrate();

            if (seed)
            {
                SeedSeasons();
                SeedVenues();
                SeedDrivers();

                SeedSeasonDrivers();
                SeedEvents();

                SaveChanges();
            }
        }

        private void SeedSeasons()
        {
            if (Seasons.Any())
                return;

            Seasons.Add(new Season { Year = 2024 });
        }

        private void SeedVenues()
        {
            if (Venues.Any())
                return;

            Venues.Add(new Venue { Name = "Sakhir", Country = "Bahrain" });
            Venues.Add(new Venue { Name = "Jeddah", Country = "Saudi Arabia" });
            Venues.Add(new Venue { Name = "Albert Park, Melbourne", Country = "Australia" });
            Venues.Add(new Venue { Name = "Sazuka", Country = "Japan" });
            Venues.Add(new Venue { Name = "Shanghai", Country = "China" });
            Venues.Add(new Venue { Name = "Miami", Country = "USA" });
            Venues.Add(new Venue { Name = "Imola", Country = "Italy" });
            Venues.Add(new Venue { Name = "Monaco", Country = "Monaco" });
            Venues.Add(new Venue { Name = "Gilles Villeneuve, Montreal", Country = "Canada" });
            Venues.Add(new Venue { Name = "Barcelona", Country = "Spain" });
            Venues.Add(new Venue { Name = "Red Bull Ring", Country = "Austria" });
            Venues.Add(new Venue { Name = "Silverstone", Country = "United Kingdom" });
            Venues.Add(new Venue { Name = "Hungaroring", Country = "Hungary" });
            Venues.Add(new Venue { Name = "Spa-Francorchamps", Country = "Belgium" });
            Venues.Add(new Venue { Name = "Zandvoort", Country = "Netherlands" });
            Venues.Add(new Venue { Name = "Monza", Country = "Italy" });
            Venues.Add(new Venue { Name = "Baku", Country = "Azerbaijan" });
            Venues.Add(new Venue { Name = "Marina Bay", Country = "Singapore" });
            Venues.Add(new Venue { Name = "Circuit of the Americas", Country = "USA" });
            Venues.Add(new Venue { Name = "Hermanos Rodriguez", Country = "Mexico" });
            Venues.Add(new Venue { Name = "Interlagos, São Paulo", Country = "Brazil" });
            Venues.Add(new Venue { Name = "Las Vegas", Country = "USA" });
            Venues.Add(new Venue { Name = "Lusail", Country = "Qatar" });
            Venues.Add(new Venue { Name = "Yas Marina", Country = "Abu Dhabi" });
        }

        private void SeedDrivers()
        {
            if (Drivers.Any())
                return;

            Drivers.Add(new Driver { Name = "Max Verstappen", Country = "Netherlands", Number = 1, Team = "Red Bull Racing" });
            Drivers.Add(new Driver { Name = "Sergio Pérez", Country = "Mexico", Number = 11, Team = "Red Bull Racing" });
            Drivers.Add(new Driver { Name = "Lewis Hamilton", Country = "United Kingdom", Number = 44, Team = "Mercedes" });
            Drivers.Add(new Driver { Name = "George Russell", Country = "United Kingdom", Number = 63, Team = "Mercedes" });
            Drivers.Add(new Driver { Name = "Charles Leclerc", Country = "Monaco", Number = 16, Team = "Ferrari" });
            Drivers.Add(new Driver { Name = "Carlos Sainz Jr.", Country = "Spain", Number = 55, Team = "Ferrari" });
            Drivers.Add(new Driver { Name = "Lando Norris", Country = "United Kingdom", Number = 4, Team = "McLaren" });
            Drivers.Add(new Driver { Name = "Oscar Piastri", Country = "New Zealand", Number = 81, Team = "McLaren" });
            Drivers.Add(new Driver { Name = "Fernando Alonso", Country = "Spain", Number = 14, Team = "Aston Martin" });
            Drivers.Add(new Driver { Name = "Lance Stroll", Country = "Canada", Number = 18, Team = "Aston Martin" });
            Drivers.Add(new Driver { Name = "Pierre Gasly", Country = "France", Number = 10, Team = "Alpine" });
            Drivers.Add(new Driver { Name = "Esteban Ocon", Country = "France", Number = 31, Team = "Alpine" });
            Drivers.Add(new Driver { Name = "Alexander Albon", Country = "Thailand", Number = 23, Team = "Williams" });
            Drivers.Add(new Driver { Name = "Logan Sargeant", Country = "USA", Number = 2, Team = "Williams" });
            Drivers.Add(new Driver { Name = "Kevin Magnussen", Country = "Denmark", Number = 20, Team = "Haas" });
            Drivers.Add(new Driver { Name = "Nico Hülkenberg", Country = "Germany", Number = 27, Team = "Haas" });
            Drivers.Add(new Driver { Name = "Valtteri Bottas", Country = "Finland", Number = 77, Team = "Sauber" });
            Drivers.Add(new Driver { Name = "Zhou Guanyu", Country = "China", Number = 24, Team = "Sauber" });
            Drivers.Add(new Driver { Name = "Daniel Ricciardo", Country = "Australia", Number = 3, Team = "VCARB" });
            Drivers.Add(new Driver { Name = "Yuki Tsunoda", Country = "Japan", Number = 22, Team = "VCARB" });
        }

        private void SeedSeasonDrivers()
        {
            var season = Seasons.FirstOrDefault(s => s.Year == 2024);

            if (season == null || season.Drivers.Count > 0)
                return;

            foreach (var driver in Drivers)
            {
                season.Drivers.Add(driver);
            }
        }

        private void SeedEvents()
        {
            var season = Seasons.FirstOrDefault(s => s.Year == 2024);

            if (season == null || season.Events.Count > 0)
                return;

            foreach (var venue in Venues)
            {
                var @event = new Event
                {
                    Venue = venue
                };

                switch (venue.Name)
                {
                    case "Sakhir":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 02, 29, 14, 30, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 02, 29, 18, 00, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 03, 01, 15, 30, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 03, 01, 19, 00, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 03, 02, 18, 00, +3) });
                        break;
                    case "Jeddah":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 03, 07, 16, 30, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 03, 07, 20, 00, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 03, 08, 16, 30, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 03, 08, 20, 00, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 03, 09, 20, 00, +3) });
                        break;
                    case "Albert Park, Melbourne":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 03, 22, 12, 30, +11) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 03, 22, 16, 00, +11) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 03, 23, 12, 30, +11) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 03, 23, 16, 00, +11) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 03, 24, 15, 00, +11) });
                        break;
                    case "Sazuka":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 04, 05, 11, 30, +8) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 04, 05, 15, 00, +8) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 04, 06, 11, 30, +8) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 04, 06, 15, 00, +8) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 04, 07, 14, 00, +8) });
                        break;
                    case "Shanghai":
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 03, 07, 16, 30, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 03, 07, 20, 00, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 03, 08, 16, 30, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 03, 08, 20, 00, 00).WithOffset(-3) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 04, 21, 10, 00, +7) });
                        break;
                    case "Miami":
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 03, 07, 16, 30, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 03, 07, 20, 00, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 03, 08, 16, 30, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 03, 08, 20, 00, 00).WithOffset(-3) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 05, 05, 23, 00, -5) });
                        break;
                    case "Imola":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 05, 17, 13, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 05, 17, 17, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 05, 18, 12, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 05, 18, 16, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 05, 19, 15, 00, +1) });
                        break;
                    case "Monaco":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 05, 24, 14, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 05, 24, 19, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 05, 25, 14, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 05, 25, 17, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 05, 26, 16, 00, +1) });
                        break;
                    case "Gilles Villeneuve, Montreal":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 06, 07, 13, 30, -5) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 06, 07, 17, 00, -5) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 06, 08, 12, 30, -5) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 06, 08, 16, 00, -5) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 06, 09, 14, 00, -5) });
                        break;
                    case "Barcelona":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 06, 21, 13, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 06, 21, 17, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 06, 22, 12, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 06, 22, 16, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 06, 23, 15, 00, +1) });
                        break;
                    case "Red Bull Ring":
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 03, 07, 16, 30, 00).WithOffset(+1) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 03, 07, 20, 00, 00).WithOffset(+1) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 03, 08, 16, 30, 00).WithOffset(+1) });
                        //@event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 03, 08, 20, 00, 00).WithOffset(+1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 06, 30, 16, 00, +1) });
                        break;
                    case "Silverstone":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 07, 05, 12, 30, +0) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 07, 05, 16, 00, +0) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 07, 06, 11, 30, +0) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 07, 06, 15, 00, +0) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 07, 07, 15, 00, +0) });
                        break;
                    case "Hungaroring":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 07, 19, 13, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 07, 19, 17, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 07, 20, 12, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 07, 20, 16, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 07, 21, 15, 00, +1) });
                        break;
                    case "Spa-Francorchamps":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 07, 26, 13, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 07, 26, 17, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 07, 27, 12, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 07, 27, 16, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 07, 28, 15, 00, +1) });
                        break;
                    case "Zandvoort":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 08, 23, 12, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 08, 23, 16, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 08, 24, 11, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 08, 24, 15, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = new DateTime(2024, 08, 25, 15, 00, +1) });
                        break;
                    case "Monza":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 08, 30, 13, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 08, 30, 17, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 08, 31, 12, 30, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 08, 31, 16, 00, +1) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = new DateTime(2024, 09, 01, 15, 00, +1) });
                        break;
                    case "Baku":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 09, 13, 13, 30, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 09, 13, 17, 00, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 09, 14, 12, 30, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 09, 14, 16, 00, +3) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = new DateTime(2024, 09, 15, 15, 00, +3) });
                        break;
                    case "Marina Bay":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 09, 20, 17, 30, -7) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 09, 20, 21, 00, -7) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 09, 21, 17, 30, -7) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 09, 21, 21, 00, -7) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = new DateTime(2024, 09, 22, 20, 00, -7) });
                        break;
                    case "Circuit of the Americas":
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 03, 07, 17, 30, 00).WithOffset(-7) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 03, 07, 21, 00, 00).WithOffset(-7) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 03, 08, 17, 30, 00).WithOffset(-7) });
                        //@event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 03, 08, 21, 00, 00).WithOffset(-7) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = new DateTime(2024, 10, 20, 14, 00, +6) });
                        break;
                    case "Hermanos Rodriguez":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 10, 25, 12, 30, +6) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 10, 25, 16, 00, +6) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 10, 26, 11, 30, +6) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 10, 26, 15, 00, +6) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = new DateTime(2024, 10, 27, 14, 00, +6) });
                        break;
                    case "Interlagos, São Paulo":
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 03, 07, 16, 30, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 03, 07, 20, 00, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 03, 08, 16, 30, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 03, 08, 20, 00, 00).WithOffset(-3) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 11, 03, 14, 00, +3) });
                        break;
                    case "Las Vegas":
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 03, 07, 16, 30, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 03, 07, 20, 00, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 03, 08, 16, 30, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 03, 08, 20, 00, 00).WithOffset(-3) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 11, 23, 22, 00, +8) });
                        break;
                    case "Lusail":
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = new DateTime(2024, 03, 07, 16, 30, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = new DateTime(2024, 03, 07, 20, 00, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = new DateTime(2024, 03, 08, 16, 30, 00).WithOffset(-3) });
                        //@event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = new DateTime(2024, 03, 08, 20, 00, 00).WithOffset(-3) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 12, 01, 19, 00, -3) });
                        break;
                    case "Yas Marina":
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice1, Start = GetDateTimeWithOffset(2024, 12, 06, 13, 30, -4) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice2, Start = GetDateTimeWithOffset(2024, 12, 06, 18, 00, -4) });
                        @event.Sessions.Add(new Session { Type = SessionType.FreePractice3, Start = GetDateTimeWithOffset(2024, 12, 07, 14, 30, -4) });
                        @event.Sessions.Add(new Session { Type = SessionType.Qualification, Start = GetDateTimeWithOffset(2024, 12, 07, 18, 00, -4) });
                        @event.Sessions.Add(new Session { Type = SessionType.Race, Start = GetDateTimeWithOffset(2024, 12, 08, 17, 00, -4) });
                        break;
                    default:
                        break;
                }

                season.Events.Add(@event);
            }
        }

        private DateTime GetDateTimeWithOffset(int year, int month, int day, int hour, int minutes, int offsetHours)
        {
            return new DateTimeOffset(new DateTime(year, month, day, hour, minutes, 00), TimeSpan.FromHours(offsetHours)).DateTime;
        }
    }
}
