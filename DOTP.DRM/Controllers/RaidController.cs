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

            return View();
        }

        //
        // POST: /Raid/Schedule/

        [HttpPost]
        public ActionResult Schedule(ScheduleRaidModel model)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

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
