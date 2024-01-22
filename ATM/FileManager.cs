using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Console;

namespace ATM
    {
    public class FileManager
        {
        private string UserDataFileName = "UserData.json";

        public List<User> ReadUserData()
            {
            try
                {
                if (File.Exists(UserDataFileName))
                    {
                    string json = File.ReadAllText(UserDataFileName);
                    return JsonConvert.DeserializeObject<List<User>>(json);
                    }
                }
            catch (Exception ex)
                {
                WriteLine($"Error reading user data: {ex.Message}");
                }

            return new List<User>();
            }

        public void WriteUserData(List<User> users)
            {
            try
                {
                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText(UserDataFileName, json);
                }
            catch (Exception ex)
                {
                WriteLine($"Error writing user data: {ex.Message}");
                }

            }
        }
    }
