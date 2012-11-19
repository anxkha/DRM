using DOTP.DRM.Models;
using DOTP.RaidManager;
using DOTP.Users;
using System;
using System.Collections.Generic;
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

        #region /Raid/Signup

        //
        // GET: /Raid/Signup?ID=<ID>

        public ActionResult Signup(int ID)
        {
            var raidDetails = new RaidDetails(ID);

            if (!raidDetails.Initialize())
                return RedirectToAction("Index", "Home");

            ViewBag.RaidDetails = raidDetails;

            if (null == raidDetails.Signups)
            {
                ViewBag.NumRostered = 0;
                ViewBag.NumQueued = 0;
                ViewBag.NumCancelled = 0;
                ViewBag.NumTotal = 0;
                ViewBag.PercentageRostered = 0;
                ViewBag.PercentageQueued = 0;
                ViewBag.PercentageCancelled = 0;
            }
            else
            {
                ViewBag.NumTotal = raidDetails.Signups.Count;
                ViewBag.NumCancelled = raidDetails.Signups.FindAll(s => s.IsCancelled).Count;
                ViewBag.NumQueued = raidDetails.Signups.FindAll(s => !s.IsCancelled && !s.IsRostered).Count;
                ViewBag.NumRostered = raidDetails.Signups.FindAll(s => s.IsRostered).Count;
                ViewBag.PercentageRostered = 0 == ViewBag.NumTotal ? 0 : (int)((ViewBag.NumRostered / ViewBag.NumTotal) * 100);
                ViewBag.PercentageQueued = 0 == ViewBag.NumTotal ? 0 : (int)((ViewBag.NumQueued / ViewBag.NumTotal) * 100);
                ViewBag.PercentageCancelled = 0 == ViewBag.NumTotal ? 0 : (int)((ViewBag.NumCancelled / ViewBag.NumTotal) * 100);
            }

            return View();
        }

        //
        // POST: /Raid/NewSignup

        [HttpPost]
        public ActionResult NewSignup(int RaidInstanceID, string Character, string Comment)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            var newSignup = new RaidSignup()
            {
                RaidInstanceID = RaidInstanceID,
                Character = Character,
                Comment = Comment,
                IsRostered = false,
                IsCancelled = false,
                RosteredSpecialization = 1,
                SignupDate = DateTime.Now
            };

            string errorMsg;

            if (!RaidSignup.Store.TryCreate(newSignup, out errorMsg))
                return new JsonResult() { Data = new RaidResponse(false, errorMsg) };

            return new JsonResult() { Data = new RaidResponse(true, "") };
        }

        //
        // POST: /Raid/CancelSignup

        [HttpPost]
        public ActionResult CancelSignup(int RaidInstanceID, string Character)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            string errorMsg;

            if (!RaidSignup.Store.TryCancel(Character, RaidInstanceID, out errorMsg))
                return new JsonResult() { Data = new RaidResponse(false, errorMsg) };

            return new JsonResult() { Data = new RaidResponse(true, "") };
        }

        //
        // POST: /Raid/RestoreSignup

        [HttpPost]
        public ActionResult RestoreSignup(int RaidInstanceID, string Character)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            string errorMsg;

            if (!RaidSignup.Store.TryRestore(Character, RaidInstanceID, out errorMsg))
                return new JsonResult() { Data = new RaidResponse(false, errorMsg) };

            return new JsonResult() { Data = new RaidResponse(true, "") };
        }

        //
        // POST: /Raid/DeleteSignup

        [HttpPost]
        public ActionResult DeleteSignup(int RaidInstanceID, string Character)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            string errorMsg;

            if (!RaidSignup.Store.TryDelete(Character, RaidInstanceID, out errorMsg))
                return new JsonResult() { Data = new RaidResponse(false, errorMsg) };

            return new JsonResult() { Data = new RaidResponse(true, "") };
        }

        #endregion

        #region /Raid/Roster

        //
        // GET: /Raid/Roster?ID=<ID>

        public ActionResult Roster(int ID)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            if (!Manager.GetCurrentUser().IsAdmin)
                return RedirectToAction("Index", "Home");

            var raidDetails = new RaidDetails(ID);

            if (!raidDetails.Initialize())
                return RedirectToAction("Index", "Home");

            ViewBag.RaidDetails = raidDetails;

            var tankSignups = new List<RaidSignup>();
            var healerSignups = new List<RaidSignup>();
            var rangedSignups = new List<RaidSignup>();
            var meleeSignups = new List<RaidSignup>();

            foreach(var signup in raidDetails.Signups)
            {
                var character = Character.Store.ReadOneOrDefault(c => c.Name == signup.Character);
                var specialization = Specialization.Store.ReadOneOrDefault(s => s.ID == (1 == signup.RosteredSpecialization ? character.PrimarySpecialization : character.SecondarySpecialization));

                if ("Tank" == specialization.Role)
                    tankSignups.Add(signup);
                else if ("Healer" == specialization.Role)
                    healerSignups.Add(signup);
                else if ("Ranged" == specialization.Role)
                    rangedSignups.Add(signup);
                else
                    meleeSignups.Add(signup);
            }

            ViewBag.TankSignups = tankSignups;
            ViewBag.HealerSignups = healerSignups;
            ViewBag.RangedSignups = rangedSignups;
            ViewBag.MeleeSignups = meleeSignups;

            if (null == raidDetails.Signups)
            {
                ViewBag.NumRostered = 0;
                ViewBag.NumQueued = 0;
                ViewBag.NumCancelled = 0;
                ViewBag.NumTotal = 0;
                ViewBag.PercentageRostered = 0;
                ViewBag.PercentageQueued = 0;
                ViewBag.PercentageCancelled = 0;
            }
            else
            {
                ViewBag.NumTotal = raidDetails.Signups.Count;
                ViewBag.NumCancelled = raidDetails.Signups.FindAll(s => s.IsCancelled).Count;
                ViewBag.NumQueued = raidDetails.Signups.FindAll(s => !s.IsCancelled && !s.IsRostered).Count;
                ViewBag.NumRostered = raidDetails.Signups.FindAll(s => s.IsRostered).Count;
                ViewBag.PercentageRostered = 0 == ViewBag.NumTotal ? 0 : (int)((ViewBag.NumRostered / ViewBag.NumTotal) * 100);
                ViewBag.PercentageQueued = 0 == ViewBag.NumTotal ? 0 : (int)((ViewBag.NumQueued / ViewBag.NumTotal) * 100);
                ViewBag.PercentageCancelled = 0 == ViewBag.NumTotal ? 0 : (int)((ViewBag.NumCancelled / ViewBag.NumTotal) * 100);
            }

            return View();
        }

        //
        // POST: /Raid/UpdateRostered

        [HttpPost]
        public ActionResult UpdateRostered(int RaidInstanceID, string Characters)
        {
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
