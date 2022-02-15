using Microsoft.EntityFrameworkCore;
using PragueParking2._0.Vehicles;

namespace PragueParking2._0
{
    public class ParkingDbContext : DbContext
    {
        private DbSet<Vehicle> Vehicles { get; set; }
        private DbSet<Car> Cars { get; set; }
        private DbSet<Mc> Mcs { get; set; }
        private DbSet<Bus> Busses { get; set; }
        private DbSet<Bike> Bikes { get; set; }
        private DbSet<ParkingHouse> ParkingHouses { get; set; }
        private DbSet<ParkingSpot> ParkingSpots { get; set; }
    }

}