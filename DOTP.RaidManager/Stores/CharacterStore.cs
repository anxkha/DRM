using DOTP.Database;
using DOTP.RaidManager.Threading;
using DOTP.Users;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace DOTP.RaidManager.Stores
{
    public class CharacterStore
    {
        private static List<Character> _cache;
        private bool _loaded;
        private ReaderWriterLock _lock;

        private static string CHARACTER_SELECT = @"
SELECT [Name], [Level], [Class], [Race], [AccountID], [PrimarySpecializationID], [SecondarySpecializationID]
FROM [DRM].[dbo].[Character]
";

        private static string CHARACTER_ADD = @"
INSERT INTO [DRM].[dbo].[Character]
    ([Name], [Level], [Class], [Race], [AccountID], [PrimarySpecializationID], [SecondarySpecializationID])
VALUES (@Name, @Level, @Class, @Race, @AccountID, @PrimarySpecialization, @SecondarySpecialization)
";

        private static string CHARACTER_DELETE = @"
DELETE FROM [DRM].[dbo].[Character]
WHERE ([Name] = @Name)
";

        private static string CHARACTER_UPDATE = @"
UPDATE [DRM].[dbo].[Character]
SET [Name] = @Name,
    [Level] = @Level,
    [Race] = @Race,
    [Class] = @Class,
    [PrimarySpecializationID] = @PrimarySpecialization,
    [SecondarySpecializationID] = @SecondarySpecialization
WHERE ([Name] = @OldName)
";

        public CharacterStore()
        {
            _loaded = false;
            _cache = new List<Character>();

            _lock = new ReaderWriterLock();
        }

        public bool TryCreate(Character character, out string errorMsg)
        {
            EnsureLoaded();

            using (new ReaderLock(_lock))
            {
                if (null != Character.Store.ReadOneOrDefault(c => c.Name == character.Name))
                {
                    errorMsg = "A character with that name already exists.";
                    return false;
                }

                if (!RaceClasses.Store.ReadOneOrDefault(character.Race).Classes.Contains(character.Class))
                {
                    errorMsg = string.Format("A {0} cannot be a {1}.", character.Race, character.Class);
                    return false;
                }

                Specialization spec;

                if (!((spec = Specialization.Store.ReadOneOrDefault(s => s.ID == character.PrimarySpecialization)).Class == character.Class))
                {
                    errorMsg = string.Format("A {0} cannot have a specialization of {1}.", character.Class, spec.Name);
                    return false;
                }

                spec = Specialization.Store.ReadOneOrDefault(s => s.Name == "None");

                if (spec.ID != character.SecondarySpecialization)
                {
                    if (!((spec = Specialization.Store.ReadOneOrDefault(s => s.ID == character.SecondarySpecialization)).Class == character.Class))
                    {
                        errorMsg = string.Format("A {0} cannot have a specialization of {1}.", character.Class, spec.Name);
                        return false;
                    }
                }

                var addSuccess = false;

                using (new WriterLock(_lock))
                {
                    if (null != Character.Store.ReadOneOrDefault(c => c.Name == character.Name))
                    {
                        errorMsg = "A character with that name already exists.";
                        return false;
                    }

                    errorMsg = "";

                    Connection.ExecuteSql(new Query(CHARACTER_ADD)
                                            .AddParam("Name", character.Name)
                                            .AddParam("Level", character.Level)
                                            .AddParam("Class", character.Class)
                                            .AddParam("Race", character.Race)
                                            .AddParam("AccountID", character.AccountID)
                                            .AddParam("PrimarySpecialization", character.PrimarySpecialization)
                                            .AddParam("SecondarySpecialization", character.SecondarySpecialization), delegate(SqlDataReader reader)
                    {
                        if (0 == reader.RecordsAffected)
                            return;

                        addSuccess = true;

                    });

                    if (addSuccess)
                        _cache.Add(character);
                    else
                        errorMsg = "Datastore failure when adding the character. Please contact the administrator.";

                    return addSuccess;
                }
            }
        }

        public bool TryDelete(string character, out string errorMsg)
        {
            EnsureLoaded();

            using (new ReaderLock(_lock))
            {
                Character chr;

                if (null == (chr = ReadOneOrDefault(c => c.Name == character)))
                {
                    errorMsg = "Unable to delete a character that does not exist.";
                    return false;
                }

                if (chr.AccountID != Manager.GetCurrentUser().ID)
                {
                    errorMsg = "You do not have permission to delete this character.";
                    return false;
                }

                using (new WriterLock(_lock))
                {
                    if (null == (chr = ReadOneOrDefault(c => c.Name == character)))
                    {
                        errorMsg = "Unable to delete a character that does not exist.";
                        return false;
                    }

                    errorMsg = "";

                    var deleteSuccess = false;

                    Connection.ExecuteSql(new Query(CHARACTER_DELETE)
                                            .AddParam("Name", character), delegate(SqlDataReader reader)
                    {
                        if (0 == reader.RecordsAffected)
                            return;

                        deleteSuccess = true;
                    });

                    if (deleteSuccess)
                        _cache.Remove(chr);
                    else
                        errorMsg = "Datastore failure when deleting the character. Please contact the administrator.";

                    return deleteSuccess;
                }
            }
        }

        public bool TryModify(string oldName, Character character, out string errorMsg)
        {
            EnsureLoaded();

            using (new ReaderLock(_lock))
            {
                Character chr;

                if (null == (chr = ReadOneOrDefault(c => c.Name == oldName)))
                {
                    errorMsg = "Unable to modify a character that does not exist.";
                    return false;
                }

                if (chr.AccountID != Manager.GetCurrentUser().ID)
                {
                    errorMsg = "You do not have permission to modify this character.";
                    return false;
                }

                using (new WriterLock(_lock))
                {
                    if (null == (chr = ReadOneOrDefault(c => c.Name == oldName)))
                    {
                        errorMsg = "Unable to modify a character that does not exist.";
                        return false;
                    }

                    errorMsg = "";

                    var updateSuccess = false;

                    Connection.ExecuteSql(new Query(CHARACTER_UPDATE)
                                            .AddParam("Name", character.Name)
                                            .AddParam("Level", character.Level)
                                            .AddParam("Race", character.Race)
                                            .AddParam("Class", character.Class)
                                            .AddParam("PrimarySpecialization", character.PrimarySpecialization)
                                            .AddParam("SecondarySpecialization", character.SecondarySpecialization)
                                            .AddParam("OldName", oldName), delegate(SqlDataReader reader)
                    {
                        if (0 == reader.RecordsAffected)
                            return;

                        updateSuccess = true;
                    });

                    if (updateSuccess)
                    {
                        chr.Name = character.Name;
                        chr.Level = character.Level;
                        chr.Race = character.Race;
                        chr.Class = character.Class;
                        chr.PrimarySpecialization = character.PrimarySpecialization;
                        chr.SecondarySpecialization = character.SecondarySpecialization;
                    }
                    else
                        errorMsg = "Datastore failure when deleting the character. Please contact the administrator.";

                    return updateSuccess;
                }
            }   
        }

        public Character ReadOneOrDefault(Func<Character, bool> func)
        {
            EnsureLoaded();

            foreach (var spec in _cache)
            {
                if (func(spec))
                    return spec;
            }

            return default(Character);
        }

        public List<Character> ReadAll()
        {
            return ReadAll(c => c.Name != null);
        }

        public List<Character> ReadAll(Func<Character, bool> func)
        {
            EnsureLoaded();

            var newList = new List<Character>();

            foreach (var entry in _cache)
            {
                if (func(entry))
                    newList.Add(entry);
            }

            return newList.Count > 0 ? newList : null;
        }

        private void EnsureLoaded()
        {
            using (new ReaderLock(_lock))
            {
                if (_loaded) return;

                using (new WriterLock(_lock))
                {
                    if (_loaded) return;

                    Connection.ExecuteSql(new Query(CHARACTER_SELECT), delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
                            if (null != _cache.Find(s => reader[0].ToString() == s.Name))
                                return;

                            var newChar = new Character()
                            {
                                Name = reader[0].ToString(),
                                Level = (int)reader[1],
                                Class = reader[2].ToString(),
                                Race = reader[3].ToString(),
                                AccountID = (int)reader[4],
                                PrimarySpecialization = (int)reader[5],
                                SecondarySpecialization = (int)reader[6]
                            };

                            _cache.Add(newChar);
                        }
                    });

                    _loaded = true;
                }
            }
        }
    }
}
