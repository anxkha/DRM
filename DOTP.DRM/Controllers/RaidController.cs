using DOTP.DRM.Models;
using DOTP.RaidManager;
using DOTP.Users;
using System.Web.Mvc;

namespace DOTP.DRM.Controllers
{
    public class RaidController : Controller
    {
        #region /Raid

        //
        // GET: /Raid/

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region /Raid/Schedule

        //
        // GET: /Raid/Schedule/

        public ActionResult Schedule()
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            if (!Manager.GetCurrentUser().IsAdmin)
                return RedirectToAction("Index", "Home");

            return View();
        }

        //
        // POST: /Raid/Schedule/

        [HttpPost]
        public ActionResult Schedule(ScheduleRaidModel model)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            if (!Manager.GetCurrentUser().IsAdmin)
                return RedirectToAction("Index", "Home");

            if(model.Name.Length > 100)
                return new JsonResult() { Data = new RaidResponse(false, "The name cannot be more than 100 characters long.") };

            if (model.Description.Length > 1000)
                return new JsonResult() { Data = new RaidResponse(false, "The description cannot be more than 1000 characters long.") };

            var instance = new RaidInstance()
            {
                Raid = model.Raid,
                Name = model.Name,
                Description = model.Description,
                InviteTime = model.InviteTime,
                StartTime = model.StartTime,
                Archived = false
            };

            string errorMsg;

            if (!RaidInstance.Store.TryCreate(instance, out errorMsg))
                return new JsonResult() { Data = new RaidResponse(false, errorMsg) };

            return new JsonResult() { Data = new RaidResponse(true, "Hi!") };
        }

        #endregion

        #region /Raid/Archive

        //
        // GET: /Raid/Archive?ID=<ID>

        public ActionResult Archive(int ID)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            if (!Manager.GetCurrentUser().IsAdmin)
                return RedirectToAction("Index", "Home");

            ViewBag.RaidInstance = RaidInstance.Store.ReadOneOrDefault(ri => ri.ID == ID);

            if (null == ViewBag.RaidInstance)
                return RedirectToAction("Index", "Home");

            return View();
        }

        //
        // POST: /Raid/Archive

        [HttpPost]
        public ActionResult Archive(int ID, string Name)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            if (!Manager.GetCurrentUser().IsAdmin)
                return RedirectToAction("Index", "Home");

            var raidInstance = RaidInstance.Store.ReadOneOrDefault(ri => ri.ID == ID);

            if (null == raidInstance)
                return new JsonResult() { Data = new RaidResponse(false, "Invalid raid instance ID provided for archiving a raid.") };

            string errorMsg;

            if (!RaidInstance.Store.TryArchive(raidInstance, out errorMsg))
                return new JsonResult() { Data = new RaidResponse(false, errorMsg) };

            return new JsonResult() { Data = new RaidResponse(true, "") };
        }

        #endregion

        #region /Raid/UnArchive

        //
        // GET: /Raid/UnArchive?ID=<ID>

        public ActionResult UnArchive(int ID)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            if (!Manager.GetCurrentUser().IsAdmin)
                return RedirectToAction("Index", "Home");

            ViewBag.RaidInstance = RaidInstance.Store.ReadOneOrDefault(ri => ri.ID == ID);

            if (null == ViewBag.RaidInstance)
                return RedirectToAction("Index", "Home");

            return View();
        }

        //
        // POST: /Raid/UnArchive

        [HttpPost]
        public ActionResult UnArchive(int ID, string Name)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            if (!Manager.GetCurrentUser().IsAdmin)
                return RedirectToAction("Index", "Home");

            var raidInstance = RaidInstance.Store.ReadOneOrDefault(ri => ri.ID == ID);

            if (null == raidInstance)
                return new JsonResult() { Data = new RaidResponse(false, "Invalid raid instance ID provided for un-archiving a raid.") };

            string errorMsg;

            if (!RaidInstance.Store.TryUnArchive(raidInstance, out errorMsg))
                return new JsonResult() { Data = new RaidResponse(false, errorMsg) };

            return new JsonResult() { Data = new RaidResponse(true, "") };
        }

        #endregion

        #region /Raid/Edit

        //
        // GET: /Raid/Edit?ID=<ID>

        public ActionResult Edit(int ID)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            if (!Manager.GetCurrentUser().IsAdmin)
                return RedirectToAction("Index", "Home");

            var raidInstance = RaidInstance.Store.ReadOneOrDefault(ri => ri.ID == ID);

            if (null == raidInstance)
                return new JsonResult() { Data = new RaidResponse(false, "Invalid raid instance ID provided for editing a raid instance.") };

            ViewBag.ID = ID;

            var model = new ScheduleRaidModel()
            {
                Raid = raidInstance.Raid,
                Name = raidInstance.Name,
                Description = raidInstance.Description,
                InviteTime = raidInstance.InviteTime,
                StartTime = raidInstance.StartTime
            };

            return View(model);
        }

        //
        // POST: /Raid/Edit

        [HttpPost]
        public ActionResult Edit(int ID, ScheduleRaidModel model)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            if (!Manager.GetCurrentUser().IsAdmin)
                return RedirectToAction("Index", "Home");

            if (model.Name.Length > 100)
                return new JsonResult() { Data = new RaidResponse(false, "The name cannot be more than 100 characters long.") };

            if (model.Description.Length > 1000)
                return new JsonResult() { Data = new RaidResponse(false, "The description cannot be more than 1000 characters long.") };

            if (null == RaidInstance.Store.ReadOneOrDefault(ri => ri.ID == ID))
                return new JsonResult() { Data = new RaidResponse(false, "Invalid raid instance ID provided for editing a raid instance.") };

            var raidInstance = new RaidInstance()
            {
                ID = ID,
                Raid = model.Raid,
                Name = model.Name,
                Description = model.Description,
                InviteTime = model.InviteTime,
                StartTime = model.StartTime
            };

            string errorMsg;

            if (!RaidInstance.Store.TryModify(raidInstance, out errorMsg))
                return new JsonResult() { Data = new RaidResponse(false, errorMsg) };

            return new JsonResult() { Data = new RaidResponse(true, "") };
        }

        #endregion

        #region RaidResponse

        private class RaidResponse
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }

            public RaidResponse(bool success, string errorMsg)
            {
                Success = success;
                ErrorMessage = errorMsg;
            }
        }

        #endregion
    }
}
