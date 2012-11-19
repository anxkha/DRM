using DOTP.Database;
using DOTP.RaidManager.Threading;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace DOTP.RaidManager.Repository
{
    public class RaidSignupStore
    {
        private ReaderWriterLock _lock;

        private static string RAID_SIGNUP_SELECT = @"
SELECT [RaidInstanceID], [Character], [Comment], [IsRostered], [IsCancelled], [RosteredSpecialization], [SignupDate]
FROM [RaidSignup]
";

        private static string RAID_SIGNUP_SELECT_BY_INSTANCE = RAID_SIGNUP_SELECT + " WHERE ([RaidInstanceID] = @RaidInstanceID)";

        private static string RAID_SIGNUP_SELECT_BY_ONE = RAID_SIGNUP_SELECT_BY_INSTANCE + " AND ([Character] = @Character)";

        private static string RAID_SIGNUP_ADD = @"
INSERT INTO [RaidSignup]
    ([RaidInstanceID], [Character], [Comment], [IsRostered], [IsCancelled], [RosteredSpecialization], [SignupDate])
VALUES
    (@RaidInstanceID, @Character, @Comment, 0, 0, @RosteredSpecialization, @SignupDate)
";

        private static string RAID_SIGNUP_CANCEL = @"
UPDATE [RaidSignup]
SET [IsCancelled] = 1, [IsRostered] = 0
WHERE ([Character] = @Character) AND ([RaidInstanceID] = @RaidInstanceID)
";

        private static string RAID_SIGNUP_RESTORE = @"
UPDATE [RaidSignup]
SET [IsCancelled] = 0, [IsRostered] = 0
WHERE ([Character] = @Character) AND ([RaidInstanceID] = @RaidInstanceID)
";

        private static string RAID_SIGNUP_DELETE = @"
DELETE FROM [RaidSignup]
WHERE ([Character] = @Character) AND ([RaidInstanceID] = @RaidInstanceID)
";

        public RaidSignupStore()
        {
            _lock = new ReaderWriterLock();
        }

        public RaidSignup ReadOneOrDefault(int raidInstanceId, string character)
        {
            RaidSignup newSignup = null;

            using (new ReaderLock(_lock))
            {
                Connection.ExecuteSql(new Query(RAID_SIGNUP_SELECT_BY_ONE)
                    .AddParam("RaidInstanceID", raidInstanceId)
                    .AddParam("Character", character), delegate(SqlDataReader reader)
                {
                    if (!reader.Read())
                        return;

                    newSignup = new RaidSignup
                    {
                        RaidInstanceID = (int)reader[0],
                        Character = reader[1].ToString(),
                        Comment = reader[2].ToString(),
                        IsRostered = (bool)reader[3],
                        IsCancelled = (bool)reader[4],
                        RosteredSpecialization = (int)reader[5],
                        SignupDate = (DateTime)reader[6]
                    };
                });
            }

            return newSignup ?? default(RaidSignup);
        }

        public List<RaidSignup> ReadAll()
        {
            return ReadAllHelper(new Query(RAID_SIGNUP_SELECT));
        }

        public List<RaidSignup> ReadAll(int raidInstanceId)
        {
            return ReadAllHelper(new Query(RAID_SIGNUP_SELECT_BY_INSTANCE).AddParam("RaidInstanceID", raidInstanceId));
        }

        private List<RaidSignup> ReadAllHelper(Query query)
        {
            var newList = new List<RaidSignup>();

            using (new ReaderLock(_lock))
            {
                Connection.ExecuteSql(query, delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
                            var newRaidSignup = new RaidSignup
                            {
                                RaidInstanceID = (int)reader[0],
                                Character = reader[1].ToString(),
                                Comment = reader[2].ToString(),
                                IsRostered = (bool)reader[3],
                                IsCancelled = (bool)reader[4],
                                RosteredSpecialization = (int)reader[5],
                                SignupDate = (DateTime)reader[6]
                            };

                            newList.Add(newRaidSignup);
                        }
                    });
            }

            return newList.Count > 0 ? newList : null;
        }

        public bool TryCreate(RaidSignup signup, out string errorMsg)
        {
            if (200 < signup.Comment.Length)
            {
                errorMsg = "A comment longer than 200 was provided. This should not happen.";
                return false;
            }

            if ((1 > signup.RosteredSpecialization) || (2 < signup.RosteredSpecialization))
            {
                errorMsg = "An invalid specialization choice was provided. This should not happen.";
                return false;
            }

            using (new ReaderLock(_lock))
            {
                if (null != ReadOneOrDefault(signup.RaidInstanceID, signup.Character))
                {
                    errorMsg = "A signup for this character for this raid already exists.";
                    return false;
                }

                if (null == Character.Store.ReadOneOrDefault(c => c.Name == signup.Character))
                {
                    errorMsg = "A signup cannot be created for a non-existant character.";
                    return false;
                }

                using (new WriterLock(_lock))
                {
                    if (null != ReadOneOrDefault(signup.RaidInstanceID, signup.Character))
                    {
                        errorMsg = "A signup for this character for this raid already exists.";
                        return false;
                    }

                    if (null == Character.Store.ReadOneOrDefault(c => c.Name == signup.Character))
                    {
                        errorMsg = "A signup cannot be created for a non-existant character.";
                        return false;
                    }

                    var success = false;

                    Connection.ExecuteSql(new Query(RAID_SIGNUP_ADD)
                                            .AddParam("RaidInstanceID", signup.RaidInstanceID)
                                            .AddParam("Character", signup.Character)
                                            .AddParam("Comment", signup.Comment)
                                            .AddParam("RosteredSpecialization", signup.RosteredSpecialization)
                                            .AddParam("SignupDate", signup.SignupDate), delegate(SqlDataReader reader)
                    {
                        if (0 == reader.RecordsAffected)
                            return;

                        success = true;
                    });

                    errorMsg = success ? "Datastore failure when creating the signup. Please contact the administrator." : "";

                    return success;
                }
            }
        }

        public bool TryCancel(string character, int raidInstanceID, out string errorMsg)
        {
            using (new ReaderLock(_lock))
            {
                var signup = ReadOneOrDefault(raidInstanceID, character);

                if (null == signup)
                {
                    errorMsg = "A signup could not be located for that character/raid instance ID pair.";
                    return false;
                }

                if (true == signup.IsCancelled)
                {
                    errorMsg = "That signup is already cancelled.";
                    return false;
                }

                using (new WriterLock(_lock))
                {
                    signup = ReadOneOrDefault(raidInstanceID, character);

                    if (null == signup)
                    {
                        errorMsg = "A signup could not be located for that character/raid instance ID pair.";
                        return false;
                    }

                    if (true == signup.IsCancelled)
                    {
                        errorMsg = "That signup is already cancelled.";
                        return false;
                    }

                    var success = false;

                    Connection.ExecuteSql(new Query(RAID_SIGNUP_CANCEL)
                                            .AddParam("RaidInstanceID", raidInstanceID)
                                            .AddParam("Character", character), delegate(SqlDataReader reader)
                                            {
                                                if (0 == reader.RecordsAffected)
                                                    return;

                                                success = true;
                                            });

                    errorMsg = "";

                    if (success)
                    {
                        signup.IsCancelled = true;
                        signup.IsRostered = false;
                    }
                    else
                        errorMsg = "Datastore failure when cancelling the signup. Please contact the administrator.";

                    return success;
                }
            }
        }

        public bool TryRestore(string character, int raidInstanceID, out string errorMsg)
        {
            using (new ReaderLock(_lock))
            {
                var signup = ReadOneOrDefault(raidInstanceID, character);

                if (null == signup)
                {
                    errorMsg = "A signup could not be located for that character/raid instance ID pair.";
                    return false;
                }

                if (true != signup.IsCancelled)
                {
                    errorMsg = "That signup is not cancelled.";
                    return false;
                }

                using (new WriterLock(_lock))
                {
                    signup = ReadOneOrDefault(raidInstanceID, character);

                    if (null == signup)
                    {
                        errorMsg = "A signup could not be located for that character/raid instance ID pair.";
                        return false;
                    }

                    if (true != signup.IsCancelled)
                    {
                        errorMsg = "That signup is not cancelled.";
                        return false;
                    }

                    var success = false;

                    Connection.ExecuteSql(new Query(RAID_SIGNUP_RESTORE)
                                            .AddParam("RaidInstanceID", raidInstanceID)
                                            .AddParam("Character", character), delegate(SqlDataReader reader)
                                            {
                                                if (0 == reader.RecordsAffected)
                                                    return;

                                                success = true;
                                            });

                    errorMsg = "";

                    if (success)
                    {
                        signup.IsCancelled = false;
                        signup.IsRostered = false;
                    }
                    else
                        errorMsg = "Datastore failure when restoring the signup. Please contact the administrator.";

                    return success;
                }
            }
        }

        public bool TryDelete(string character, int raidInstanceId, out string errorMsg)
        {
            using (new ReaderLock(_lock))
            {
                var signup = ReadOneOrDefault(raidInstanceId, character);
                if (null == signup)
                {
                    errorMsg = "A signup could not be located for that character/raid instance ID pair.";
                    return false;
                }

                using (new WriterLock(_lock))
                {
                    signup = ReadOneOrDefault(raidInstanceId, character);
                    if (null == signup)
                    {
                        errorMsg = "A signup could not be located for that character/raid instance ID pair.";
                        return false;
                    }

                    var success = false;

                    Connection.ExecuteSql(new Query(RAID_SIGNUP_DELETE)
                                            .AddParam("RaidInstanceID", raidInstanceId)
                                            .AddParam("Character", character), delegate(SqlDataReader reader)
                                            {
                                                if (0 == reader.RecordsAffected)
                                                    return;

                                                success = true;
                                            });

                    errorMsg = "";

                    if (success)
                    {
                        signup.IsCancelled = true;
                        signup.IsRostered = true;
                    }
                    else
                        errorMsg = "Datastore failure when deleting the signup. Please contact the administrator.";

                    return success;
                }
            }
        }
    }
}
