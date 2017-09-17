using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FindMyDistance.Models;
using System.Data.Common;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using GoogleDistanceMatrix.Services;

namespace FindMyDistance.Controllers
{
    public class DistancesController : Controller
    {
        private readonly FindMyDistanceContext _context;

        public DistancesController(FindMyDistanceContext context)
        {
            _context = context;
        }

        // GET: Distances
        public async Task<IActionResult> Index()
        {

            GoogleDistanceMatrixApi api = new GoogleDistanceMatrixApi(new[] { "Auckland Airport" }, new[] { "Corner Princes Street and Waterloo Quadrant" });
            var response = await api.GetResponse();
            //return Json(response, JsonRequestBehavior.AllowGet);

            return View(await _context.Distance.ToListAsync());
        }

        // GET: Distances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distance = await _context.Distance
                .SingleOrDefaultAsync(m => m.Id == id);
            if (distance == null)
            {
                return NotFound();
            }

            return View(distance);
        }

        // GET: Distances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Distances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,City,Lattitude,Longitude")] Distance distance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(distance);
        }


        // GET: Distances/GetDistance
        public IActionResult GetDistance()
        {
            return View();
        }

        // POST: Distances/GetDistance
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetDistance([Bind("StartLattitude,StartLongitude,EndLattitude,EndLongitude")] GetDistance distance)
        {
            if (ModelState.IsValid)
            {
                //var distanceBetween = await _context.Database.ExecuteSqlCommandAsync("GetDistance @p0, @p1, @p2, @p3", parameters: new[] { distance.StartLattitude, distance.StartLongitude, distance.EndLattitude, distance.EndLongitude});
                var bookIdParameter = new SqlParameter();
                bookIdParameter.ParameterName = "@Distance";
                bookIdParameter.Direction = ParameterDirection.Output;
                bookIdParameter.SqlDbType = SqlDbType.VarChar;
                bookIdParameter.Size = 250;
                var authors = _context.Database.ExecuteSqlCommand("GetDistance @StartLattitude,@StartLongitude,@EndLattitude,@EndLongitude, @Distance OUT",
                    new SqlParameter("@StartLattitude", distance.StartLattitude),
                    new SqlParameter("@StartLongitude", distance.StartLongitude),
                    new SqlParameter("@EndLattitude", distance.EndLattitude),
                    new SqlParameter("@EndLongitude", distance.EndLongitude),
                    bookIdParameter);

                //var affRows = _context.Database.ExecuteSqlCommand("GetDistance @p0, @p1, @p2, @p3, @Distance OUT", distance.StartLattitude, distance.StartLongitude, distance.EndLattitude, distance.EndLongitude, bookIdParameter);
                //Console.WriteLine(bookIdParameter.Value);
                distance.DistanceBetween = bookIdParameter.Value.ToString();
                return View(distance);
            }
            return View("Test");
        }

        // GET: Distances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distance = await _context.Distance.SingleOrDefaultAsync(m => m.Id == id);
            if (distance == null)
            {
                return NotFound();
            }
            return View(distance);
        }

        // POST: Distances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,City,Lattitude,Longitude")] Distance distance)
        {
            if (id != distance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(distance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistanceExists(distance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(distance);
        }

        // GET: Distances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distance = await _context.Distance
                .SingleOrDefaultAsync(m => m.Id == id);
            if (distance == null)
            {
                return NotFound();
            }

            return View(distance);
        }

        // POST: Distances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var distance = await _context.Distance.SingleOrDefaultAsync(m => m.Id == id);
            _context.Distance.Remove(distance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistanceExists(int id)
        {
            return _context.Distance.Any(e => e.Id == id);
        }
    }
}
