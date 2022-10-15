using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using otel_rezervasyonu.Identity;
using otel_rezervasyonu.Models;
using System.Diagnostics;


namespace otel_rezervasyonu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustomIdentityDbContext customIdentityDbContext;
        private readonly UserManager<CustomUser> userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<CustomUser> userManager)
        {
            _logger = logger;
            var contextOptions = new DbContextOptionsBuilder<CustomIdentityDbContext>()
             .UseSqlServer(@"Server=DESKTOP-J0S4R9L;Database=Otel;Trusted_Connection=true;")
             .Options;
            customIdentityDbContext = new CustomIdentityDbContext(contextOptions);
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            List<Rooms> rooms = new List<Rooms>();
            RoomInformation roomInformation = new RoomInformation();
            rooms.AddRange(customIdentityDbContext.Rooms.Where(x=>x.situation == false).ToList());
            List<Rooms> ClosedRooms = new List<Rooms>();
            ClosedRooms.AddRange(customIdentityDbContext.Rooms.Where(x => x.situation == true).ToList());
            RoomRentalViewModel roomRentalViewModel = new RoomRentalViewModel();
            roomRentalViewModel.Rooms = rooms;
            roomRentalViewModel.RoomInformation = roomInformation;
            roomRentalViewModel.ClosedRooms = ClosedRooms;

            return View(roomRentalViewModel);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Privacy(RoomInformation model)
        {          
            if (!ModelState.IsValid)
                return RedirectToAction("Privacy");
            else
            {
                if (model.DateOfEntry >= model.LeavingDate)
                    return RedirectToAction("Privacy");

                if (model.NumberOfCustomers == 0)
                    return RedirectToAction("Privacy");

                if (model.RoomNo == 0)
                    return RedirectToAction("Privacy");

                var user = User.Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                
                if(user == null)
                    return RedirectToAction("Privacy");


                var room = customIdentityDbContext.Set<Rooms>().FirstOrDefault(x=>x.no == model.RoomNo);
                
           
                if (room != null)
                    customIdentityDbContext.Entry(room).State = EntityState.Detached;
              
 
                int price = room != null ? room.price : 0;
                
                if(price == 0)
                    return RedirectToAction("Privacy");


                try
                {
                    var girisGunu = model.DateOfEntry;
                    var cikisGunu = model.LeavingDate;


                    int sayac = 0;
                    int activeprice = price;

                    do
                    {
                        if (model.DateOfEntry.DayOfWeek == DayOfWeek.Saturday || model.DateOfEntry.DayOfWeek == DayOfWeek.Sunday)
                        {
                            sayac++;
                        }
                        model.DateOfEntry = model.DateOfEntry.AddDays(1);
                    } while (model.DateOfEntry <= model.LeavingDate.Date);


                    if (model.NumberOfCustomers == 3)
                        activeprice = activeprice + (activeprice * 20) / 100;

                    if (model.NumberOfCustomers == 1)
                        activeprice = activeprice - (activeprice * 30) / 100;



                    var haftasonutoplamfiyat = (activeprice + ((activeprice * 30) / 100)) * sayac;

                    var haftaicitoplamgun = (cikisGunu.Day - girisGunu.Day) - sayac;

                    var haftaicitoplamfiyat = activeprice * haftaicitoplamgun;

                    var toplamfiyat = haftaicitoplamfiyat + haftasonutoplamfiyat;


                    Rooms rooms = new Rooms();
                    rooms.activeprice = toplamfiyat;
                    rooms.no = model.RoomNo;
                    rooms.situation = true;
                    rooms.numberofactivebeds = model.NumberOfCustomers;
                    rooms.price = price;
                    rooms.numberofbeds = 2;
                    rooms.renterid = user.Value;



                    customIdentityDbContext.Entry<Rooms>(rooms).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    customIdentityDbContext.SaveChanges();


                    return RedirectToAction("Privacy", new { situation = true });
                }
                catch (Exception)
                {
                    return RedirectToAction("Privacy");
                }
              
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}