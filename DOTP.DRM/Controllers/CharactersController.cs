using DOTP.DRM.Models;
using DOTP.RaidManager;
using DOTP.Users;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DOTP.DRM.Controllers
{
    public class CharactersController : Controller
    {
        #region /Characters/Add

        //
        // GET: /Characters/Add

        public ActionResult Add()
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            return View();
        }

        //
        // POST: /Characters/Add

        [HttpPost]
        public ActionResult Add(AddCharacterModel model)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            var newChar = new Character()
            {
                Name = model.Name,
                Level = int.Parse(model.Level),
                Race = model.Race,
                Class = model.Class,
                AccountID = Manager.GetCurrentUser().ID,
                PrimarySpecialization = model.PrimarySpecialization,
                SecondarySpecialization = model.SecondarySpecialization
            };

            string errorMsg;

            if (!Character.Store.TryCreate(newChar, out errorMsg))
                return new JsonResult() { Data = new AddCharacterResponse(false, errorMsg) };

            return new JsonResult() { Data = new AddCharacterResponse(true, "") };
        }

        #endregion

        #region /Characters/Edit

        //
        // GET: /Characters/Edit/?Name=<Name>

        public ActionResult Edit(string Name)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            var character = Character.Store.ReadOneOrDefault(c => c.Name == Name);

            if (null == character)
                return RedirectToAction("Index", "Characters");

            if (Manager.GetCurrentUser().ID != character.AccountID)
                return RedirectToAction("Index", "Characters");

            var model = new EditCharacterModel()
            {
                OldName = character.Name,
                Name = character.Name,
                Level = character.Level.ToString(),
                Race = character.Race,
                Class = character.Class,
                PrimarySpecialization = character.PrimarySpecialization,
                SecondarySpecialization = character.SecondarySpecialization
            };

            return View(model);
        }

        //
        // POST: /Characters/Edit

        [HttpPost]
        public ActionResult Edit(EditCharacterModel model)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            var character = new Character()
            {
                Name = model.Name,
                Level = int.Parse(model.Level),
                Race = model.Race,
                Class = model.Class,
                AccountID = Manager.GetCurrentUser().ID,
                PrimarySpecialization = model.PrimarySpecialization,
                SecondarySpecialization = model.SecondarySpecialization
            };

            string errorMsg;

            if (!Character.Store.TryModify(model.OldName, character, out errorMsg))
                return new JsonResult() { Data = new AddCharacterResponse(false, errorMsg) };

            return new JsonResult() { Data = new AddCharacterResponse(true, "") };
        }

        #endregion

        #region /Characters/Delete

        //
        // GET: /Characters/Delete/?Name=<Name>

        public ActionResult Delete(string Name)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            ViewBag.Character = Character.Store.ReadOneOrDefault(c => c.Name == Name);

            if (null == ViewBag.Character)
                return RedirectToAction("Index", "Characters");

            if (Manager.GetCurrentUser().ID != ViewBag.Character.AccountID)
                return RedirectToAction("Index", "Characters");

            return View();
        }

        //
        // POST: /Characters/Delete

        [HttpPost]
        public ActionResult Delete(string Name, int AccountID)
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            string errorMsg;

            if (!Character.Store.TryDelete(Name, out errorMsg))
                return new JsonResult() { Data = new AddCharacterResponse(false, errorMsg) };

            return new JsonResult() { Data = new AddCharacterResponse(true, "") };
        }

        #endregion

        #region /Characters/

        //
        // GET: /Characters/

        public ActionResult Index()
        {
            if (!Manager.IsReallyAuthenticated(Request))
                return RedirectToAction("LogOn", "Account");

            ViewBag.Characters = Character.Store.ReadAll(c => c.AccountID == Manager.GetCurrentUser().ID);

            return View();
        }

        #endregion

        #region AddCharacterResponse

        private class AddCharacterResponse
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }

            public AddCharacterResponse(bool success, string errorMsg)
            {
                Success = success;
                ErrorMessage = errorMsg;
            }
        }

        #endregion
    }
}
