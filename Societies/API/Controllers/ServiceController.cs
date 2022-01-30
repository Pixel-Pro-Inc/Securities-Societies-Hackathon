using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ServiceController : BaseApiController
    {
        private readonly IFileService _fileService;
        private readonly CommunicationsController _communicationsController;

        public ServiceController(IFileService fileService)
        {
            _fileService = fileService;
            _communicationsController = new CommunicationsController();
        }

        #region Registration
        [Authorize]
        [HttpPost("reg/submit/{id}")]
        public async Task<ActionResult<bool>> RegSubmit(string id, NameAuthDto nameAuthDto)
        {
            var apps = await _firebaseDataContext.GetData<NameAuth>("NameAuthorisation");

            NameAuth nameAuth = apps.Where(a => a.Id == Int32.Parse(id)).ToList()[0]; ;

            foreach (var item in nameAuthDto.files)
            {
                string[] vals = await GetNameAndData(item);

                var result = await _fileService.Submit(vals[0].Trim(), vals[1].Trim());

                if (result != null)
                {
                    nameAuth.FileUrls.Add(result);
                    nameAuth.FileNames.Add(vals[1].Trim());
                }
            }

            nameAuth.Review = true;
            nameAuth.Accepted = false;

            _firebaseDataContext.DeleteData("Registration/" + nameAuth.Id);

            _firebaseDataContext.StoreData("Registration/" + nameAuth.Id, nameAuth);

            Communicate(nameAuth.User, "We have received your application for registration we will respond as soon as possible.", "Registration of " + nameAuth.SocietyType + " Society");

            return true;
        }
        #endregion

        #region AdminEditApplication
        [Authorize]
        [HttpPost("editappadmin")]
        public async Task<ActionResult<bool>> EditAppAdmin(EditAppDto editAppDto)
        {
            if(editAppDto.stage == "Name Authorisation")
            {
                var apps = await _firebaseDataContext.GetData<NameAuth>("NameAuthorisation");

                NameAuth nameAuth = apps.Where(a => a.Id == editAppDto.Id).ToList()[0];

                nameAuth.Review = editAppDto.review;
                nameAuth.Rejected = editAppDto.rejected;
                nameAuth.Accepted = editAppDto.accepted;
                nameAuth.AcceptedNames = editAppDto.nameApproved;
                nameAuth.Admin = await GetUser(editAppDto.admin);
                nameAuth.Corrections = editAppDto.corrections;
                nameAuth.DateReviewed = System.DateTime.Now;
                nameAuth.Feedback = editAppDto.feedback != null? await ConvertToList(editAppDto.feedback): new List<string>();

                _firebaseDataContext.EditData("NameAuthorisation/" + editAppDto.Id, nameAuth);

                Communicate(nameAuth.User, "We have responded to your name authorisation application. Please login in to the Blue Union platform and navigate to /myapplications as soon as possible.", "Name Authorisation of " + nameAuth.SocietyType + " Result");
            }

            if (editAppDto.stage == "Registration")
            {
                var apps = await _firebaseDataContext.GetData<NameAuth>("Registration");

                NameAuth nameAuth = apps.Where( a => a.Id == editAppDto.Id).ToList()[0];

                if (!string.IsNullOrEmpty(editAppDto.certificate))
                {
                    string[] vals = await GetNameAndData(editAppDto.certificate);

                    var result = await _fileService.Submit(vals[0].Trim(), vals[1].Trim());

                    if (result != null)
                    {
                        nameAuth.FileUrls.Add(result);
                        nameAuth.FileNames.Add(vals[1].Trim());
                    }
                }                

                nameAuth.Review = editAppDto.review;
                nameAuth.Rejected = editAppDto.rejected;
                nameAuth.Accepted = editAppDto.accepted;
                nameAuth.AcceptedNames = editAppDto.nameApproved;
                nameAuth.Admin = await GetUser(editAppDto.admin);
                nameAuth.Corrections = editAppDto.corrections;
                nameAuth.DateReviewed = System.DateTime.Now;
                nameAuth.Feedback = editAppDto.feedback != null ? await ConvertToList(editAppDto.feedback) : new List<string>();

                _firebaseDataContext.EditData("Registration/" + editAppDto.Id, nameAuth);

                Communicate(nameAuth.User, "We have responded to your Registration application. Please login in to the Blue Union platform and navigate to /myapplications as soon as possible.", "Registration of " + nameAuth.SocietyType + " Result");
            }

            return true;
        }
        #endregion

        #region GetApplications
        [Authorize]
        [HttpGet("getmyapplications/{accountId}")]
        public async Task<ActionResult<List<IDictionary<string, object>>>> GetApps(string accountId)
        {
            User user = await GetUser(accountId);

            List<IDictionary<string, object>> keyValues = new List<IDictionary<string, object>>();

            List<NameAuth> result = await _firebaseDataContext.GetData<NameAuth>("NameAuthorisation");

            keyValues.AddRange(GetAppsUser(result, user, false));

            result = await _firebaseDataContext.GetData<NameAuth>("Registration");

            keyValues.AddRange(GetAppsUser(result, user, true));

            return keyValues;
        }

        List<IDictionary<string, object>> GetAppsUser(List<NameAuth> result, User user, bool registration)
        {
            var myApps = result.Where(r => r.User.Email == user.Email || r.User.PhoneNumber == user.PhoneNumber).ToList();

            List<IDictionary<string, object>> keyValues = new List<IDictionary<string, object>>();

            foreach (var item in myApps)
            {
                IDictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add("corrections", item.Corrections);
                keyValuePairs.Add("rejected", item.Rejected);
                keyValuePairs.Add("accepted", item.Accepted);
                keyValuePairs.Add("review", item.Review);
                keyValuePairs.Add("type", item.SocietyType);
                keyValuePairs.Add("stage", registration? "Registration" : "Name Authorisation");
                keyValuePairs.Add("feedback", item.Feedback);
                keyValuePairs.Add("Id", item.Id);
                keyValuePairs.Add("fileUrls", item.FileUrls);

                keyValues.Add(keyValuePairs);
            }

            return keyValues;
        }

        [Authorize]
        [HttpGet("getapplications")]
        public async Task<ActionResult<List<IDictionary<string, object>>>> GetAllApps()
        {
            List<NameAuth> result = await _firebaseDataContext.GetData<NameAuth>("NameAuthorisation");

            List<IDictionary<string, object>> keyValues = new List<IDictionary<string, object>>();

            keyValues.AddRange(GetAppsAdmin(result, false));

            result = await _firebaseDataContext.GetData<NameAuth>("Registration");

            keyValues.AddRange(GetAppsAdmin(result, true));

            return keyValues;
        }

        List<IDictionary<string, object>> GetAppsAdmin(List<NameAuth> result, bool registration)
        {
            List<IDictionary<string, object>> keyValues = new List<IDictionary<string, object>>();

            foreach (var item in result)
            {
                IDictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add("corrections", item.Corrections);
                keyValuePairs.Add("rejected", item.Rejected);
                keyValuePairs.Add("accepted", item.Accepted);
                keyValuePairs.Add("review", item.Review);
                keyValuePairs.Add("type", item.SocietyType);
                keyValuePairs.Add("stage", registration? "Registration" : "Name Authorisation");
                keyValuePairs.Add("feedback", item.Feedback);
                keyValuePairs.Add("Id", item.Id);
                keyValuePairs.Add("date", item.DateSubmitted.ToLongDateString());
                keyValuePairs.Add("firstname", item.User.FirstName);
                keyValuePairs.Add("lastname", item.User.LastName);
                keyValuePairs.Add("email", item.User.Email);
                keyValuePairs.Add("names", item.Names);
                keyValuePairs.Add("objectives", item.Objectives);
                keyValuePairs.Add("fileUrls", item.FileUrls);
                keyValuePairs.Add("fileNames", item.FileNames);

                keyValues.Add(keyValuePairs);
            }

            return keyValues;
        }
        #endregion

        #region NameAuthorisationEdit
        [Authorize]
        [HttpPost("nameauth/edit/{id}")]
        public async Task<ActionResult<bool>> NameAuthorisationEdit(string id, NameAuthDto nameAuthDto)
        {
            NameAuth nameAuth = new NameAuth()
            {
                Names = await ConvertToList(nameAuthDto.societyNames),
                Objectives = await ConvertToList(nameAuthDto.objectives),
                SocietyType = nameAuthDto.societyType,
                User = await GetUser(nameAuthDto.accountId),
                Review = true,
                Corrections = false,
                DateSubmitted = DateTime.Now
            };

            nameAuth.Id = Int32.Parse(id);

            nameAuth.FileUrls = new List<string>();
            nameAuth.FileNames = new List<string>();

            foreach (var item in nameAuthDto.files)
            {
                string[] vals = await GetNameAndData(item);

                var result = await _fileService.Submit(vals[0].Trim(), vals[1].Trim());

                if (result != null)
                {
                    nameAuth.FileUrls.Add(result);
                    nameAuth.FileNames.Add(vals[1].Trim());
                }
            }

            _firebaseDataContext.EditData("NameAuthorisation/" + nameAuth.Id, nameAuth);

            Communicate(nameAuth.User, "We have received your name authorisation corrections you will receive a response as soon as possible.", "Name Authorisation Correction of " + nameAuth.SocietyType);

            return true;
        }
        #endregion

        #region NameAuthorisation
        [Authorize]
        [HttpPost("nameauth/submit")]
        public async Task<ActionResult<bool>> NameAuthorisation(NameAuthDto nameAuthDto)
        {
            NameAuth nameAuth = new NameAuth()
            {
                Names = await ConvertToList(nameAuthDto.societyNames),
                Objectives = await ConvertToList(nameAuthDto.objectives),
                SocietyType = nameAuthDto.societyType,
                User = await GetUser(nameAuthDto.accountId),
                Review = true,
                DateSubmitted = DateTime.Now
            };

            nameAuth.Id = await GetId();

            nameAuth.FileUrls = new List<string>();
            nameAuth.FileNames = new List<string>();

            foreach (var item in nameAuthDto.files)
            {
                string[] vals = await GetNameAndData(item);

                var result = await _fileService.Submit(vals[0].Trim(), vals[1].Trim());

                if (result != null)
                {
                    nameAuth.FileUrls.Add(result);
                    nameAuth.FileNames.Add(vals[1].Trim());
                }                    
            }

            _firebaseDataContext.StoreData("NameAuthorisation/" + nameAuth.Id, nameAuth);

            Communicate(nameAuth.User, "We have received your name authorisation application you will receive a response as soon as possible.", "Name Authorisation of " + nameAuth.SocietyType);

            return true;
        }

        private async Task<string[]> GetNameAndData(string input)
        {
            string[] vals = new string[2];

            foreach (char item in input)
            {
                if(item == ',')
                {
                    vals[1] = input.Substring(0, input.IndexOf(item));
                    vals[0] = input.Substring(input.IndexOf(item) + 1, input.Length - (input.IndexOf(item) + 1));

                    break;
                }
            }

            return vals;
        }

        private async Task<List<string>> ConvertToList(string input)
        {
            string[] stringsArray = input.Split(',');
            List<string> stringsList = new List<string>(stringsArray.Length);
            stringsList.AddRange(stringsArray);

            List<string> result = new List<string>();

            foreach (string item in stringsList)
            {
                result.Add(item.Trim());
            }

            return result;
        }

        private async Task<User> GetUser(string accountID)
        {
            List<User> users = await _firebaseDataContext.GetData<User>("Account");
            return users.Where(u => u.Email == accountID || u.PhoneNumber.ToString() == accountID).ToList().Count != 0 ? users.Where(u => u.Email == accountID || u.PhoneNumber.ToString() == accountID).ToList()[0] : null;
        }

        async Task<int> GetId()
        {
            List<NameAuth> nameAuths = await _firebaseDataContext.GetData<NameAuth>("NameAuthorisation");

            nameAuths.AddRange(await _firebaseDataContext.GetData<NameAuth>("Registration"));

            bool search = true;
            int lastId = 0;
            while (search)
            {
                lastId = new Random().Next(10000, 99999);

                List<int> ids = new List<int>();
                
                foreach (var item in nameAuths)
                {
                    ids.Add(item.Id);
                }

                search = ids.Contains(lastId);
            }

            return lastId;
        }
        #endregion

        void Communicate(User user, string msg, string subject)
        {
            _communicationsController.SendMessage(user.PhoneNumber.ToString(), user.Email,
                msg,
                subject);
        }
    }
}