using Gostinica.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace Gostinica.DataAccess.DAO
{
    public class HotelRepository
    {

        private readonly AppDbContext _context;

        public HotelRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Room> GetAllRooms()
        {
            return _context.Room.Include(r => r.Bookings).ToList();
        }
        public List<Guest> GetCurrentGuests()
        {
            var now = DateTime.Now;
            return _context.Booking
                .Include(b => b.Guest)
                .Where(b => b.Status == "CheckedIn" && b.CheckInDate <= now && b.CheckOutDate >= now)
                .Select(b => b.Guest)
                .Distinct()
                .ToList();
        }
        public Guest GetGuestById(int id)
        {
            var guest = _context.Guest
                .Include(g => g.Bookings)
                    .ThenInclude(b => b.Room)
                .Include(g => g.Bookings)
                    .ThenInclude(b => b.UsedServices)
                .Where(g => g.GuestId == id)
                .FirstOrDefault();

            if (guest != null) return guest;
            return null!;
        }
        public decimal CalculateGuestStayCost(int guestId)
        {
            var bookings = _context.Booking
                .Include(b => b.Room)
                .Include(b => b.UsedServices)
                .Where(b => b.GuestId == guestId && b.Status != "Cancelled")
                .ToList();

            return bookings.Sum(b => CalculateTotalForBooking(b));
        }
        public decimal CalculateAllGuestsStayCostInPeriod(DateTime startDate, DateTime endDate)
        {
            var bookings = _context.Booking
                .Include(b => b.Room)
                .Where(b => b.Status != "Cancelled" && b.CheckInDate < endDate && b.CheckOutDate > startDate)
                .ToList();

            decimal total = 0;
            foreach (var b in bookings)
            {
                var actualStart = b.CheckInDate > startDate ? b.CheckInDate : startDate;
                var actualEnd = b.CheckOutDate < endDate ? b.CheckOutDate : endDate;
                var days = Math.Max(1, (actualEnd.Date - actualStart.Date).Days);
                total += days * b.Room.PricePerDay;
            }
            return total;
        }
        public List<HotelService> GetAllServices()
        {
            return _context.HotelService.ToList();
        }
        public List<Room> GetAvailableRoomsNow()
        {
            var now = DateTime.Now;
            var occupiedRoomIds = _context.Booking
                .Where(b => (b.Status == "CheckedIn" || b.Status == "Reserved") && b.CheckInDate <= now && b.CheckOutDate > now)
                .Select(b => b.RoomId)
                .ToList();

            return _context.Room.Where(r => !occupiedRoomIds.Contains(r.RoomId)).ToList();
        }
        public List<Room> GetAvailableRoomsInPeriod(DateTime startDate, DateTime endDate)
        {
            var busyRoomIds = _context.Booking
                .Where(b => b.Status != "Cancelled" && b.CheckInDate < endDate && b.CheckOutDate > startDate)
                .Select(b => b.RoomId)
                .ToList();

            return _context.Room.Where(r => !busyRoomIds.Contains(r.RoomId)).ToList();
        }
        public List<Room> GetReservedRooms()
        {
            return _context.Booking
                .Include(b => b.Room)
                .Where(b => b.Status == "Reserved")
                .Select(b => b.Room)
                .Distinct()
                .ToList();
        }
        public List<Guest> GetDebtorGuests()
        {
            var bookings = _context.Booking
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .Include(b => b.UsedServices)
                .Where(b => b.Status != "Cancelled")
                .ToList();

            return bookings
                .Where(b => b.PaidAmount < CalculateTotalForBooking(b))
                .Select(b => b.Guest)
                .DistinctBy(g => g.GuestId)
                .ToList();
        }
        public async Task<Room> CreateRoom(Room room)
        {
            await _context.Room.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Guest> CreateGuest(Guest guest)
        {
            await _context.Guest.AddAsync(guest);
            await _context.SaveChangesAsync();
            return guest;
        }

        public async Task<Employee> CreateEmployee(Employee emp)
        {
            await _context.Employee.AddAsync(emp);
            await _context.SaveChangesAsync();
            return emp;
        }

        public async Task<HotelService> CreateService(HotelService service)
        {
            await _context.HotelService.AddAsync(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<Booking> CreateBooking(Booking booking)
        {
            await _context.Booking.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        private decimal CalculateTotalForBooking(Booking b)
        {
            var days = Math.Max(1, (b.CheckOutDate.Date - b.CheckInDate.Date).Days);
            var servicesSum = b.UsedServices?.Sum(s => s.Price) ?? 0;
            return (days * b.Room.PricePerDay) + servicesSum;
        }
    }
}
